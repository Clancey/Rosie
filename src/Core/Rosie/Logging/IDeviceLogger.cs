using System;
using System.Threading.Tasks;

namespace Rosie
{
	public interface IDeviceLogger
	{
		Task<bool> AddDevice (Device device);
		Task<bool> DeviceUpdate (DeviceState state);
		Task<bool> DeviceRemoved (string deviceId);
	}
}

