using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Threading.Tasks;

namespace Rosie
{
    public class DeviceDatabase
    {
        public SQLiteAsyncConnection DatabaseConnection { get; set; }

        public bool UseInMemoryCache { get; set; } = true;

        public DeviceDatabase() : this("devices.db")
        {

        }

        public DeviceDatabase(string databasePath)
        {
            //System.IO.File.Delete (databasePath);
            DatabaseConnection = new SQLiteAsyncConnection(databasePath, true);
            var s = DatabaseConnection.CreateTablesAsync<Device, DeviceGroup, DeviceKeyGrouping, DeviceState>().Result;
        }

        public static DeviceDatabase Shared { get; set; } = new DeviceDatabase();

        public Task<DeviceState> GetDeviceState(string deviceId, string key)
        {
            return DatabaseConnection.Table<DeviceState>().Where(x => x.DeviceId == deviceId && x.PropertyKey == key).FirstOrDefaultAsync();
        }

        public Task<DeviceState[]> GetDeviceState(string deviceId)
        {
            return DatabaseConnection.Table<DeviceState>().Where(x => x.DeviceId == deviceId).ToArrayAsync();
        }

        public Task InsertDeviceState(DeviceState state)
        {
            return DatabaseConnection.InsertOrReplaceAsync(state);
        }

        public async Task<Device> GetDevice(string id)
        {
            if (UseInMemoryCache)
            {
                if (devices == null)
                    await GetAllDevices();
                if (!devices.TryGetValue(id, out var device))
                    return null;
                return device;
            }
            return await DatabaseConnection.Table<Device>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Device> GetDevice(string service, string serviceDeviceId)
        {
            if(UseInMemoryCache){
                if (devices == null)
                    await GetAllDevices();
                var id = $"{service} - {serviceDeviceId}";
                if (!serviceDeviceIds.TryGetValue(id, out var deviceId))
                    return null;
                return devices[deviceId];
            }
            return await DatabaseConnection.Table<Device>().Where(x => x.ServiceDeviceId == serviceDeviceId && x.Service == service).FirstOrDefaultAsync();
        }

        Dictionary<string, Device> devices;
        Dictionary<string, string> serviceDeviceIds;
        public async Task<List<Device>> GetAllDevices()
        {
            if (UseInMemoryCache)
            {
                if (devices == null)
                {
                    var data = await DatabaseConnection.Table<Device>().ToListAsync();
                    devices = data.ToDictionary(x => x.Id, x => x);
                    serviceDeviceIds = data.ToDictionary(x => x.GetUniqueServiceId(), x => x.Id);
                }
                return devices.Values.ToList(); ;

            }
            return await DatabaseConnection.Table<Device>().ToListAsync();
        }

        public Task<List<Device>> GetEchoDevices()
        {
            throw new NotSupportedException();

            //return DatabaseConnection.Table<Device>().Where(x => x.Discoverable && x.DeviceType != DeviceType.Unknown).ToListAsync();
        }

        public async Task<bool> InsertDevice(Device device)
        {
            var s = await DatabaseConnection.InsertOrReplaceAsync(device);
            if (UseInMemoryCache)
            {
                if (devices != null)
                {
                    devices[device.Id] = device;
                    serviceDeviceIds[device.GetUniqueServiceId()] = device.Id;
                }
            }
            return s > 0;
        }

        public async Task<bool> DeleteDevice(Device device)
        {
            var s = await DatabaseConnection.DeleteAsync(device);
            if (devices?.ContainsKey(device.Id) ?? false)
                devices?.Remove(device.Id);
            var serviceKey = device.GetUniqueServiceId();
            if (serviceDeviceIds?.ContainsKey(serviceKey) ?? false)
                serviceDeviceIds?.Remove(serviceKey);
            return s > 0;
        }
    }
}

