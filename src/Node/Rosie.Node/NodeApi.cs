using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAuth;
namespace Rosie.Node
{
	class NodeApi : ApiKeyApi
	{
		static string NodeServerApiKey {
			get { return Rosie.Settings.GetSecretString (); }
		}
		public NodeApi () : this (NodeServerApiKey)
		{

		}
		public NodeApi (string apiKey) : base(apiKey,"apikey", AuthLocation.Query)
		{
			BaseAddress = new Uri($"http://localhost:8080");
		}
		[Path("api/devices")]
		public Task<List<NodeDevice>> GetDevices ()
		{
			return Get<List<NodeDevice>> ();
		}

		[Path ("api/device")]
		public async Task<bool> SetState (NodeDeviceCommands command, object value)
		{
			try {
				var data = new {
					nodeId = command.NodeId,
					commandClass = command.ClassId,
					instance = command.Instance,
					index = command.Index,
					value = value
				};
				Console.WriteLine ($"Sending: {data.ToJson ()}");
				var s = await Post (data);
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}

	}
}

