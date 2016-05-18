using System;
using System.Threading.Tasks;
using SQLite;

namespace Rosie
{
	public class SqliteDeviceLogger : IDeviceLogger
	{
		public SQLiteAsyncConnection DatabaseConnection { get; set; }

		public SqliteDeviceLogger () : this ("log.db")
		{

		}

		public SqliteDeviceLogger (string databasePath)
		{
			DatabaseConnection = new SQLiteAsyncConnection (databasePath, true);
			var s = DatabaseConnection.CreateTablesAsync<Device, DeviceUpdate, DeviceState> ().Result;
		}

		public async Task<bool> AddDevice (Device device)
		{
			await DatabaseConnection.InsertOrReplaceAsync (device);
			return true;
		}

		public async Task<bool> DeviceRemoved (string deviceId)
		{
			var device = await DatabaseConnection.Table<Device> ().Where (x => x.Id == deviceId).FirstOrDefaultAsync();
			if (device == null)
				return true;
			await DatabaseConnection.DeleteAsync (device);
			return true;
		}

		public async Task<bool> DeviceUpdate (DeviceState state)
		{
			//This saves the current state
			await DatabaseConnection.InsertOrReplaceAsync (state);
			//this saves it as an update which is incremented
			await DatabaseConnection.InsertAsync (state, typeof (DeviceUpdate));
			return true;
		}
	}
}

