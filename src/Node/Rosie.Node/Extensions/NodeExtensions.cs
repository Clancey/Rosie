using System;
using System.Threading.Tasks;

namespace Rosie.Node
{
	public static class NodeExtensions
	{
		public static bool Update(this Device device, NodeDevice nodeDevice)
		{
			bool didChange = false;
			device.ServiceDeviceId = nodeDevice.NodeId.ToString();
			if (device.Description != nodeDevice.Name)
			{
				device.Description = nodeDevice.Name;
				didChange = true;
			}
			if (device.Location != nodeDevice.Loc)
			{
				device.Location = nodeDevice.Loc;
				didChange = true;
			}
			if (device.Manufacturer != nodeDevice.Manufacturer)
			{
				device.Manufacturer = nodeDevice.Manufacturer;
				didChange = true;
			}
			if (device.ManufacturerId != nodeDevice.ManufacturerId)
			{
				device.ManufacturerId = nodeDevice.ManufacturerId;
				didChange = true;
			}
			if (device.Product != nodeDevice.Product)
			{
				device.Product = nodeDevice.Product;
				didChange = true;
			}
			if (device.ProductType != nodeDevice.ProductType)
			{
				device.ProductType = nodeDevice.ProductType;
				didChange = true;
			}
			if (device.ProductId != nodeDevice.ProductId)
			{
				device.ProductId = nodeDevice.ProductId;
				didChange = true;
			}
			var decentName = nodeDevice.DecentName();
			if (device.Name != decentName)
			{
				device.Name = decentName;
				didChange = true;
			}
			if (device.Type != nodeDevice.Type)
			{
				device.Type = nodeDevice.Type;
				didChange = true;
			}
			var deviceType = nodeDevice.GetDeviceType();
			if (device.DeviceType != deviceType)
			{
				device.DeviceType = deviceType;
				didChange = true;
			}
			return didChange || string.IsNullOrWhiteSpace(device.Id);
		}

		internal static string GetDeviceType(this NodeDevice device)
		{
			var type = device.Type;
			switch (type)
			{
				case "Binary Power Switch":
				case "Secure Keypad Door Lock":
					return DeviceTypeKeys.Switch;
				case "Multilevel Scene Switch":
					return DeviceTypeKeys.DimmerSwitch;
			}
			return "Unknown";
		}

		public static string DecentName(this NodeDevice device)
		{
			if (!string.IsNullOrWhiteSpace(device.Name))
				return device.Name;
			if (!string.IsNullOrEmpty(device.Manufacturer))
				return $"{device.Manufacturer} {device.Type}";
			return null;
		}
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
				Value = update?.Value?.Value,
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

