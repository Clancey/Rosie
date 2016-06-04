using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Rosie.Node;
using Rosie.Server;

namespace Rosie.Server.Routes.Node
{
	[Path ("api/NodeDevices")]
	public class NodeDevicesRoute : Route<List<NodeDevice>>
	{
		public NodeDevicesRoute ()
		{
		}

		public override Task<List<NodeDevice>> GetResponse (string method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			return NodeDatabase.Shared.DatabaseConnection.Table<NodeDevice>().ToListAsync();
		}

		public override bool SupportsMethod (string method)
		{
			return method == "GET";
		}
	}
}

