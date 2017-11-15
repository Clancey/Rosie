using System;
using SQLite;

namespace Rosie
{
	public partial class DeviceState : DeviceUpdate
	{
		public override string Id {
			get {
				return base.Id;
			}
			set {
				base.Id = value;
			}
		}

		[PrimaryKey]
		public string DeviceStateId {
			get {
				return  $"{DeviceId} - {Key}";
			}
			set {
				
			}
		}

		public string GroupType { get; set; }
	}
}

