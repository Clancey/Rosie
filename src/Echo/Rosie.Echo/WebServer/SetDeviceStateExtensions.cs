using System;
namespace Rosie
{
	public static class SetDeviceStateExtensions
	{
		public static string GetBriStringValue (this SetDeviceStateRequest request, Device device, bool isHex = false)
		{
			int val = request.GetBriValue (device);

			return isHex ? val.ToString ("x8") : val.ToString ();
		}

		public static int GetBriValue (this SetDeviceStateRequest request, Device device)
		{
			int val = request.Bri;
			//switch (device.ConversionType) {
			//case BriConversionType.Byte:
			//	val = request.Bri;
			//	break;
			//case BriConversionType.Percent:
			//	val = (int)Math.Round ((request.Bri / 255d) * 100);
			//	break;
			//case BriConversionType.Math:
			//	throw new NotSupportedException ();
			//}
			throw new NotSupportedException();
			return val;
		}

		public static void SetValueFromBri (this SetDeviceStateRequest request, Device device)
		{
			//switch (device.DeviceType) {
			//case DeviceType.Switch:
			//	//TODO: set dim
			//	return;
			//case DeviceType.Thermostat:
			//	var temp = request.GetBriValue (device);
			//	request.Value = temp;
			//	return;
			//}
		}
	}
}

