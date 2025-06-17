namespace GenerateRSAKey
{
	internal class Program
	{

		private const string UsageMessage = "Usage: GenerateRSAKey [\"2048\" or \"4096\" default value 2048> [optional: \"-s\" save pem and jwks.json file]";
		private const string GenerateRsaKeyDescription = "Generates an RSA key pair and outputs the private key in PEM format and the JWKS in JSON format.";
		static void Main(string[] args)
		{
			Console.WriteLine($"{GenerateRsaKeyDescription}\n\n");
			var (keySize, saveInFile) = ParseArguments(args);
			Console.WriteLine("Generate RSA KEY");
			KeyGenerator.GenerateRsaKeyAndJwks(keySize, saveInFile);
			Console.WriteLine("Key generated");
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		private static (KeySize keySize, bool saveInFile) ParseArguments(string[] args)
		{
			KeySize keySize = KeySize._2048; // Default key size
			bool saveInFile = false;

			if (args.Length == 1 && (args[0] == "h" || args[0] == "-h"))
			{
				Console.WriteLine(UsageMessage);
				Environment.Exit(0);
			}

			if (args.Length > 2)
			{
				throw new ArgumentException("Too many arguments. Usage: GenerateRSAKey [\"2048\" or \"4096\" default value 2048> [optional: \"-s\" save pem and jkws.json file]");
			}

			if (args.Length == 0)
			{
				return (keySize, saveInFile);
			}
			foreach (var arg in args)
			{
				if (arg == "4096")
				{
					keySize = KeySize._4096;
				}
				else if (arg == "2048")
				{
					keySize = KeySize._2048;
				}
				else if (arg == "-s")
				{
					saveInFile = true;
				}
				else
				{
					throw new ArgumentException($"Invalid argument: {arg}. Use '2048', '4096' or '-s'.");
				}
			}
			return (keySize, saveInFile);
		}
	}
}
