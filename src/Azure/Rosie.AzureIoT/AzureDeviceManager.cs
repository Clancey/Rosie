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
			//deviceJobClient = JobClient.CreateFromConnectionString (connectionString);
			await DeviceDatabase.Shared.DatabaseConnection.CreateTableAsync<DeviceAzureKey> ();
			var device = new Device { Id = Settings.DeviceId,Description = System.Environment.MachineName };
			await AddDevice (device);
		}


		public async Task<bool> AddDevice (Device device)
		{
			try {
				ADevice adevice;
				try {
					adevice = await registryManager.AddDeviceAsync (new ADevice (device.Id));
				} catch (DeviceAlreadyExistsException) {
					adevice = await registryManager.GetDeviceAsync (device.Id);
				}
				adevice.Status = DeviceStatus.Enabled;
				//await PopulateProperties (adevice,device);
				var key = new DeviceAzureKey { Id = device.Id, Key = adevice.Authentication.SymmetricKey.PrimaryKey };
				await DeviceDatabase.Shared.DatabaseConnection.InsertOrReplaceAsync (key);
				return true;
			} catch (Exception ex) {
				//TODO: Log
				Console.WriteLine (ex);
			}
			return false;
		}

		//JobClient deviceJobClient;
		//public async Task PopulateProperties (ADevice adevice, Device device)
		//{
		//	if (adevice.GetDeviceDescription () != device.Description) {
		//		await adevice.SetDeviceDescription (device.Description);
		//	}
		//}

		//public Task SetDeviceProperty (string deviceId, string deviceProperty, object value)
		//{
		//	return deviceJobClient.ScheduleDevicePropertyWriteAsync (Guid.NewGuid ().ToString (), deviceId, deviceProperty, value);
		//}

		public async  Task<DeviceAzureKey> GetDeviceKey (Device device)
		{
			var key = await DeviceDatabase.Shared.GetAzureKey (device.Id);
			if (key != null)
				return key;
			if (!(await AddDevice (device)))
				return null;
			return await DeviceDatabase.Shared.GetAzureKey (device.Id);
		}


	}
}

