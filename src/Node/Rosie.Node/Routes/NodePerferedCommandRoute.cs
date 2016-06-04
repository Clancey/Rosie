using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Rosie.Server;
using System.Collections.Generic;
using Rosie.Node;

namespace Rosie.Server.Routes.Node
{
	[Path ("api/NodePerferedCommand/{deviceId}")]
	public class NodePerferedCommandRoute: Route<NodeDeviceCommands>
	{
		public override async Task<NodeDeviceCommands> GetResponse (string method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			var deviceId = queryString ["deviceId"];
			var device = await NodeDatabase.Shared.GetDevice (deviceId);
			var command = await device.GetPerferedCommand ();
			return command;
		}

		public override bool SupportsMethod (string method)
		{
			return method == "GET" || method == "POST";
		}

		public override Task<string> GetResponseString (string method, System.Net.HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			if (method == "GET")
				return base.GetResponseString (method, request, queryString, data);
			else if (method == "POST")
				return GetPostedResponseString (request, queryString, data);
			return null;
			
		}

		public async Task<string> GetPostedResponseString (System.Net.HttpListenerRequest request, NameValueCollection queryString, string data)
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

