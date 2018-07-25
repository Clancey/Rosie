using System;
using SQLite;
using Newtonsoft.Json;
using Rosie.Models.JsonConverters;
namespace Rosie
{
	public class DeviceCommand
	{
		public string Command { get; set; }

		[Ignore]
		public DeviceCommandArguments[] Arguments { get; set; }

		/// <summary>
		/// Used by Sqlite-net. Do not use
		/// </summary>
		/// <value>The device command arguments string.</value>
		[JsonIgnore]
		public string _deviceCommandArgumentsString {
			get => Arguments?.Length > 0 ? JsonConvert.SerializeObject(Arguments) : null;
			set => Arguments = string.IsNullOrWhiteSpace(value) ? null : JsonConvert.DeserializeObject<DeviceCommandArguments[]>(value);
		}

	}
}
