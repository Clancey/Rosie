using System;
using System.Threading.Tasks;
namespace Rosie.AzureIoT
{
	public static class DeviceDatabaseExtension
	{
		public static Task CreateAzureTables (this DeviceDatabase database)
		{
			return database.DatabaseConnection.CreateTableAsync<DeviceAzureKey> ();
		}

		public static Task<DeviceAzureKey> GetAzureKey (this DeviceDatabase database, Device device)
		{
			return database.GetAzureKey (device.Id);
		}
		public static Task<DeviceAzureKey> GetAzureKey (this DeviceDatabase database, string deviceId)
		{
			return database.DatabaseConnection.Table<DeviceAzureKey> ().Where (x => x.Id == deviceId).FirstOrDefaultAsync();
		}
	}
}

