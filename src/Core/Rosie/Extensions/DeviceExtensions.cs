namespace Rosie
{
	public static class DeviceExtensions
	{
		//TODO: We can do this using reflection
        public static bool Update (this Device device, Device newDevice)
		{
			bool didChange = false;
			if (device.Description != newDevice.Name) {
				device.Description = newDevice.Name;
				didChange = true;
			}
			if (device.Location != newDevice.Location) {
				device.Location = newDevice.Location;
				didChange = true;
			}
			if (device.Manufacturer != newDevice.Manufacturer) {
				device.Manufacturer = newDevice.Manufacturer;
				didChange = true;
			}
			if (device.ManufacturerId != newDevice.ManufacturerId) {
				device.ManufacturerId = newDevice.ManufacturerId;
				didChange = true;
			}
			if (device.Product != newDevice.Product) {
				device.Product = newDevice.Product;
				didChange = true;
			}

			if (device.ProductType != newDevice.ProductType) {
				device.ProductType = newDevice.ProductType;
				didChange = true;
			}

			if (device.ProductId != newDevice.ProductId) {
				device.ProductId = newDevice.ProductId;
				didChange = true;
			}

			if (device.Name != newDevice.Name) {
				device.Name = newDevice.Name;
				didChange = true;
			}

			if (device.Type != newDevice.Type) {
				device.Type = newDevice.Type;
				didChange = true;
			}
			if (device.DeviceType != newDevice.DeviceType) {
				device.DeviceType = newDevice.DeviceType;
				didChange = true;
			}
			return didChange || string.IsNullOrWhiteSpace (device.Id);
		}

        public static string GetUniqueServiceId(this Device device) => $"{device.Service} - {device.ServiceDeviceId}";
	}
}

