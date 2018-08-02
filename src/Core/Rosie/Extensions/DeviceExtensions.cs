namespace Rosie
{
	public static class DeviceExtensions
	{
		//TODO: We can do this using reflection
		public static bool Update (this Device device, Device nodeDevice)
		{
			bool didChange = false;
			if (device.Description != nodeDevice.Name) {
				device.Description = nodeDevice.Name;
				didChange = true;
			}
			if (device.Location != nodeDevice.Location) {
				device.Location = nodeDevice.Location;
				didChange = true;
			}
			if (device.Manufacturer != nodeDevice.Manufacturer) {
				device.Manufacturer = nodeDevice.Manufacturer;
				didChange = true;
			}
			if (device.ManufacturerId != nodeDevice.ManufacturerId) {
				device.ManufacturerId = nodeDevice.ManufacturerId;
				didChange = true;
			}
			if (device.Product != nodeDevice.Product) {
				device.Product = nodeDevice.Product;
				didChange = true;
			}

			if (device.ProductType != nodeDevice.ProductType) {
				device.ProductType = nodeDevice.ProductType;
				didChange = true;
			}

			if (device.ProductId != nodeDevice.ProductId) {
				device.ProductId = nodeDevice.ProductId;
				didChange = true;
			}

			if (device.Name != nodeDevice.Name) {
				device.Name = nodeDevice.Name;
				didChange = true;
			}

			if (device.Type != nodeDevice.Type) {
				device.Type = nodeDevice.Type;
				didChange = true;
			}
			if (device.DeviceType != nodeDevice.DeviceType) {
				device.DeviceType = nodeDevice.DeviceType;
				didChange = true;
			}
			return didChange || string.IsNullOrWhiteSpace (device.Id);
		}
	}
}

