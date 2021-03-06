﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rosie
{
	public class SetDeviceArgs : EventArgs
	{
		public Device Device { get; set; }

		public DeviceUpdate Request { get; set; }

		public bool Success { get; set; }
	}

	internal class DeviceManager : IDeviceManager
	{
		public DeviceManager(IDeviceLogger defaultDeviceLogger)
		{
			RegisterHandler<WebRequestHandler>();
			if (defaultDeviceLogger != null)
			{
				RegisterHandler(defaultDeviceLogger);
			}
		}

		public async Task<bool> AddDevice (Device device)
		{
			if (device == null) {
				return false;
			};

			Device foundDev = null;
			if (!string.IsNullOrWhiteSpace (device.Id)) {
				foundDev = await GetDevice (device.Id);
			}

			if (foundDev == null) {
				var oldDevices = await GetAllDevices ();
				foundDev = oldDevices.FirstOrDefault (x => x.Service == device.Service && x.ServiceDeviceId == device.Id);
			}

			if (foundDev != null) {
				//our device was updated
				foundDev.Update (device);
				return true;
			}

			if (string.IsNullOrWhiteSpace(device.Id)) {
				device.Id = Guid.NewGuid().ToString();
			}

			if (DeviceLogHandlers.Any()) {
				var tasks = new Task[DeviceLogHandlers.Count];
				for (int i = 0; i < DeviceLogHandlers.Count; i++) {
					try {
						tasks[i] = DeviceLogHandlers[i].AddDevice (device);
					} catch (Exception ex) {
						Console.WriteLine (ex);
					}
				}
				await Task.WhenAll (tasks);
			}
			return await DeviceDatabase.Shared.InsertDevice(device);
		}

		public Task<List<Device>> GetAllDevices()
		{
			return DeviceDatabase.Shared.GetAllDevices();
		}

		public Task<Device> GetDevice(string id)
		{
			return DeviceDatabase.Shared.GetDevice(id);
		}


        public Task<Device> GetDevice(string service, string serviceDeviceId)
        {
            return DeviceDatabase.Shared.GetDevice(service,serviceDeviceId);
        }

		public delegate bool SetDeviceStateHandler(object sender, SetDeviceArgs args);

		public event SetDeviceStateHandler SetDeviceEvent;

		public async Task<bool> SetDeviceState(Device device, DeviceUpdate state)
		{
			var events = SetDeviceEvent?.GetInvocationList().ToList();
			//Let the last subscription get first dibs
			events?.Reverse();
			var handled = false;
			var args = new SetDeviceArgs
			{
				Device = device,
				Request = state,
			};
			if (events != null)
				foreach (var e in events)
				{
					try
					{
						handled = await Task.Run(() => (e as SetDeviceStateHandler)?.Invoke(this, args) ?? false);
						if (handled)
							return args.Success;
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);
					}
				}


			var handlerList = GetHandlerList(device.Service ?? WebRequestHandler.Identifier);
			foreach (var handler in handlerList)
			{
				handled = await handler.HandleRequest(device, state);
				if (handled)
					return true;
			}

			return false;
		}

		public async Task UpdateCurrentState(DeviceState state)
		{
			if (DeviceLogHandlers.Any())
			{
				Task.WhenAll(DeviceLogHandlers.Select(handler => Task.Run(async () =>
				{
					try
					{
						await handler.DeviceUpdate(state);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);
					}
				})).ToList());
				//await Task.WhenAll(

			}
			await DeviceDatabase.Shared.InsertDeviceState(state);

			if (state.PropertyKey == DevicePropertyKey.Level)
			{
				var switchState = new DeviceState
				{
					Value = state.IntValue > 0,
					PropertyKey = DevicePropertyKey.SwitchState,
					DeviceId = state.DeviceId,
					DataType = DataTypes.Bool,
				};
				await UpdateCurrentState(switchState);
			}
		}

		Dictionary<string, List<IDeviceService>> handlers = new Dictionary<string, List<IDeviceService>>();

		public void RegisterHandler<T>() where T : IDeviceService
		{
			var handler = (T)Activator.CreateInstance(typeof(T));
			RegisterHandler(handler);
		}

		public void RegisterHandler(IDeviceService handler)
		{
			var handlerList = GetHandlerList(handler.ServiceIdentifier);
			handlerList.Add(handler);
		}

		List<IDeviceService> GetHandlerList(string service)
		{
			List<IDeviceService> handlerList;
			if (!handlers.TryGetValue(service, out handlerList))
				handlers[service] = handlerList = new List<IDeviceService>();
			return handlerList;
		}

		List<IDeviceLogger> DeviceLogHandlers = new List<IDeviceLogger>();
		public void RegisterDeviceLogHandler<T>() where T : IDeviceLogger
		{
			var handler = (T)Activator.CreateInstance(typeof(T));
			RegisterHandler(handler);
		}

		public void RegisterHandler(IDeviceLogger handler)
		{
			DeviceLogHandlers.Add(handler);
		}
    }
}

