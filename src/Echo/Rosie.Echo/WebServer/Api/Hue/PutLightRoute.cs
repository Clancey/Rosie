using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}/lights/{lightId}/state")]
	public class PutLightRoute : Route
	{
		public PutLightRoute ()
		{
			IsSecured = false;
		}

		public override bool SupportsMethod (string method) => method == "PUT";

		public override async Task<string> GetResponseString (string method, System.Net.HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var stateRequest = await data.ToObjectAsync<SetDeviceStateRequest> ().ConfigureAwait (false);
			var lightId = queryString ["lightId"];
			var device = await DeviceDatabase.Shared.GetDevice (lightId);
			//This will convert the Value property from the Bri from the echo
			stateRequest.SetValueFromBri (device);
			Console.WriteLine (data);
			Console.WriteLine (device);
			var success = await DeviceManager.Shared.SetDeviceState (device, stateRequest);
			var result = await new [] { new { success = new Dictionary<string, bool> { { $"lights/{lightId}/state/on", success ? stateRequest.On : !stateRequest.On } } } }.ToJsonAsync();
			return result;
			//
		}
	}
}

