using System;
using System.Threading.Tasks;
namespace Rosie
{
	public interface IDeviceService
	{
		string ServiceIdentifier { get; }

		Task<bool> HandleRequest (Device device, DeviceUpdate request);
	}
}

