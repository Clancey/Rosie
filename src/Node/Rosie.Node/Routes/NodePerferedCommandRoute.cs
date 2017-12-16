using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Rosie.Server;
using System.Collections.Generic;
using Rosie.Node;
using System.Net.Http;

namespace Rosie.Server.Routes.Node
{
	[Path ("api/NodePerferedCommand/{deviceId}")]
	public class NodePerferedCommandRoute: Route<NodeDeviceCommands>
	{
		public override async Task<NodeDeviceCommands> GetResponse<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			var deviceId = queryString ["deviceId"];
			var device = await NodeDatabase.Shared.GetDevice (deviceId);
			var command = await device.GetPerferedCommand ();
			return command;
		}


		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get, HttpMethod.Post };

		public override Task<string> GetResponseString<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			if (method == HttpMethod.Get)
				return base.GetResponseString (method, request, queryString, data);
			else if (method == HttpMethod.Post)
				return GetPostedResponseString (request, queryString, data);
			return null;
			
		}

		public async Task<string> GetPostedResponseString<HttpListenerRequest> (HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			var dict = await data.ToObjectAsync<Dictionary<string,string>> ();

			var deviceId = queryString ["deviceId"];
			var command = dict ["command"];
			var nodeDevice = await NodeDatabase.Shared.GetDevice (deviceId);

			nodeDevice.PerferedCommand = command;
			await NodeDatabase.Shared.InsertDevice (nodeDevice);

			return new { Success = true }.ToJson();
		}

	}
}

