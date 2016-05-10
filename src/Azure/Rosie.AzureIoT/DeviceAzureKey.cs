using System;
using SQLite;
namespace Rosie.AzureIoT
{
	public class DeviceAzureKey
	{
		[PrimaryKey]
		public string Id { get; set; }

		public string Key { get; set; }
	}
}

