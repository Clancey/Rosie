using System;
using System.Security;
using System.Threading.Tasks;
using SimpleAuth;

namespace Rosie
{
	public class BasicAuthController
	{
		readonly IBasicAuthenicator authenticator;

		public BasicAuthController (IBasicAuthenicator authenticator)
		{
			this.authenticator = authenticator;
		}


		public async Task<Tuple<string, string>> GetCredentials (string title, string details = "")
		{
			try {
				Console.WriteLine ("******************");
				Console.WriteLine (title);
				Console.WriteLine (details);
				Console.WriteLine ("******************");
				Console.WriteLine ("Enter Username:");
				var username = Console.ReadLine ();
				Console.WriteLine ("Enter Password:");
				var password = GetPassword ();

				var result = new Tuple<string, string> (username, password);
				try {
					bool success = false;
					var basic = authenticator;
					if (basic != null) {
						success = await basic.VerifyCredentials (result.Item1, result.Item2);
					}
					if (!success)
						throw new Exception ("Invalid Credentials");
				} catch (Exception ex) {
					result = await GetCredentials (title, $"Error: {ex.Message}");
				}
				return result;
			} catch (TaskCanceledException) {
				authenticator.OnCancelled ();
				return null;
			}
		}
		public string GetPassword ()
		{
			var pwd = "";
			while (true) {
				ConsoleKeyInfo i = Console.ReadKey (true);
				if (i.Key == ConsoleKey.Enter) {
					break;
				} else if (i.Key == ConsoleKey.Backspace) {
					if (pwd.Length > 0) {
						pwd.Remove (pwd.Length - 1);
						Console.Write ("\b \b");
					}
				} else {
					pwd += (i.KeyChar);
					Console.Write ("*");
				}
			}
			return pwd;
		}
	}
}

