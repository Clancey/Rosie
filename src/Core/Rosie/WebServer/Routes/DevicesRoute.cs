using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

namespace Rosie.Server.Routes
{
	[Path("api/Devices")]
	public class DevicesRoute : Route<List<Device>>
	{
		IDeviceManager _deviceManager;
		public DevicesRoute(IDeviceManager deviceManager)
		{
			_deviceManager = deviceManager;
		}

		public override System.Threading.Tasks.Task<List<Device>> GetResponse<HttpRequest>(HttpMethod method, HttpRequest request, NameValueCollection queryString, string data)
		{
			return _deviceManager.GetAllDevices();
		}

		public override HttpMethod[] GetSupportedMethods() =>  new HttpMethod[] { HttpMethod.Get };
	}
}

