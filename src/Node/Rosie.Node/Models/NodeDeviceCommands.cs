using System;
namespace Rosie.Node
{
	public class NodeDeviceCommands
	{
		[SQLite.PrimaryKey]
		public string Id { get { return $"{NodeId} - {CommandId}"; } set { } }

		public int NodeId { get; set; }
		public string CommandId { get { return $"{ClassId} - {Instance} - {Index}"; } set { } }
		public string SimpleStringCommand => $"{ClassId} - {Index}";
		public CommandGenre Genre { get; set; }
		public int ClassId { get; set; }
		public int Instance { get; set; }
		public int Index { get; set; }
		public string Units { get; set; }
		public string Description { get; set; }
		public string Help { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsWriteOnly { get; set; }
		public int Min { get; set; }
		public int Max { get; set; }
		public string Values { get; set; }
	}
}

