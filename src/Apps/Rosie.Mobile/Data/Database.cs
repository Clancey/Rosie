using System;
using SQLite;
using System.IO;

namespace Rosie.Mobile
{
	public class Database : SQLiteAsyncConnection
	{
		public static Database Shared { get; set; } = new Database ();
		public Database () : base (Path.Combine(Locations.DocumentsDir, "devices.db"), true)
		{
			var s = CreateTablesAsync<Device, DeviceGroup, DeviceKeyGrouping, DeviceState> ().Result;
		}
	}
}


