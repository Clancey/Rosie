using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Rosie.SmartThings
{
	public class SmartThingsManager : IDeviceHandler
	{
		public static SmartThingsManager Shared { get; set; } = new SmartThingsManager ();

		public string ServiceIdentifier => "SmartThings";

		public Task<bool> HandleRequest (Rosie.Device device, SetDeviceStateRequest request)
		{
			throw new NotImplementedException ();
		}
		SmartThingsApi api = new SmartThingsApi ("SmartThings");
		SmartThingsUpdateListener updater;
		public async Task<bool> Init ()
		{
			try {
				var account = await api.Authenticate ();
				if (account == null)
					return false;
				updater = new SmartThingsUpdateListener (api);
				updater.UpdateReceived += (obj) => {
					Console.WriteLine ("Status update");
				};
				await updater.StartListening ();

				#region testing
				var devices = await api.GetDevices ();
				devices.ToList ().ForEach (x => Console.WriteLine ($"Name: {x.Name} Label: {x.Label}"));
				var officeLights = devices.FirstOrDefault((x) => (x.Label?.Contains("Office") ?? false)  && (x.Label?.Contains("Light") ?? false) );
				await api.TurnOnDevice (officeLights);
				await Task.Delay (10000);
				await api.TurnOffDevice (officeLights);
				#endregion
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}

		public static string GetSig (string input, string key, bool trim = true)
		{
			var encoding = new ASCIIEncoding ();
			//var newKey = StringToAscii(key);
			byte [] newKey = StringToAscii (key); // 
			byte [] newKey2 = encoding.GetBytes (key);
			var hmacsha1 = new HMACSHA1 (newKey);

			//byte[] byteArray = StringToAscii(input);
			byte [] byteArray = StringToAscii (input); // encoding.GetBytes (input);
			byte [] foo = hmacsha1.ComputeHash (byteArray);

			string ret = Convert.ToBase64String (foo);
			ret = ret.Replace ('+', '-')
				.Replace ('/', '_');
			if (!trim)
				return ret;
			return ret.Substring (0, ret.Length - 1);
			//.Replace('=', '.');
		}

		public static byte [] StringToAscii (string s)
		{
			var retval = new byte [s.Length];
			for (int ix = 0; ix < s.Length; ++ix) {
				char ch = s [ix];
				if (ch <= 0x7f)
					retval [ix] = (byte)ch;
				else
					retval [ix] = (byte)'?';
			}
			return retval;
		}
	}
}

