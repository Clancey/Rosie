using System;
using SQLite;
namespace Rosie
{
	public class DeviceType
	{
		[PrimaryKey]
		public string Key { get; set; }

		public string[] Capabilities { get; set; }

		public string PrimaryCapability { get; set; }
	}
}
