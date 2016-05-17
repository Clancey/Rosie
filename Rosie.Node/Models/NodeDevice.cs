using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rosie.Node
{
	public class NodeDevice
	{
		[JsonProperty ("manufacturer")]
		public string Manufacturer { get; set; }

		[JsonProperty ("manufacturerid")]
		public string Manufacturerid { get; set; }

		[JsonProperty ("product")]
		public string Product { get; set; }

		[JsonProperty ("producttype")]
		public string Producttype { get; set; }

		[JsonProperty ("productid")]
		public string Productid { get; set; }

		[JsonProperty ("type")]
		public string Type { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("loc")]
		public string Loc { get; set; }

		[JsonProperty ("classes")]
		public Dictionary<string, Dictionary<string, NodeValue>> Classes { get; set; }

		[JsonProperty ("ready")]
		public bool Ready { get; set; }
	}
}

