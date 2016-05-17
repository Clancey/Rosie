using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.Generic;
using System.IO;

namespace Rosie
{
	public static class Settings
	{
		public static string DeviceId {
			get { 
				var value = GetString ();
				if (string.IsNullOrWhiteSpace (value))
					value = DeviceId = GetMacAddress ();
				return value;
			}
			set { SetString (value);}
		}
		public static string ZWavePort {
			get { return GetString ("/dev/tty.usbmodem1411");}
			set { GetString (value);}
		}
		#region Helpers
		public static ISettings AppSettings { get; } = CrossSettings.Current;

		public static string GetSecretString (string defaultValue = "", [CallerMemberName] string memberName = "")
		{
			try{
				var json = File.ReadAllText ("Secrets.json");
				var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
				return data [memberName];
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return defaultValue;
		}

		public static string GetString (string defaultValue = "", [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetString (string value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<string> (memberName, value);
		}
		public static int GetInt (int defaultValue = 0, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetInt (int value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<int> (memberName, value);
		}

		public static long GetLong (long defaultValue = 0, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetLong (long value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<long> (memberName, value);
		}

		public static bool GetBool (bool defaultValue = false, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		public static void SetBool (bool value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<bool> (memberName, value);
		}


		static T Get<T> (T defaultValue, [CallerMemberName] string memberName = "")
		{
			return AppSettings.GetValueOrDefault (memberName, defaultValue);
		}

		static void Set<T> (T value, [CallerMemberName] string memberName = "")
		{
			AppSettings.AddOrUpdateValue<T> (memberName, value);
		}

		public static string GetMacAddress ()
		{
			var interfaces = NetworkInterface.GetAllNetworkInterfaces ();
			return NetworkInterface.GetAllNetworkInterfaces ()
				                   .Where (x => x.OperationalStatus == OperationalStatus.Up)
				                   .Select (x => x.GetPhysicalAddress ().ToString ())
				                   .Where(x=> !string.IsNullOrWhiteSpace(x))
				                   .FirstOrDefault();
		}
		#endregion
	}
}

