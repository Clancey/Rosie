using System;
using Newtonsoft.Json;
using SQLite;
namespace Rosie
{
	public class DeviceCapability
	{
		[PrimaryKey]
		public string Key { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is string s)
				return s == Key;
			if (obj is DeviceCapability c)
				return Key == c.Key;
			return false;
		}

		public override int GetHashCode()
		{
			return Key.GetHashCode();
		}

		[Ignore]
		public string[] Commands { get; set; }

		[Ignore]
		public string[] Properties { get; set; }

		/// <summary>
		/// Internal used by Sqlite-net
		/// </summary>
		/// <value>The commands string.</value>
		public string _commandsString
		{
			get => string.Join(",", Commands);
			set => Commands = value?.Split(',');
		}


		/// <summary>
		/// Internal used by Sqlite-net
		/// </summary>
		/// <value>The commands string.</value>
		public string _propertiesString
		{
			get => string.Join(",", Properties);
			set => Properties = value?.Split(',');
		}
	}
}
