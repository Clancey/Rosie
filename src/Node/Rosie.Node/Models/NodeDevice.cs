using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using SQLite;

namespace Rosie.Node
{
	public class NodeDevice
	{
		[JsonProperty ("manufacturer")]
		public string Manufacturer { get; set; }

		[JsonProperty ("manufacturerid")]
		public string ManufacturerId { get; set; }

		[JsonProperty ("product")]
		public string Product { get; set; }

		[JsonProperty ("producttype")]
		public string ProductType { get; set; }

		[JsonProperty ("productid")]
		public string ProductId { get; set; }

		[JsonProperty ("type")]
		public string Type { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("loc")]
		public string Loc { get; set; }

		[PrimaryKeyAttribute]
		public int NodeId { get; set; }

		public string Id { get; set; }

		[JsonProperty ("classes")]
		[SQLite.Ignore]
		public Dictionary<string, Dictionary<string, NodeValue>> Classes { get; set; }

		[JsonProperty ("ready")]
		public bool Ready { get; set; }

		public string PerferedCommand { get; set; }

		public async Task<NodeDeviceCommands> GetPerferedCommand ()
		{
			if (!string.IsNullOrWhiteSpace (PerferedCommand))
				return await NodeDatabase.Shared.GetDeviceCommand (PerferedCommand);
			var commands = await NodeDatabase.Shared.GetDeviceCommands (NodeId);
			var userCommand = commands.FirstOrDefault (x => x.Genre == CommandGenre.User );
			if (userCommand != null)
				return userCommand;
			return commands.FirstOrDefault ();
		}
	}
}

