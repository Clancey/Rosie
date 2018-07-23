//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Net.Http;
//namespace Rosie.Server.Routes.Echo
//{
//	[Path ("api/{userId}/lights/{lightId}/state")]
//	public class PutLightRoute : Route
//	{
//		IDeviceManager _deviceManager;
//		public PutLightRoute (IDeviceManager deviceManager)
//		{
//			IsSecured = false;
//			_deviceManager = deviceManager;
//		}
//		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Put };

//		public override async Task<string> GetResponseString<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
//		{
//			var stateRequest = await data.ToObjectAsync<SetDeviceStateRequest> ().ConfigureAwait (false);
//			var lightId = queryString ["lightId"];
//			var device = await DeviceDatabase.Shared.GetDevice (lightId);
//			//This will convert the Value property from the Bri from the echo
//			stateRequest.SetValueFromBri (device);
//			//TODO: Convert to DeviceUpdate
//			var success = await _deviceManager.SetDeviceState (device, new DeviceUpdate
//			{
//				Key = stateRequest.CommandKey,
//				Value = stateRequest.Value,
//			});
//			var result = await new [] { new { success = new Dictionary<string, bool> { { $"lights/{lightId}/state/on", success ? stateRequest.On : !stateRequest.On } } } }.ToJsonAsync();
//			return result;
//			//
//		}
//	}
//}

