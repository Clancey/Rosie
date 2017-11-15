using System;
using System.Collections.Generic;


namespace Rosie.SmartThings
{
	public static class Extensions
	{
		public static string CreateApiUrl(this UserLocation location, string path)
		{
			return new Uri(location.Shard.Api.Uri, path).AbsoluteUri;
		}

		public static bool ShouldIgnoreUpdate(this SmartThingsUpdate update) => !string.Equals(update?.Event?.EventSource, "DEVICE", StringComparison.OrdinalIgnoreCase);

		public static DeviceUpdate ToDeviceUpdate(this SmartThingsUpdate update)
		{
			return new DeviceUpdate
			{
				DeviceId = update.Event.DeviceId,

			};
		}

		static Dictionary<string, string> CommandNames = new Dictionary<string, string>
		{
			["switch"] = DeviceState.SwitchState,
			["battery"] = DeviceState.BatteryLevel,
			["lock"] = DeviceState.Lock,
			["motion"] = DeviceState.Motion,
			//["checkInterval"] = DeviceStateKeys.in,
			["level"] = DeviceState.Level,
			["energy"] = DeviceState.Energy,
			["power"] = DeviceState.Power,
			["about"] = DeviceState.About,
			["networkAddress"] = DeviceState.NetworkAddress,
			["status"] = DeviceState.Status,
			["serialNumber"] = DeviceState.SerialNumber,
			["indicatorStatus"] = DeviceState.IndicatorStatus,
			["water"] = DeviceState.Water,
			["button"] = DeviceState.Button,
			["numberOfButtons"] = DeviceState.NumberOfButtons,
			["hue"] = DeviceState.Hue,
			["saturation"] = DeviceState.Saturation,
			["color"] = DeviceState.Color
		};

		static string StatusKey(this SmartThingsUpdate command)
		{
			var commandName = command?.Event?.Name;
			return CommandNames.TryGetValue(commandName, out var name) ? name : commandName;
		}
	}
}

