using System;
using System.Threading.Tasks;

namespace Rosie.Node
{
	public static class NodeExtensions
	{
		public static async Task<DeviceState> ToDeviceUpdate (this NodeValueUpdate update)
		{
			var command = await update.GetNodeCommand ();
			var node = await NodeDatabase.Shared.GetDevice (update.NodeId);
			var statusKey = command.StatusKey ();
			var dataType = command.GetDataType ();
			return new DeviceState {
				DeviceId = node.Id,
				DataType = dataType,
				Value = update.Value.Value,
				Key = statusKey,
			};
		}

		public static async Task<NodeCommand> GetNodeCommand (this NodeValueUpdate update)
		{
			var command = await NodeDatabase.Shared.DatabaseConnection.Table<NodeCommand> ().Where (x => x.ClassId == update.Value.ClassId && x.Index == update.Value.Index).FirstOrDefaultAsync ();
			return command;
		}

		public static string StatusKey (this NodeCommand command)
		{
			var commandId = command?.Id;
			switch (commandId) {
			case "37 - 0":
			case "39 - 0":
				return "Switch State";
			case "117 - 0":
				return "Protection";
			case "32 - 0":
				return "Basic";
			case "48 - 0":
				return "Sensor";
			case "49 - 1":
				return "Temperature";
			case "49 - 3":
				return "Luminance";
			case "49 - 25":
				return "Seismic Intensity";
			case "113 - 10":
				return "Burglar";
			case "128 - 0":
				return "Battery Level";
			}
			return $"Unknown ({commandId})";
		}

		public static DataTypes GetDataType (this NodeCommand command)
		{
			var commandId = command?.Id;
			switch (commandId) {
			case "37 - 0":
			case "39 - 0":
				return DataTypes.Bool;
			//case "117 - 0":
			//	return "Protection";
			//case "32 - 0":
			//	return "Basic";
			//case "48 - 0":
			//	return "Sensor";
			//case "49 - 1":
			//	return "Temperature";
			//case "49 - 3":
			//	return "Luminance";
			//case "49 - 25":
			//	return "Seismic Intensity";
			//case "113 - 10":
			//	return "Burglar";
			//case "128 - 0":
			//	return "Battery Level";
			}
			return DataTypes.Raw;
		}
	}
}

