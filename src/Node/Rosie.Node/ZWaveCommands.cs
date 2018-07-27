using System;
using System.Collections.Generic;
using System.Linq;
namespace Rosie.Node
{
	public static class ZWaveCommands
	{
		public static Dictionary<string, string> RosieCommandsToZwaveDictionary = new Dictionary<string, string>
		{
			[DevicePropertyKey.SwitchState] = "37 - 0",
			["basic"] = "32 - 0",  //{"value_id":"1-32-1-0","node_id":1,"class_id":32,"type":"byte","genre":"basic","instance":1,"index":0,"label":"Basic","units":"","help":"","read_only":false,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":0}
			[DevicePropertyKey.Level] = "38 - 0",  //{"value_id":"2-38-1-0","node_id":2,"class_id":38,"type":"byte","genre":"user","instance":1,"index":0,"label":"Level","units":"","help":"","read_only":false,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":68}
			["bright"] = "38 - 1",  //{"value_id":"2-38-1-1","node_id":2,"class_id":38,"type":"button","genre":"user","instance":1,"index":1,"label":"Bright","units":"","help":"","read_only":false,"write_only":true,"is_polled":false,"min":0,"max":0,"values":null,"value":null}
			["dim"] = "38 - 2",  //{"value_id":"2-38-1-2","node_id":2,"class_id":38,"type":"button","genre":"user","instance":1,"index":2,"label":"Dim","units":"","help":"","read_only":false,"write_only":true,"is_polled":false,"min":0,"max":0,"values":null,"value":null}
			[DevicePropertyKey.Sensor] = "48 - 0",  //{"value_id":"3-48-1-0","node_id":3,"class_id":48,"type":"bool","genre":"user","instance":1,"index":0,"label":"Sensor","units":"","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":0,"values":null,"value":true}
			[DevicePropertyKey.Temperature] = "49 - 1",  //{"value_id":"3-49-1-1","node_id":3,"class_id":49,"type":"decimal","genre":"user","instance":1,"index":1,"label":"Temperature","units":"F","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":0,"values":null,"value":"74.3"}
			[DevicePropertyKey.Luminance] = "49 - 3",  //{"value_id":"3-49-1-3","node_id":3,"class_id":49,"type":"decimal","genre":"user","instance":1,"index":3,"label":"Luminance","units":"lux","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":0,"values":null,"value":"0"}
			[DevicePropertyKey.Humidity] = "49 - 5",  //{"value_id":"3-49-1-5","node_id":3,"class_id":49,"type":"decimal","genre":"user","instance":1,"index":5,"label":"Relative Humidity","units":"%","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":0,"values":null,"value":"52"}
			[DevicePropertyKey.UltravioletIndex] = "49 - 27",  //{"value_id":"3-49-1-27","node_id":3,"class_id":49,"type":"decimal","genre":"user","instance":1,"index":27,"label":"Ultraviolet","units":"","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":0,"values":null,"value":"0"}
			[DevicePropertyKey.AlarmType] = "113 - 0",  //{"value_id":"3-113-1-0","node_id":3,"class_id":113,"type":"byte","genre":"user","instance":1,"index":0,"label":"Alarm Type","units":"","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":0}
			["alarmLevel"] = "113 - 1",  //{"value_id":"3-113-1-1","node_id":3,"class_id":113,"type":"byte","genre":"user","instance":1,"index":1,"label":"Alarm Level","units":"","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":0}
			["sourceNodeId"] = "113 - 2",  //{"value_id":"3-113-1-2","node_id":3,"class_id":113,"type":"byte","genre":"user","instance":1,"index":2,"label":"SourceNodeId","units":"","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":0}
			["burglar"] = "113 - 10",  //{"value_id":"3-113-1-10","node_id":3,"class_id":113,"type":"byte","genre":"user","instance":1,"index":10,"label":"Burglar","units":"","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":0}
			[DevicePropertyKey.BatteryLevel] = "128 - 0",  //{"value_id":"3-128-1-0","node_id":3,"class_id":128,"type":"byte","genre":"user","instance":1,"index":0,"label":"Battery Level","units":"%","help":"","read_only":true,"write_only":false,"is_polled":false,"min":0,"max":255,"values":null,"value":100}

		};


		public static Dictionary<string, string> ZWaveCommandsToRosieCommandsDictionary = RosieCommandsToZwaveDictionary.ToDictionary(x => x.Value, x => x.Key);
	}
}
