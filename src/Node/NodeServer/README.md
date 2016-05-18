Rosie Node Server
================

This is an Openzwave node API wrapper. It gives a nice rest api to interact with ZWave devices.

There is also a Socket.IO stream available to get updates in realtime!

Connecting to your Controller
============================
change zwavePort variable to your Controllers port. I have suggested values for different OS versions

Securing the API
===============
The rosie client is setup for a generic apiKey. To set the API key, add a Secrets.json file with the following:


	{
		"apiKey":"foo"
	}
