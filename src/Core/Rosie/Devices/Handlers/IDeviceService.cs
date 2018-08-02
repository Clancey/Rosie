using System;
using System.Threading.Tasks;
namespace Rosie
{
	public interface IDeviceService
	{
		event EventHandler<DeviceState> CurrentStateUpdated;
		event EventHandler<Device> DeviceAdded;

		string ServiceIdentifier { get; }

		Task<bool> HandleRequest (Device device, DeviceUpdate request);
	}
}

