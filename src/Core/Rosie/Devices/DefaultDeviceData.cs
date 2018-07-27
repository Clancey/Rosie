using System;
using System.Collections.Generic;

namespace Rosie.Devices
{
	public static class DefaultDeviceData
	{
		public static class DeviceTypes
		{
			public static readonly DeviceType Switch = new DeviceType
			{
				Key = DeviceTypeKeys.Switch,
				Capabilities = new[]{
					DeviceCapabilityKeys.Switch,
				},
				PrimaryCapability = DeviceCapabilityKeys.Switch,
			};

			public static readonly DeviceType DimmerSwitch = new DeviceType
			{
				Key = DeviceTypeKeys.DimmerSwitch,
				Capabilities = new[]{
					DeviceCapabilityKeys.SwitchLevel,
					DeviceCapabilityKeys.Switch,
				},
				PrimaryCapability = DeviceCapabilityKeys.Switch,
			};
		}

		public static class Capabilities
		{
			public static readonly DeviceCapability AccelerationSensor = new DeviceCapability
			{

				Key = DeviceCapabilityKeys.AccelerationSensor,
				Properties = new[] {
					DevicePropertyKey.Acceleration,
				},
			};

			public static readonly DeviceCapability Alarm = new DeviceCapability
			{
				Key = DeviceCapabilityKeys.Alarm,
				Properties = new[]{
					DevicePropertyKey.AlarmState,
				},
				Commands = new[]{
					DeviceCommandKey.Off,
				}
			};

			public static readonly DeviceCapability Battery = new DeviceCapability
			{
				Key = DeviceCapabilityKeys.Battery,
				Properties = new[]{
					DevicePropertyKey.BatteryLevel,
				},
			};

			public static readonly DeviceCapability Switch = new DeviceCapability
			{
				Key = DeviceCapabilityKeys.Switch,
				Properties = new[]{
					DevicePropertyKey.SwitchState,
				},
			};

			public static readonly DeviceCapability SwitchLevel = new DeviceCapability
			{
				Key = DeviceCapabilityKeys.SwitchLevel,
				Properties = new[]{
					DevicePropertyKey.SwitchState,
					DevicePropertyKey.Level,
				},
			};

			public static Dictionary<string, DeviceCapability> All = new Dictionary<string, DeviceCapability>
			{
				[DeviceCapabilityKeys.AccelerationSensor] = AccelerationSensor,
				[DeviceCapabilityKeys.Alarm] = Alarm,
				[DeviceCapabilityKeys.Switch] = Switch,
				[DeviceCapabilityKeys.SwitchLevel] = SwitchLevel,
				[DeviceCapabilityKeys.Battery] = Battery,
			};
		}

		public static class Commands
		{
			public readonly static DeviceCommand AlarmStateCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.AlarmState,
				Arguments = new[]{
					new DeviceCommandEnumArguments {
						Required = true,
						Options = new[] { "siren", "strobe", "both", "off" },
					}
				}
			};

			public readonly static DeviceCommand CloseCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Close,
			};

			public readonly static DeviceCommand ColorCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Color,
				Arguments = new [] {
					new DeviceCommandColorArgument {
						Required = true
					}
				}
			};

			public readonly static DeviceCommand ColorTemperatureCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.ColorTemperature,
				Arguments = new[] {
					new DeviceCommandIntegerArguments {
						Required = true,
						LowerLimit = 1,
						UpperLimit = 30000
					}
				}
			};

			public readonly static DeviceCommand ConfigureCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Configure,
			};

			public readonly static DeviceCommand CoolingSetpointCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.CoolingSetpoint,
				Arguments = new[] {
					new DeviceCommandTemperatureArgument {
						Required = true,
					}
				}
			};

			public readonly static DeviceCommand HueCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Hue,
				Arguments = new[] {
					new DeviceCommandIntegerArguments {
						Required = true,
						LowerLimit = 0,
					}
				}
			};

			public readonly static DeviceCommand InfraredLevelCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.InfraredLevel,
				Arguments = new[] {
					new DeviceCommandIntegerArguments {
						Required = true,
						LowerLimit = 0,
						UpperLimit = 100
					}
				}
			};

			public readonly static DeviceCommand MuteStateCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Mute,
				Arguments = new[] {
					new DeviceCommandEnumArguments {
						Required = true,
						Options = new [] { "mute", "unmute" }
					}
				}
			};

			public readonly static DeviceCommand NotificationCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Notify,
				Arguments = new[] {
					new DeviceCommandStringArgument {
						Required = true,
					}
				}
			};

			public readonly static DeviceCommand OffCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Off
			};

			public readonly static DeviceCommand OnCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.On
			};

			public readonly static DeviceCommand OpenCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Open,
			};

			public readonly static DeviceCommand PushCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Push,
			};

			public readonly static DeviceCommand RefreshCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Refresh,
			};

			public readonly static DeviceCommand SaturationCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Saturation,
				Arguments = new[] {
					new DeviceCommandColorArgument {
						Required = true
					}
				}
			};

			public readonly static DeviceCommand SwitchLevelCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.SwitchLevel,
				Arguments = new[]{ new DeviceCommandIntegerArguments{
						Required = true,
						LowerLimit = 0,
						UpperLimit = 100,
					}
				}
			};

			public static Dictionary<string, DeviceCommand> All = new Dictionary<string, DeviceCommand>
			{
				[DeviceCommandKey.AlarmState] = AlarmStateCommand,
				[DeviceCommandKey.Close] = CloseCommand,
				[DeviceCommandKey.Color] = ColorCommand,
				[DeviceCommandKey.ColorTemperature] = ColorTemperatureCommand,
				[DeviceCommandKey.Configure] = ConfigureCommand,
				[DeviceCommandKey.CoolingSetpoint] = CoolingSetpointCommand,
				[DeviceCommandKey.Hue] = HueCommand,
				[DeviceCommandKey.InfraredLevel] = InfraredLevelCommand,
				[DeviceCommandKey.Mute] = MuteStateCommand,
				[DeviceCommandKey.Notification] = NotificationCommand,
				[DeviceCommandKey.Off] = OffCommand,
				[DeviceCommandKey.On] = OnCommand,
				[DeviceCommandKey.Open] = OpenCommand,
				[DeviceCommandKey.Push] = PushCommand,
				[DeviceCommandKey.Refresh] = RefreshCommand,
				[DeviceCommandKey.Saturation] = SaturationCommand,
				[DeviceCommandKey.SwitchLevel] = SwitchLevelCommand,
			};

		}

		public static class Properties
		{
			public static readonly DeviceProperty Acceleration = new DeviceProperty
			{
				Key = DevicePropertyKey.Acceleration,
				IsReadonly = true,
				DataType = DataTypes.Bool,
			};

			public static readonly DeviceProperty AlarmState = new DeviceProperty
			{
				Key = DevicePropertyKey.AlarmState,
				IsReadonly = false,
				DataType = DataTypes.String,
				Arguments = new DeviceCommandEnumArguments
				{
					Required = true,
					Options = new[] { "siren", "strobe", "both", "off" },
				},
			};

			public static readonly DeviceProperty SwitchState = new DeviceProperty
			{
				Key = DevicePropertyKey.SwitchState,
				IsReadonly = false,
				DataType = DataTypes.Bool,
			};

			public static readonly DeviceProperty Level = new DeviceProperty
			{
				Key = DevicePropertyKey.Level,
				IsReadonly = false,
				DataType = DataTypes.Int,
				Arguments = new DeviceCommandIntegerArguments
				{
					Required = true,
					LowerLimit = 0,
					UpperLimit = 100,

				}
			};

			public static readonly DeviceProperty BatteryLevel = new DeviceProperty
			{
				Key = DevicePropertyKey.BatteryLevel,
				IsReadonly = true,
				DataType = DataTypes.Decimal,
			};

			public static Dictionary<string, DeviceProperty> All = new Dictionary<string, DeviceProperty>
			{
				[DevicePropertyKey.Acceleration] = Acceleration,
				[DevicePropertyKey.AlarmState] = AlarmState,
				[DevicePropertyKey.SwitchState] = SwitchState,
				[DevicePropertyKey.Level] = Level,
				[DevicePropertyKey.BatteryLevel] = BatteryLevel,
			};

		}
	}
}
