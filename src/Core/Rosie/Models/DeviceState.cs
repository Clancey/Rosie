using System;
using SQLite;

namespace Rosie
{
	public class DeviceState : DeviceUpdate
	{
		[PrimaryKey]
		public override string Id {
			get {
				return $"{DeviceId} - {Key}";
			}
			set {
				base.Id = value;
			}
		}
	}
}

