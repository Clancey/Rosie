using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rosie.SmartThings
{
	public class UserInfo
	{
		[JsonProperty ("uuid")]
		public string UUID { get; set; }

		[JsonProperty ("username")]
		public string Username { get; set; }

		[JsonProperty ("email")]
		public string Email { get; set; }

		[JsonProperty ("fullName")]
		public string FullName { get; set; }
	}

	public class ServerConnection
	{

		[JsonProperty ("scheme")]
		public string Scheme { get; set; }

		[JsonProperty ("host")]
		public string Host { get; set; }

		[JsonProperty ("port")]
		public string Port { get; set; }

		public string UrlPrefix => Port == "443" ? "https" : "http";
		public Uri Uri => new Uri ($"{UrlPrefix}://{Host}");

	}



	public class Support
	{

		[JsonProperty ("welcomeUrl")]
		public string WelcomeUrl { get; set; }

		[JsonProperty ("customerPicsUrl")]
		public string CustomerPicsUrl { get; set; }

		[JsonProperty ("supportUrl")]
		public string SupportUrl { get; set; }

		[JsonProperty ("chatId")]
		public string ChatId { get; set; }

		[JsonProperty ("email")]
		public string Email { get; set; }
	}

	public class Shard
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("api")]
		public ServerConnection Api { get; set; }

		[JsonProperty ("client")]
		public ServerConnection Client { get; set; }

		[JsonProperty ("dev")]
		public ServerConnection Dev { get; set; }

		[JsonProperty ("marketplace")]
		public ServerConnection Marketplace { get; set; }

		[JsonProperty ("videoconn")]
		public object Videoconn { get; set; }

		[JsonProperty ("videohost")]
		public ServerConnection Videohost { get; set; }

		[JsonProperty ("apigateway")]
		public ServerConnection Apigateway { get; set; }

		[JsonProperty ("support")]
		public Support Support { get; set; }
	}

	public class UserLocation
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("backgroundImage")]
		public string BackgroundImage { get; set; }

		[JsonProperty ("shard")]
		public Shard Shard { get; set; }

		[JsonProperty ("region")]
		public string Region { get; set; }
	}

	public class Mode
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("locationId")]
		public string LocationId { get; set; }
	}

	public class Data
	{

		[JsonProperty ("bluetoothRadioDetected")]
		public string BluetoothRadioDetected { get; set; }

		[JsonProperty ("macAddress")]
		public string MacAddress { get; set; }

		[JsonProperty ("bluetoothRadioFunctional")]
		public string BluetoothRadioFunctional { get; set; }

		[JsonProperty ("batteryInUse")]
		public string BatteryInUse { get; set; }

		[JsonProperty ("zwaveSucID")]
		public string ZwaveSucID { get; set; }

		[JsonProperty ("zigbeePanID")]
		public string ZigbeePanID { get; set; }

		[JsonProperty ("zwaveRadioEnabled")]
		public string ZwaveRadioEnabled { get; set; }

		[JsonProperty ("localSrvPortUDP")]
		public string LocalSrvPortUDP { get; set; }

		[JsonProperty ("zigbeeChannel")]
		public string ZigbeeChannel { get; set; }

		[JsonProperty ("zwaveRadioFunctional")]
		public string ZwaveRadioFunctional { get; set; }

		[JsonProperty ("zwaveSerialVersion")]
		public string ZwaveSerialVersion { get; set; }

		[JsonProperty ("zwaveRegion")]
		public string ZwaveRegion { get; set; }

		[JsonProperty ("zigbeeNodeID")]
		public string ZigbeeNodeID { get; set; }

		[JsonProperty ("zwaveHomeID")]
		public string ZwaveHomeID { get; set; }

		[JsonProperty ("updaterVersion")]
		public string UpdaterVersion { get; set; }

		[JsonProperty ("zigbeePowerLevel")]
		public string ZigbeePowerLevel { get; set; }

		[JsonProperty ("zigbeeUnsecureRejoin")]
		public string ZigbeeUnsecureRejoin { get; set; }

		[JsonProperty ("appengineVersion")]
		public string AppengineVersion { get; set; }

		[JsonProperty ("localIP")]
		public string LocalIP { get; set; }

		[JsonProperty ("bootloaderVersion")]
		public string BootloaderVersion { get; set; }

		[JsonProperty ("zigbeeRadioDetected")]
		public string ZigbeeRadioDetected { get; set; }

		[JsonProperty ("localSrvPortTCP")]
		public string LocalSrvPortTCP { get; set; }

		[JsonProperty ("zigbeeFirmware")]
		public string ZigbeeFirmware { get; set; }

		[JsonProperty ("zwaveVersion")]
		public string ZwaveVersion { get; set; }

		[JsonProperty ("zwaveNodeID")]
		public string ZwaveNodeID { get; set; }

		[JsonProperty ("zwaveRadioDetected")]
		public string ZwaveRadioDetected { get; set; }

		[JsonProperty ("zwavePowerLevel")]
		public string ZwavePowerLevel { get; set; }

		[JsonProperty ("appengineEnabled")]
		public string AppengineEnabled { get; set; }

		[JsonProperty ("videocoreVersion")]
		public string VideocoreVersion { get; set; }

		[JsonProperty ("hardwareID")]
		public string HardwareID { get; set; }

		[JsonProperty ("zigbeeRadioEnabled")]
		public string ZigbeeRadioEnabled { get; set; }

		[JsonProperty ("uptime")]
		public string Uptime { get; set; }

		[JsonProperty ("presenceTimeout")]
		public string PresenceTimeout { get; set; }

		[JsonProperty ("appengineConnected")]
		public string AppengineConnected { get; set; }

		[JsonProperty ("hubcoreVersion")]
		public string HubcoreVersion { get; set; }

		[JsonProperty ("zigbeeRadioFunctional")]
		public string ZigbeeRadioFunctional { get; set; }

		[JsonProperty ("zigbeeEui")]
		public string ZigbeeEui { get; set; }

		[JsonProperty ("zwaveControllerStatus")]
		public string ZwaveControllerStatus { get; set; }

		[JsonProperty ("bluetoothRadioEnabled")]
		public string BluetoothRadioEnabled { get; set; }

		[JsonProperty ("backupVersion")]
		public string BackupVersion { get; set; }
	}

	public class Type
	{

		[JsonProperty ("name")]
		public string Name { get; set; }
	}

	public class Hub
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("locationId")]
		public string LocationId { get; set; }

		[JsonProperty ("firmwareVersion")]
		public string FirmwareVersion { get; set; }

		[JsonProperty ("zigbeeId")]
		public string ZigbeeId { get; set; }

		[JsonProperty ("status")]
		public string Status { get; set; }

		[JsonProperty ("onlineSince")]
		public DateTime OnlineSince { get; set; }

		[JsonProperty ("signalStrength")]
		public string SignalStrength { get; set; }

		[JsonProperty ("batteryLevel")]
		public object BatteryLevel { get; set; }

		[JsonProperty ("data")]
		public Data Data { get; set; }

		[JsonProperty ("type")]
		public Type Type { get; set; }

		[JsonProperty ("virtual")]
		public bool Virtual { get; set; }

		[JsonProperty ("role")]
		public string Role { get; set; }

		[JsonProperty ("firmwareUpdateAvailable")]
		public bool FirmwareUpdateAvailable { get; set; }

		[JsonProperty ("hardwareId")]
		public string HardwareId { get; set; }

		[JsonProperty ("hardwareDescription")]
		public string HardwareDescription { get; set; }

		[JsonProperty ("hardwareType")]
		public string HardwareType { get; set; }

	}

	public class Features
	{

		[JsonProperty ("contactBook")]
		public bool ContactBook { get; set; }

		[JsonProperty ("hasVoice")]
		public bool HasVoice { get; set; }
	}

	public class Location
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("accountId")]
		public string AccountId { get; set; }

		[JsonProperty ("latitude")]
		public double Latitude { get; set; }

		[JsonProperty ("longitude")]
		public double Longitude { get; set; }

		[JsonProperty ("additionalSetupRequired")]
		public string AdditionalSetupRequired { get; set; }

		[JsonProperty ("regionRadius")]
		public int RegionRadius { get; set; }

		[JsonProperty ("backgroundImage")]
		public string BackgroundImage { get; set; }

		[JsonProperty ("mode")]
		public Mode Mode { get; set; }

		[JsonProperty ("modes")]
		public IList<Mode> Modes { get; set; }

		[JsonProperty ("role")]
		public string Role { get; set; }

		[JsonProperty ("helloHomeAppId")]
		public string HelloHomeAppId { get; set; }

		[JsonProperty ("temperatureScale")]
		public string TemperatureScale { get; set; }

		[JsonProperty ("hubs")]
		public IList<Hub> Hubs { get; set; }

		[JsonProperty ("features")]
		public Features Features { get; set; }

		[JsonProperty ("timeZoneId")]
		public string TimeZoneId { get; set; }
	}

	public class CurrentState
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("hubId")]
		public string HubId { get; set; }

		[JsonProperty ("isVirtualHub")]
		public bool IsVirtualHub { get; set; }

		[JsonProperty ("description")]
		public string Description { get; set; }

		[JsonProperty ("rawDescription")]
		public string RawDescription { get; set; }

		[JsonProperty ("displayed")]
		public bool Displayed { get; set; }

		[JsonProperty ("isStateChange")]
		public bool IsStateChange { get; set; }

		[JsonProperty ("linkText")]
		public string LinkText { get; set; }

		[JsonProperty ("date")]
		public DateTime Date { get; set; }

		[JsonProperty ("unixTime")]
		public object UnixTime { get; set; }

		[JsonProperty ("value")]
		public string Value { get; set; }

		[JsonProperty ("viewed")]
		public bool Viewed { get; set; }

		[JsonProperty ("translatable")]
		public bool Translatable { get; set; }

		[JsonProperty ("archivable")]
		public bool Archivable { get; set; }

		[JsonProperty ("deviceId")]
		public string DeviceId { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("locationId")]
		public string LocationId { get; set; }

		[JsonProperty ("eventSource")]
		public string EventSource { get; set; }

		[JsonProperty ("deviceTypeId")]
		public string DeviceTypeId { get; set; }

		[JsonProperty ("data")]
		public string Data { get; set; }

		[JsonProperty ("unit")]
		public string Unit { get; set; }

		[JsonProperty ("handlerName")]
		public string HandlerName { get; set; }

	}

	public class StateOverride
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("tileName")]
		public string TileName { get; set; }

		[JsonProperty ("stateName")]
		public string StateName { get; set; }

		[JsonProperty ("icon")]
		public string Icon { get; set; }
	}

	public class Device
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("hubId")]
		public string HubId { get; set; }

		[JsonProperty ("locationId")]
		public string LocationId { get; set; }

		[JsonProperty ("label")]
		public string Label { get; set; }

		[JsonProperty ("status")]
		public string Status { get; set; }

		[JsonProperty ("currentStates")]
		public IList<CurrentState> CurrentStates { get; set; }

		[JsonProperty ("typeId")]
		public string TypeId { get; set; }

		[JsonProperty ("typeName")]
		public string TypeName { get; set; }

		[JsonProperty ("deviceNetworkId")]
		public string DeviceNetworkId { get; set; }

		[JsonProperty ("virtual")]
		public bool Virtual { get; set; }

		[JsonProperty ("primaryTileName")]
		public object PrimaryTileName { get; set; }

		[JsonProperty ("completedSetup")]
		public bool CompletedSetup { get; set; }

		[JsonProperty ("network")]
		public string Network { get; set; }

		[JsonProperty ("stateOverrides")]
		public IList<StateOverride> StateOverrides { get; set; }

		[JsonProperty ("role")]
		public string Role { get; set; }

		[JsonProperty ("parentSmartAppId")]
		public string ParentSmartAppId { get; set; }

		[JsonProperty ("isExecutingLocally")]
		public bool IsExecutingLocally { get; set; }

		[JsonProperty ("sortOrder")]
		public int? SortOrder { get; set; }

		[JsonProperty ("backgroundImage")]
		public object BackgroundImage { get; set; }

		[JsonProperty ("heroDeviceId")]
		public string HeroDeviceId { get; set; }

		[JsonProperty ("dateCreated")]
		public DateTime? DateCreated { get; set; }

	}

    public class LocationResponse
	{

		[JsonProperty ("location")]
		public Location Location { get; set; }

		[JsonProperty ("devices")]
		public IList<Device> Devices { get; set; }

		[JsonProperty ("hubs")]
		public IList<Hub> Hubs { get; set; }

		[JsonProperty ("events")]
		public IList<object> Events { get; set; }
	}



	public class Event
	{
		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("hubId")]
		public string HubId { get; set; }

		[JsonProperty ("isVirtualHub")]
		public bool IsVirtualHub { get; set; }

		[JsonProperty ("description")]
		public string Description { get; set; }

		[JsonProperty ("rawDescription")]
		public string RawDescription { get; set; }

		[JsonProperty ("displayed")]
		public bool Displayed { get; set; }

		[JsonProperty ("isStateChange")]
		public bool IsStateChange { get; set; }

		[JsonProperty ("linkText")]
		public string LinkText { get; set; }

		[JsonProperty ("date")]
		public DateTime Date { get; set; }

		[JsonProperty ("unixTime")]
		public long UnixTime { get; set; }

		[JsonProperty ("value")]
		public string Value { get; set; }

		[JsonProperty ("viewed")]
		public bool Viewed { get; set; }

		[JsonProperty ("translatable")]
		public bool Translatable { get; set; }

		[JsonProperty ("archivable")]
		public bool Archivable { get; set; }

		[JsonProperty ("deviceId")]
		public string DeviceId { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }


		[JsonProperty ("locationId")]
		public string LocationId { get; set; }

		[JsonProperty ("groupId")]
		public string GroupId { get; set; }

		[JsonProperty ("commandId")]
		public string CommandId { get; set; }

		[JsonProperty ("eventSource")]
		public string EventSource { get; set; }

		[JsonProperty ("deviceTypeId")]
		public string DeviceTypeId { get; set; }

		[JsonProperty ("data")]
		public string Data { get; set; }
	
		[JsonProperty ("smartAppId")]
		public string SmartAppId { get; set; }

		[JsonProperty ("smartAppVersionId")]
		public string SmartAppVersionId { get; set; }

		[JsonProperty ("installedSmartAppId")]
		public string InstalledSmartAppId { get; set; }

		[JsonProperty ("installedSmartAppParentId")]
		public string InstalledSmartAppParentId { get; set; }

	}

	public class DeviceInfo
	{

		[JsonProperty ("device")]
		public Device Device { get; set; }

		[JsonProperty ("events")]
		public List<object> Events { get; set; }
	}

	public class SmartThingsUpdate
	{
		[JsonProperty ("event")]
		public Event Event { get; set; }
	}

	public class ColorArgument
	{
		[JsonProperty ("green")]
		public int Green { get; set; }

		[JsonProperty ("alpha")]
		public int Alpha { get; set; }

		[JsonProperty ("hex")]
		public string Hex { get; set; }

		[JsonProperty ("level")]
		public int Level { get; set; }

		[JsonProperty ("saturation")]
		public double Saturation { get; set; }

		[JsonProperty ("blue")]
		public int Blue { get; set; }

		[JsonProperty ("red")]
		public int Red { get; set; }

		[JsonProperty ("hue")]
		public double Hue { get; set; }
	}

	public class CommandModel
	{

		[JsonProperty ("arguments")]
		public List<object> Arguments { get; set; } = new List<object> ();
	}

	public class EventsModel
	{
		[JsonProperty ("data")]
		public string Data { get; set; }
	}
}

