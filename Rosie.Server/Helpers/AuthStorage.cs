using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SimpleAuth;
using System.Reflection;

namespace Rosie
{
	public class AuthStorage : IAuthStorage
	{
		static string EncryptString(string text, string keyString)
		{
			var algorithm = GetAlgorithm(keyString);

			//Anything to process?
			if (string.IsNullOrWhiteSpace(text))
				return "";

			byte[] encryptedBytes;
			using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
			{
				byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(text);
				encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
			}
			return Convert.ToBase64String(encryptedBytes);
		}

		static string DecryptString(string cipherText, string keyString)
		{
			var algorithm = GetAlgorithm(keyString);

			//Anything to process?

			if (string.IsNullOrWhiteSpace(cipherText))
				return "";
			byte[] descryptedBytes;
			using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
			{
				byte[] encryptedBytes = Convert.FromBase64String(cipherText);
				descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
			}
			return Encoding.UTF8.GetString(descryptedBytes);
		}

		static string CalculateMD5Hash(string input)
		{
			var md5 = MD5.Create();

			var inputBytes = Encoding.ASCII.GetBytes(input);
			var hash = md5.ComputeHash(inputBytes);
			var sb = new StringBuilder();

			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}

			return sb.ToString();

		}
		private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
		{
			MemoryStream memory = new MemoryStream();
			using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
			{
				stream.Write(data, 0, data.Length);
			}
			return memory.ToArray();
		}
		private static readonly byte[] salt = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().GetName().FullName);
		private static RijndaelManaged GetAlgorithm(string encryptionPassword)
		{
			// Create an encryption key from the encryptionPassword and salt.
			var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

			// Declare that we are going to use the Rijndael algorithm with the key that we've just got.
			var algorithm = new RijndaelManaged();
			int bytesForKey = algorithm.KeySize / 8;
			int bytesForIV = algorithm.BlockSize / 8;
			algorithm.Key = key.GetBytes(bytesForKey);
			algorithm.IV = key.GetBytes(bytesForIV);
			return algorithm;
		}

		public void SetSecured(string identifier, string value, string clientId, string clientSecret, string sharedGroup)
		{
			var key = $"{clientId}-{identifier}-{clientId}-{sharedGroup}";
			var newKey = CalculateMD5Hash(key);
			Plugin.Settings.CrossSettings.Current.AddOrUpdateValue(newKey, EncryptString(value, clientSecret));
		}

		public string GetSecured(string identifier, string clientId, string clientSecret, string sharedGroup)
		{
			try
			{
				var key = $"{clientId}-{identifier}-{clientId}-{sharedGroup}";
				var newKey = CalculateMD5Hash(key);
				return DecryptString(Plugin.Settings.CrossSettings.Current.GetValueOrDefault(newKey, ""), clientSecret);
			}
			catch (Exception ex)
			{
				//Console.WriteLine(ex);
			}
			return null;
		}
	}
}
