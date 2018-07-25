using System;
using System.Threading.Tasks;

namespace Rosie.Node
{
	public static class NodeExtensions
	{
		public static async Task<DeviceState> ToDeviceUpdate(this NodeValueUpdate update)
		{
			var command = await update.GetNodeCommand();
			if (command.ShouldIgnore())
				return null;
			var node = await NodeDatabase.Shared.GetDevice(update.NodeId);
			var statusKey = command.StatusKey();
			var dataType = command.GetDataType(update);
			return new DeviceState
			{
				DeviceId = node.Id,
				DataType = dataType,
				Value = update.Value.Value,
				PropertyKey = statusKey,
			};
		}

		public static async Task<NodeCommand> GetNodeCommand(this NodeValueUpdate update)
		{
			var command = await NodeDatabase.Shared.DatabaseConnection.Table<NodeCommand>().Where(x => x.ClassId == update.Value.ClassId && x.Index == update.Value.Index).FirstOrDefaultAsync();
			return command;
		}

		public static bool ShouldIgnore(this NodeCommand command)
		{
			var commandId = command?.Id;
			switch (commandId)
			{
				case "113 - 0":
				case "113 - 1":
				case "113 - 2":
					return true;
				default:
					return false;
			}
		}

		public static string StatusKey(this NodeCommand command)
		{
			var commandId = command?.Id;
			switch (commandId)
			{
				case "37 - 0":
				case "39 - 0":
					return DevicePropertyKey.SwitchState;
				case "117 - 0":
					return "Protection";
				case "32 - 0":
					return "Basic";
				case "48 - 0":
					return "Sensor";
				case "49 - 1":
					return DevicePropertyKey.Temperature;
				case "49 - 3":
					return DevicePropertyKey.Illuminance;
				case "49 - 25":
					return "Seismic Intensity";
				case "113 - 10":
					return "Burglar";
				case "128 - 0":
					return DevicePropertyKey.BatteryLevel;
			}
			return $"Unknown ({commandId})";
		}

		public static DataTypes GetDataType(this NodeCommand command, NodeValueUpdate update)
		{
			var commandId = command?.Id;
			switch (commandId)
			{
				case "37 - 0":
					return DataTypes.Bool;
				case "49 - 1":
					return DataTypes.Decimal;
			}
			if (update.Value.Values != null)
				return DataTypes.List;
			var s = update.Value.Value?.ToString();
			if (string.IsNullOrEmpty(s))
				return DataTypes.String;

			int i;
			if (int.TryParse(s, out i))
				return DataTypes.Int;
			double d;
			if (double.TryParse(s, out d))
				return DataTypes.Decimal;
			return DataTypes.String;
		}
	}
}

