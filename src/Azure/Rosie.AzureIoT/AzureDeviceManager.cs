using System;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Threading.Tasks;
using ADevice = Microsoft.Azure.Devices.Device;
namespace Rosie.AzureIoT
{
	public class AzureDeviceManager
	{
		public static AzureDeviceManager Shared { get; set; } = new AzureDeviceManager ();
		RegistryManager registryManager;

		static string AzureIoTUrl {
			get { return Rosie.Settings.GetSecretString ();}
		}
		public Task Init ()
		{
			return this.Init (AzureIoTUrl);
		}
		public async Task Init (string connectionString)
		{
			registryManager = RegistryManager.CreateFromConnectionString (connectionString);
			await DeviceDatabase.Shared.DatabaseConnection.CreateTableAsync<DeviceAzureKey> ();
			await AddDevice (Settings.DeviceId);
		}


		public async Task<bool> AddDevice (string deviceId)
		{
			try {
				ADevice device;
				try {
					device = await registryManager.AddDeviceAsync (new ADevice (deviceId));
				} catch (DeviceAlreadyExistsException) {
					device = await registryManager.GetDeviceAsync (deviceId);
				}
				var key = new DeviceAzureKey { Id = deviceId, Key = device.Authentication.SymmetricKey.PrimaryKey };
				await DeviceDatabase.Shared.DatabaseConnection.InsertOrReplaceAsync (key);
				return true;
			} catch (Exception ex) {
				//TODO: Log
				Console.WriteLine (ex);
			}
			return false;
		}

		public Task<bool> AddDevice (Device device)
		{
			return AddDevice (device.Id);
		}

		public Task<DeviceAzureKey> GetDeviceKey (Device device)
		{
			return GetDeviceKey (device.Id);
		}

		public async Task<DeviceAzureKey> GetDeviceKey (string deviceId)
		{
			var key = await DeviceDatabase.Shared.GetAzureKey (deviceId);
			if (key != null)
				return key;
			if(!(await AddDevice (deviceId)))
			   return null;
			return await DeviceDatabase.Shared.GetAzureKey (deviceId);
			
		}

	}
}

