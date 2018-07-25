using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
namespace Rosie.Models.JsonConverters
{
	public class DeviceCommandJsonConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType) => typeof(DeviceCommandArguments).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());


		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			var typeName = jObject.Value<string>("CommandType");
			Type type = objectType;
			switch (typeName)
			{
				case nameof(DeviceCommandEnumArguments):
					type = typeof(DeviceCommandEnumArguments);
					break;
				case nameof(DeviceCommandIntegerArguments):
					type = typeof(DeviceCommandIntegerArguments);
					break;
				case nameof(DeviceCommandUrlArgument):
					type = typeof(DeviceCommandUrlArgument);
					break;
			}
			var obj = Activator.CreateInstance(type);
			serializer.Populate(jObject.CreateReader(), obj);

			return obj;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
