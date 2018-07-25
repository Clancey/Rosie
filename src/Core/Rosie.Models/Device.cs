using System;
using Newtonsoft.Json;
using SQLite;
namespace Rosie
{
	public enum BriConversionType
	{
		Percent,
		Byte,
		Math,
	}

	public class Device
	{
		[PrimaryKey]
		public string Id { get; set; } 

		public string Name { get; set; }

		public string Description { get; set; }

		[Indexed]
		public string Location { get; set; }

		public string Manufacturer { get; set; }

		public string ManufacturerId { get; set; }

		public string Product { get; set; }

		public string ProductType { get; set; }

		public string ProductId { get; set; }

		public string Type { get; set; }

		public string DeviceType { get; set; }

		public string[] AdditionalCapabilities { get; set; }

		public string[] AdditionalCommands { get; set; }

		[Indexed]
		public string Service { get; set; }

		public bool Discoverable { get; set; } = true;
	}
}

