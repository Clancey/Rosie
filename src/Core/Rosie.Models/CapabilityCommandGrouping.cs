using System;
using SQLite;
namespace Rosie.Models
{


	public class CapabilityCommandGrouping
	{
		[PrimaryKeyAttribute]
		public string Id
		{
			get
			{
				return $"{Capability} - {Command}";
			}
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			{
				//This is here just for SqlIte;
			}
		}

		[Indexed]
		public string Capability { get; set; }

		[Indexed]
		public string Command { get; set; }
	}
}
