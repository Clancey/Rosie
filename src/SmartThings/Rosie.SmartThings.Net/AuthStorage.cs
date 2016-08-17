using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SimpleAuth;

namespace Rosie
{
	class AuthStorage : IAuthStorage
	{
		public void SetSecured(string identifier, string value, string clientId, string clientSecret,string sharedGroup)
		{
			var key = $"{clientId}-{identifier}-{clientId}-{sharedGroup}";
			var newKey = CalculateMD5Hash (key);
			var secured = Protect (value, clientSecret).ToJson();
			Settings.SetString (secured,newKey);

		}

		public string GetSecured(string identifier, string clientId, string clientSecret,string sharedGroup)
		{
			var key = $"{clientId}-{identifier}-{clientId}-{sharedGroup}";
			var newKey = CalculateMD5Hash (key);
			var val = Settings.GetString("",newKey).ToObject<byte[]>();

			var outValue = Encoding.UTF8.GetString (Unprotect(val,clientSecret));
			return outValue;
		}
		static byte [] aditionalEntropy = { 3, 8, 0, 5, 2 };
		public static string CalculateMD5Hash (string input)
		{
			
			var md5 = MD5.Create ();

			var inputBytes = Encoding.ASCII.GetBytes (input);
			var hash = md5.ComputeHash (inputBytes);
			var sb = new StringBuilder ();

			for (int i = 0; i < hash.Length; i++) {
				sb.Append (hash [i].ToString ("X2"));
			}

			return sb.ToString ();

		}
		static byte[] Protect (string data,string key)
		{
			try {
				// Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
				//  only by the same current user.
				return ProtectedData.Protect (Encoding.UTF8.GetBytes(data), string.IsNullOrWhiteSpace(key) ? aditionalEntropy : Encoding.UTF8.GetBytes(key), DataProtectionScope.CurrentUser);
			} catch (Exception e) {
				Console.WriteLine ("Data was not encrypted. An error occurred.");
				Console.WriteLine (e.ToString ());
				return null;
			}
		}

		static byte[] Unprotect (byte[] data,string key)
		{
			try {
				//Decrypt the data using DataProtectionScope.CurrentUser.
				return ProtectedData.Unprotect (data,string.IsNullOrWhiteSpace (key) ? aditionalEntropy : Encoding.UTF8.GetBytes (key), DataProtectionScope.CurrentUser);
			} catch (Exception e) {
				Console.WriteLine ("Data was not decrypted. An error occurred.");
				Console.WriteLine (e.ToString ());
				return null;
			}
		}
	}
}
