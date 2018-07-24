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
			["switch"] = DeviceStateKey.SwitchState,
			["battery"] = DeviceStateKey.BatteryLevel,
			["lock"] = DeviceStateKey.Lock,
			["motion"] = DeviceStateKey.Motion,
			//["checkInterval"] = DeviceStateKey.in,
			["level"] = DeviceStateKey.Level,
			["energy"] = DeviceStateKey.Energy,
			["power"] = DeviceStateKey.Power,
			["about"] = DeviceStateKey.About,
			["networkAddress"] = DeviceStateKey.NetworkAddress,
			["status"] = DeviceStateKey.Status,
			["serialNumber"] = DeviceStateKey.SerialNumber,
			["indicatorStatus"] = DeviceStateKey.IndicatorStatus,
			["water"] = DeviceStateKey.Water,
			["button"] = DeviceStateKey.Button,
			["numberOfButtons"] = DeviceStateKey.NumberOfButtons,
			["hue"] = DeviceStateKey.Hue,
			["saturation"] = DeviceStateKey.Saturation,
			["color"] = DeviceStateKey.Color
		};

		static string StatusKey(this SmartThingsUpdate command)
		{
			var commandName = command?.Event?.Name;
			return CommandNames.TryGetValue(commandName, out var name) ? name : commandName;
		}
	}
}

