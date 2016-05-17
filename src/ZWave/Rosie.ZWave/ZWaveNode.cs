using System;
using System.Collections.Generic;

namespace Rosie.ZWave
{
	public class ZWaveNode
	{
		public byte Id { get; set; }
		public uint HomeId { get; set; }

		public string Name { get; set; }
		public string Location { get; set; }
		public string Label { get; set; }

		public string Manufacturer { get; set; }

		public string Product { get; set; }

		public List<ZWaveCommandTypes> CommandTypes { get; set; }

	}
}

