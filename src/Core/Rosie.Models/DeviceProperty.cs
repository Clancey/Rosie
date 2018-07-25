using System;
using Newtonsoft.Json;
using SQLite;

namespace Rosie
{
	public class DeviceProperty
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public DataTypes DataType { get; set; }
		public bool IsReadonly { get; set; }

		[Ignore]
		public DeviceCommandArguments Arguments { get; set; }

		/// <summary>
		/// Used by Sqlite-net. Do not use
		/// </summary>
		/// <value>The device command arguments string.</value>
		[JsonIgnore]
		public string _deviceCommandArgumentsString
		{
			get => Arguments != null ? JsonConvert.SerializeObject(Arguments) : null;
			set => Arguments = string.IsNullOrWhiteSpace(value) ? null : JsonConvert.DeserializeObject<DeviceCommandArguments>(value);
		}
	}
}
