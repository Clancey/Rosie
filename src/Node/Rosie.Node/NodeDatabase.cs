using System;
using System.Threading.Tasks;
using SQLite;
using System.Linq;
using System.Collections.Generic;

namespace Rosie.Node
{
	public class NodeDatabase
	{
		public static NodeDatabase Shared { get; set; } = new NodeDatabase ();

		public SQLiteAsyncConnection DatabaseConnection { get; set; }

		public NodeDatabase ()
		{
			DatabaseConnection = DeviceDatabase.Shared.DatabaseConnection;
			var s = DatabaseConnection.CreateTablesAsync<NodeDeviceCommands,NodeDevice, NodeCommand> ().Result;
		}

		internal async Task InsertDevice (NodeDevice nodeDevice)
		{
			if (string.IsNullOrWhiteSpace (nodeDevice.PerferedCommand)) {
				var oldDevice = await GetDevice (nodeDevice.NodeId);
				if(oldDevice != null)
					nodeDevice.PerferedCommand = oldDevice.PerferedCommand;
			}
			await DatabaseConnection.InsertOrReplaceAsync (nodeDevice);
			var nodeValues = nodeDevice.Classes?.SelectMany (x => x.Value.Select (y => y.Value)).ToList ();
			var commands = nodeValues?.Select (x => new NodeCommand { ClassId = x.ClassId, Index = x.Index, Genre = Enum.Parse<CommandGenre> (x.Genre), Description = x.Label }).ToList ();
			var grouped = nodeValues?.Select (x => new NodeDeviceCommands {
				ClassId = x.ClassId,
				Index = x.Index,
				Instance = x.Instance,
				NodeId = x.NodeId,
				Help = x.Help,
				Description = x.Label,
				IsReadOnly = x.ReadOnly,
				IsWriteOnly = x.WriteOnly,
				Max = x.Max,
				Min = x.Min,
				Units = x.Units,
				Values =  x.Values?.ToJson (),
				Genre = Enum.Parse<CommandGenre> (x.Genre),
			}).ToList ();
			if(commands != null)
				await DatabaseConnection.InsertOrReplaceAllAsync (commands);
			if(grouped != null)
				await DatabaseConnection.InsertOrReplaceAllAsync (grouped);
			//var commands = nodeDevice.Classes.Select(x=> new NodeCommand{CommandId = x.Key, 
		}

		public Task<NodeDeviceCommands> GetDeviceCommand (string id)
		{
			return DatabaseConnection.Table<NodeDeviceCommands> ().Where (x => x.Id == id).FirstOrDefaultAsync();
		}
		public Task<List<NodeDeviceCommands>> GetDeviceCommands (int nodeId)
		{
			return DatabaseConnection.Table<NodeDeviceCommands> ().Where (x => x.NodeId == nodeId).ToListAsync ();
		}
		public Task<NodeDevice> GetDevice (int nodeId)
		{

			return DatabaseConnection.Table<NodeDevice> ().Where (x => x.NodeId == nodeId).FirstOrDefaultAsync ();
		}

		public Task<NodeDevice> GetDevice (string deviceId)
		{
			return DatabaseConnection.Table<NodeDevice> ().Where (x => x.Id == deviceId).FirstOrDefaultAsync ();
		}
	}
}

