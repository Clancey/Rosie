using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Rosie.Server;
using Microsoft.Extensions.DependencyInjection;
namespace Rosie.Server.Routes
{

	[Path("api/{deviceId}/State")]
	public class DeviceStateRoute : Route<DeviceState[]>
	{
		public DeviceStateRoute()
		{
		}

		public override Task<DeviceState[]> GetResponse<HttpRequest>(HttpMethod method, HttpRequest request, NameValueCollection queryString, string data)
		{
			var deviceId = queryString["deviceId"];
			if (method == HttpMethod.Get)
				return GetDeviceState(deviceId);
			else if (method == HttpMethod.Post)
				return SetDeviceState(deviceId, data.ToObject<DeviceUpdate>());
			throw new NotSupportedException($"Not supported HttpMethod: {method.Method}");
		}

		async Task<DeviceState[]> SetDeviceState(string deviceId, DeviceUpdate state)
		{
			var manager = ServiceProvider.GetService<IDeviceManager>();
			var device = await DeviceDatabase.Shared.GetDevice(deviceId);
			if (!await manager.SetDeviceState(device, state))
				throw new Exception("Error processing the request");
			return new[] { await DeviceDatabase.Shared.GetDeviceState(deviceId, state.PropertyKey) };
		}

		Task<DeviceState[]> GetDeviceState(string deviceId)
		{
			return DeviceDatabase.Shared.GetDeviceState(deviceId);
		}


		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get, HttpMethod.Post };
	}
}
