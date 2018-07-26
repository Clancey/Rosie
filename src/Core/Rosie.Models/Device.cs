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
		/// <summary>
		/// This is a guid that is automatically generated.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public string Id { get; set; }

		/// <summary>
		/// This is the ID that is used by each service to identify devices for their platform.
		/// </summary>
		/// <value>The service device identifier.</value>
		public string ServiceDeviceId { get; set; }

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

		[Ignore]
		public string[] AdditionalCapabilities { get; set; }
		/// <summary>
		/// Internal used by Sqlite-net
		/// </summary>
		/// <value>The commands string.</value>
		[JsonIgnore]
		public string _additionalCapabilitiesString
		{
			get => AdditionalCapabilities == null ? "" : string.Join(",", AdditionalCapabilities);
			set => AdditionalCapabilities = value?.Split(',');
		}


		[Ignore]
		public string[] AdditionalCommands { get; set; }
		/// <summary>
		/// Internal used by Sqlite-net
		/// </summary>
		/// <value>The commands string.</value>
		[JsonIgnore]
		public string _AdditionalCommandsString
		{
			get => AdditionalCommands == null ? "" : string.Join(",", AdditionalCommands);
			set => AdditionalCommands = value?.Split(',');
		}

		[Indexed]
		public string Service { get; set; }

		public bool Discoverable { get; set; } = true;
	}
}

