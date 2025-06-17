using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace GenerateRSAKey
{
	public enum KeySize
	{
		_2048 = 2048,
		_4096 = 4096
	}
	public class KeyGenerator
	{
		public static (string privateKeyPem, string jwksJson, string keyId) GenerateRsaKeyAndJwks(KeySize keySize, bool saveInFile = false)
		{
			using (var rsa = RSA.Create((int)keySize))
			{
				string keyId = Guid.NewGuid().ToString();
				var securityKey = new RsaSecurityKey(rsa)
				{
					KeyId = keyId
				};


				var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(securityKey);
				jwk.Use = "sig";
				jwk.Alg = SecurityAlgorithms.RsaSha256;

				var jwks = new JwksWrapper
				{
					Keys = new List<JsonWebKey> { jwk }
				};

				var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
				string jwksJson = JsonSerializer.Serialize(jwks, jsonOptions);

				string privateKeyPem = rsa.ExportRSAPrivateKeyPem();

				Console.WriteLine("--- CHIAVE PRIVATA (PEM - NON CONDIVIDERE MAI) ---");
				Console.WriteLine(privateKeyPem);
				Console.WriteLine("\n--- JWKS.json (DA RENDERE PUBBLICAMENTE ACCESSIBILE) ---");
				Console.WriteLine(jwksJson);
				Console.WriteLine($"\nKey ID (kid) generato: {keyId}");

				if (saveInFile)
				{
					string jwksFilePath = "jwks.json";
					File.WriteAllText(jwksFilePath, jwksJson);
					Console.WriteLine($"\nJWKS salvato in: {jwksFilePath}");

					string privatePemFilePath = "private.pem";
					File.WriteAllText(privatePemFilePath, privateKeyPem);
					Console.WriteLine($"\nprivate.pem salvato in: {privatePemFilePath}");
				}
				return (privateKeyPem, jwksJson, keyId);
			}
		}
	}

	// Helper class per serializzare il JWKS nel formato richiesto.
	public class JwksWrapper
	{
		public List<JsonWebKey> Keys { get; set; }
	}
}