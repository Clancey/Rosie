using System;
using SQLite;
namespace Rosie
{
	public class DeviceGroup
	{
		[PrimaryKey]
		public string Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}

	public class DeviceKeyGrouping
	{
		[PrimaryKey]
		public string Id { 
			get {
				return $"{GroupId} - {DeviceId}";
			}
			set {
				//This is here just for SqlIte;
			}
		}

		[Indexed]
		public string GroupId { get; set; }

		public string DeviceId { get; set; }
	}
}

