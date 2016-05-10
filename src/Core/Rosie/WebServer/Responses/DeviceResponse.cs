using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Converters;
namespace Rosie
{
	//public enum DeviceType
	//{
	//	"
	//}
	public class DeviceResponse
	{
		[JsonProperty ("state")]
		public State State { get; set; } = new State ();

		[JsonProperty ("type")]
		//[JsonConverter (typeof (StringEnumConverter))]
		public string Type { get; set; } = "Extended color light";

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("modelid")]
		public string Modelid { get; set; } = "LCT001";

		[JsonProperty ("manufacturername")]
		public string Manufacturername { get; set; } = "Philips";

		[JsonProperty ("uniqueid")]
		public string Uniqueid { get; set; }

		[JsonProperty ("swversion")]
		public string Swversion { get; set; } = "65003148";

		[JsonProperty ("pointsymbol")]
		public Dictionary<string, string> Pointsymbol { get; set; } = Enumerable.Range (1, 8).ToDictionary (x => x.ToString (), x => "none");
	}

	public class State
	{

		[JsonProperty ("on")]
		public bool On { get; set; }

		[JsonProperty ("bri")]
		public int Bri { get; set; } = 255;

		[JsonProperty ("hue")]
		public int Hue { get; set; } = 15823;

		[JsonProperty ("sat")]
		public int Sat { get; set; } = 88;

		[JsonProperty ("effect")]
		public string Effect { get; set; } = "none";

		[JsonProperty ("ct")]
		public int Ct { get; set; } = 313;

		[JsonProperty ("alert")]
		public string Alert { get; set; } = "none";

		[JsonProperty ("colormode")]
		public string Colormode { get; set; } = "ct";

		[JsonProperty ("reachable")]
		public bool Reachable { get; set; } = true;

		[JsonProperty ("xy")]
		public List<double> Xy { get; set; } = new List<double> {
			0.4255,
			 0.3998
		};
	}


}

