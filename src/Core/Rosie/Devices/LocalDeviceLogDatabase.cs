using System;
using SQLite;

namespace Rosie
{
	public class LocalDeviceLogDatabase
	{

		public SQLiteAsyncConnection DatabaseConnection { get; set; }


		public LocalDeviceLogDatabase () : this ("devices.db")
		{

		}

		public LocalDeviceLogDatabase (string databasePath)
		{
			DatabaseConnection = new SQLiteAsyncConnection (databasePath, true);
			var s = DatabaseConnection.CreateTablesAsync<Device, DeviceGroup, DeviceKeyGrouping, DeviceState> ().Result;
		}

	}
}

