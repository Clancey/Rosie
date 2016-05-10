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
		public void Init ()
		{
			this.Init (AzureIoTUrl);
		}
		public void Init (string connectionString)
		{
			registryManager = RegistryManager.CreateFromConnectionString (connectionString);
			AddDevice (Settings.DeviceId).Wait();
		}


		public async Task AddDevice (string deviceId)
		{
			ADevice device;
			try {
				device = await registryManager.AddDeviceAsync (new ADevice (deviceId));
			} catch (DeviceAlreadyExistsException) {
				device = await registryManager.GetDeviceAsync (deviceId);
			}
			Console.WriteLine ("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);

		}

		public Task AddDevice (Device device)
		{
			return AddDevice (device.Id);
		}

	}
}

