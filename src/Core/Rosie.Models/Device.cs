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

	public enum DeviceType
	{
		Unknown,
		Switch,
		Thermostat,
		Light,
		Sensor,
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

		public DeviceType DeviceType { get; set; } = DeviceType.Switch;

		public string OffUrl { get; set; }

		public string OnUrl { get; set; }

		public BriConversionType ConversionType { get; set; }

		[Indexed]
		public string Service { get; set; }

		public bool Discoverable { get; set; } = true;
	}
}

