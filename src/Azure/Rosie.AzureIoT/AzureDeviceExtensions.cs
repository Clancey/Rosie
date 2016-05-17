//using System;
//using System.Threading.Tasks;
//using ADevice = Microsoft.Azure.Devices.Device;
//using Microsoft.Azure.Devices;

//namespace Rosie.AzureIoT
//{
//	public static class AzureDeviceExtensions
//	{
//		public static string GetDeviceDescription (this ADevice device)
//		{
//			return device.GetDeviceProperty<string> (DevicePropertyNames.DeviceDescription);
//		}

//		public static Task SetDeviceDescription (this ADevice device, string description)
//		{
//			return AzureDeviceManager.Shared.SetDeviceProperty (device.Id, DevicePropertyNames.DeviceDescription, description);
//		}

//		public static string GetManufacturer (this ADevice device)
//		{
//			return device.GetDeviceProperty<string> (DevicePropertyNames.Manufacturer);
//		}

//		public static Task SetManufacturer (this ADevice device, string manufacturer)
//		{
//			return AzureDeviceManager.Shared.SetDeviceProperty (device.Id, DevicePropertyNames.Manufacturer, manufacturer);
//		}

//		public static string GetModelNumber (this ADevice device)
//		{
//			return device.GetDeviceProperty<string> (DevicePropertyNames.ModelNumber);
//		}

//		public static Task SetModelNumber (this ADevice device, string modelNumber)
//		{
//			return AzureDeviceManager.Shared.SetDeviceProperty (device.Id, DevicePropertyNames.ModelNumber, modelNumber);
//		}

//		public static string GetHardwareVersion (this ADevice device)
//		{
//			return device.GetDeviceProperty<string> (DevicePropertyNames.HardwareVersion);
//		}

//		public static Task SetHardwareVersion (this ADevice device, string hardwareVersion)
//		{
//			return AzureDeviceManager.Shared.SetDeviceProperty (device.Id, DevicePropertyNames.HardwareVersion, hardwareVersion);
//		}

//		public static float GetBatteryLevel (this ADevice device)
//		{
//			return device.GetDeviceProperty<float> (DevicePropertyNames.BatteryLevel);
//		}

//		public static Task SetBatteryLevel (this ADevice device, float batteryLevel)
//		{
//			return AzureDeviceManager.Shared.SetDeviceProperty (device.Id, DevicePropertyNames.BatteryLevel, batteryLevel);
//		}

//		public static string GetBatteryStatus (this ADevice device)
//		{
//			return device.GetDeviceProperty<string> (DevicePropertyNames.BatteryStatus);
//		}

//		public static T GetDeviceProperty<T> (this ADevice device, string property)
//		{
//			DevicePropertyValue propValue;
//			device.DeviceProperties.TryGetValue (property, out propValue);
//			return (T)(propValue?.Value ?? default (T));
//		}
//	}
//}

