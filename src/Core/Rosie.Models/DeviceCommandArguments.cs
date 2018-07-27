using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rosie.Models.JsonConverters;
namespace Rosie
{

	[JsonConverter(typeof(DeviceCommandJsonConverter))]
	public class DeviceCommandArguments
	{
		public string Name { get; set; }
		public DataTypes DataType { get; set; }
		public string CommandType { get; set; } = nameof(DeviceCommand);
		public bool Required { get; set; }

		public virtual bool IsValid(object value) => !Required || value != null;

	}

	public class DeviceCommandUrlArgument : DeviceCommandArguments
	{
		public DeviceCommandUrlArgument()
		{
			DataType = DataTypes.String;
			CommandType = nameof(DeviceCommandArguments);
		}
		public string Url { get; set; }
	}

	public class DeviceCommandColorArgument : DeviceCommandArguments
	{
		public DeviceCommandColorArgument()
		{
			DataType = DataTypes.String;
			CommandType = nameof(DeviceCommandArguments);
		}
		public string Color { get; set; }
	}

	public class DeviceCommandStringArgument : DeviceCommandArguments
	{
		public DeviceCommandStringArgument()
		{
			DataType = DataTypes.String;
			CommandType = nameof(DeviceCommandArguments);
		}
		public string Value { get; set; }
	}

	public class DeviceCommandTemperatureArgument : DeviceCommandArguments
	{
		public DeviceCommandTemperatureArgument()
		{
			DataType = DataTypes.Decimal;
			CommandType = nameof(DeviceCommandArguments);
		}
		public double DegreesCelsius { get; set; }

		// TODO: DegreesFarenheit which wraps DegreesCelsius
	}

	public class DeviceCommandEnumArguments : DeviceCommandArguments
	{
		public DeviceCommandEnumArguments()
		{
			DataType = DataTypes.String;
			CommandType = nameof(DeviceCommandEnumArguments);
		}

		public IList<string> Options { get; set; }

		public override bool IsValid(object value)
		{
			var stringValue = value as String;
			return (Options?.Contains(stringValue) ?? false) || (string.IsNullOrWhiteSpace(stringValue) && !Required);
		}
	}

	public class DeviceCommandIntegerArguments : DeviceCommandArguments
	{
		public DeviceCommandIntegerArguments()
		{
			DataType = DataTypes.Int;
			CommandType = nameof(DeviceCommandEnumArguments);
		}

		public int? LowerLimit { get; set; }
		public int? UpperLimit { get; set; }
		public override bool IsValid(object value)
		{
			var intergerValue = value as int?;
			if (!intergerValue.HasValue && !Required)
				return true;
			if (UpperLimit.HasValue && intergerValue > UpperLimit)
			{
				return false;
			}
			if (LowerLimit.HasValue && intergerValue < LowerLimit)
			{
				return false;
			}

			return true;
		}
	}
}
