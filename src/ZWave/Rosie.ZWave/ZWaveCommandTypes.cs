﻿using System;
namespace Rosie.ZWave
{
	public enum ZWaveCommandTypes
	{
		Basic = 0x20,
		SwitchBinary = 0x25,
		SensorBinary = 0x30,
		SensorMultiLevel = 0x31,
		Meter = 0x32,
		Color = 0x33,
		ThermostatSetpoint = 0x43,
		MultiChannel = 0x60,
		Configuration = 0x70,
		Alarm = 0x71,
		ManufacturerSpecific = 0x72,
		Battery = 0x80,
		Clock = 0x81,
		WakeUp = 0x84,
		Association = 0x85,
		Version = 0x86,
		SensorAlarm = 0x9C,
	}
}

