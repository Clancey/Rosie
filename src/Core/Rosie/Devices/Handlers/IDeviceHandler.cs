using System;
using System.Threading.Tasks;
namespace Rosie
{
	public interface IDeviceHandler
	{
		string ServiceIdentifier { get; }

		Task<bool> HandleRequest (Device device, DeviceUpdate request);
	}
}

