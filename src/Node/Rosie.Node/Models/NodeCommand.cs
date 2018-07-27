using System;
using SQLite;

namespace Rosie.Node
{
	public enum CommandGenre
	{
		Basic,
		User,
		Config,
		System,
		Invalid,
	}
	public class NodeCommand
	{
		[PrimaryKey]
		public string Id { get { return $"{ClassId} - {Index}"; } set {; } }

		public int ClassId { get; set; }

		public int Index { get; set; }

		public int Instance { get; set; }

		public CommandGenre Genre { get; set; }

		public string Description { get; set; }

		public override bool Equals (object obj)
		{
			var command = obj as NodeCommand;
			if (command == null)
				return false;
			return command.Id == Id;
		}
		public override int GetHashCode ()
		{
			return Id.GetHashCode ();
		}

	}
}

