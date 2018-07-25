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
			public readonly static DeviceCommand RefreshCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Refresh,
			};

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

			public readonly static DeviceCommand OnCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.On
			};

			public readonly static DeviceCommand OffCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Off
			};

			public readonly static DeviceCommand SwitchLevelCommand = new DeviceCommand
			{
				Command = DeviceCommandKey.Level,
				Arguments = new[]{ new DeviceCommandIntegerArguments{
						Required = true,
						LowerLimit = 0,
						UpperLimit = 100,
					}
				}
			};
			public static Dictionary<string, DeviceCommand> All = new Dictionary<string, DeviceCommand>
			{
				[DeviceCommandKey.Refresh] = RefreshCommand,
				[DeviceCommandKey.AlarmState] = AlarmStateCommand,
				[DeviceCommandKey.On] = OnCommand,
				[DeviceCommandKey.Off] = OffCommand,
				[DeviceCommandKey.Level] = SwitchLevelCommand,
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
