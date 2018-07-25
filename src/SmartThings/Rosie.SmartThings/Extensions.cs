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
			["switch"] = DevicePropertyKey.SwitchState,
			["battery"] = DevicePropertyKey.BatteryLevel,
			["lock"] = DevicePropertyKey.Lock,
			["motion"] = DevicePropertyKey.Motion,
			//["checkInterval"] = DeviceStateKey.in,
			["level"] = DevicePropertyKey.Level,
			["energy"] = DevicePropertyKey.Energy,
			["power"] = DevicePropertyKey.Power,
			["about"] = DevicePropertyKey.About,
			["networkAddress"] = DevicePropertyKey.NetworkAddress,
			["status"] = DevicePropertyKey.Status,
			["serialNumber"] = DevicePropertyKey.SerialNumber,
			["indicatorStatus"] = DevicePropertyKey.IndicatorStatus,
			["water"] = DevicePropertyKey.Water,
			["button"] = DevicePropertyKey.Button,
			["numberOfButtons"] = DevicePropertyKey.NumberOfButtons,
			["hue"] = DevicePropertyKey.Hue,
			["saturation"] = DevicePropertyKey.Saturation,
			["color"] = DevicePropertyKey.Color
		};

		static string StatusKey(this SmartThingsUpdate command)
		{
			var commandName = command?.Event?.Name;
			return CommandNames.TryGetValue(commandName, out var name) ? name : commandName;
		}
	}
}

