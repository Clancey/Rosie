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
				return  $"{DeviceId} - {PropertyKey}";
			}
			//Not needed, here for Sqlite-net
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set
			{
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter

			}
		}

		public string GroupType { get; set; }
	}
}

