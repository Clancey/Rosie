using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Rosie.Node;
using Rosie.Server;

namespace Rosie.Server.Routes.Node
{
	[Path ("api/NodeDevice/{deviceId}")]
	public class NodeDeviceRoute : Route<NodeDevice>
	{
		public override Task<NodeDevice> GetResponse (string method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			var deviceId = queryString ["deviceId"];
			return NodeDatabase.Shared.GetDevice (deviceId);
		}

		public override bool SupportsMethod (string method)
		{
			return method == "GET";
		}
	}
}

