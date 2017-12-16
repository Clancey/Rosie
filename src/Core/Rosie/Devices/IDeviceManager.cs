using System.Threading.Tasks;

namespace Rosie
{
	public interface IDeviceManager
	{
		Task<bool> AddDevice(Device device);
		void RegisterDeviceLogHandler<T>() where T : IDeviceLogger;
		void RegisterHandler<T>() where T : IDeviceHandler;
		void RegisterHandler(IDeviceHandler handler);
		void RegisterHandler(IDeviceLogger handler);
		Task<bool> SetDeviceState(Device device, SetDeviceStateRequest state);
		Task UpdateCurrentState(DeviceState state);
	}
}