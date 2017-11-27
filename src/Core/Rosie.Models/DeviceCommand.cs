using System;
using SQLite;
namespace Rosie
{
	public class DeviceCommand
	{
		[PrimaryKey]
		public string Command { get; set; }

		public DataTypes DataType {get;set;}

		public bool IsReadonly {get;set;}

	}
	public class CapabilityCommandGrouping
	{
		[PrimaryKey]
		public string Id
		{
			get
			{
				return $"{Capability} - {Command}";
			}
			set
			{
				//This is here just for SqlIte;
			}
		}

		[Indexed]
		public string Capability { get; set; }

		public string Command { get; set; }
	}
}
