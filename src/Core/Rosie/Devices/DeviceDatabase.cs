using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Threading.Tasks;

namespace Rosie
{
	public class DeviceDatabase
	{
		public SQLiteAsyncConnection DatabaseConnection { get; set;}

		public DeviceDatabase () : this ("devices.db")
		{

		}

		public DeviceDatabase (string databasePath)
		{
			//System.IO.File.Delete (databasePath);
			DatabaseConnection = new SQLiteAsyncConnection (databasePath, true);
			var s = DatabaseConnection.CreateTablesAsync<Device,DeviceGroup,DeviceKeyGrouping, DeviceState> ().Result;
		}

		public static DeviceDatabase Shared { get; set; } = new DeviceDatabase ();

		public List<Device> TestDevices { get; set; } = new List<Device> {
			new Device{
				Name = "Bedroom Thermostat",
				Id = "720e95b3-9498-4e86-af8d-1f177c9e25b8",
				DeviceType = DeviceType.Thermostat
			},
			new Device{
				Name = "Kitchen Lights",
				Id = "476b66f1-29fe-42fe-bb9c-d17421bf5f1e",
			},
		};

		public Task<DeviceState> GetDeviceState(string deviceId)
		{
			return DatabaseConnection.Table<DeviceState>().Where(x => x.DeviceId == deviceId).FirstOrDefaultAsync();
		}

		//public async void TestUpdates ()
		//{
		//	try {
		//		var device = TestDevices [0];
		//		var update = new DeviceState { DeviceId = device.Id, Key = "Temperature", Value = 70, DataType = DataTypes.Decimal, DataFormat = "F" };
		//		var json = update.ToSimpleObject ().ToJson ();
		//		await DatabaseConnection.InsertOrReplaceAsync (update);
		//		var resp = await DatabaseConnection.Table<DeviceState> ().Where (x => x.DeviceId == device.Id).ToListAsync();

		//		Console.WriteLine (resp [0]);
		//	} catch (Exception ex) {
		//		Console.WriteLine (ex);
		//	}

		//}

		public Task InsertDeviceState (DeviceState state)
		{
			return DatabaseConnection.InsertOrReplaceAsync (state);
		}

		public Task<Device> GetDevice (string id)
		{
			return DatabaseConnection.Table<Device> ().Where (x => x.Id == id).FirstOrDefaultAsync ();
		}

		public Task<List<Device>> GetAllDevices ()
		{
			return DatabaseConnection.Table<Device> ().ToListAsync ();
		}

		public Task<List<Device>> GetEchoDevices ()
		{

			return DatabaseConnection.Table<Device> ().Where(x=> x.Discoverable && x.DeviceType != DeviceType.Unknown).ToListAsync ();
		}

		public async Task<bool> InsertDevice (Device device)
		{
			var s = await DatabaseConnection.InsertOrReplaceAsync (device);
			return s > 0;
		}

		public async Task<bool> DeleteDevice (Device device)
		{
			var s = await DatabaseConnection.DeleteAsync (device);
			return s > 0;
		}

		public readonly DeviceCapability[] DefaultCapabilities = {
			new DeviceCapability{Key = DeviceCapabilityKeys.Switch},
			new DeviceCapability{Key = DeviceCapabilityKeys.Sensor},
		};
	}
}

