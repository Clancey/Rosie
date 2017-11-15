Rosie
================

Rosie is my First attempt at Home Automation.

Goals
====
* Secured
* Amazon Echo Support
* Z-Wave device Control
* Azure IoT Backend
* Run great on Raspberry Pi
* Plugable 
	* Additional Devices 
	* Logging Backends

Running
===== 
You can run the compiled Rosie.Server.exe on a Raspberry pie using "mono-service Rosie.Server.exe"

Debugging on the Raspberry pie via XS can be achieved by using (https://github.com/logicethos/SSHDebugger/)

Secrets.json
===
This is just a json file with Key Value Pairs. It's a place to put your API keys (src/Core/Rosie/Secrets.json)
To use the built in Api to get data out, set the `ApiKey` value

`{

	"ApiKey":"theKey",
    "AzureIoTUrl":"http://...",
}`

Raspberry Pi
===========
Make sure you have the latest Mono Installed

Make sure you create the isolated storage directory 

`sudo mkdir /usr/share/.isolated-storage`
`sudo chmod 757 -R /usr/share/.isolated-storage/`


ZWave (Node)
===========
It turns out ZWave in C#/Mono is harder than it should be. Long term I will bind to OpenZwave via CppSharp.

For now, there is a node app you can run that will proxy via openzwave-shared

Future Plans
===========
* Home Kit Support (Maybe via https://github.com/nfarina/homebridge )
* Windows IoT Support
	* Sockets port
	* Http listener port