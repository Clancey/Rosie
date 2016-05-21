using System;
using System.IO;
using Rosie.Resources;

namespace Rosie.Mobile
{
	public static class ImageHelper
	{
		public static void SaveImage (string svg, double size, string cachedPath)
		{
			var svgStream = new StreamReader (ResourceHelper.GetEmbeddedResourceStream (svg));
			var image = svgStream.LoadImageFromSvgStream (size);
			image.Compress (Android.Graphics.Bitmap.CompressFormat.Png, 100, File.Open (cachedPath, FileMode.Create));
		}

		public static string GetCachedImagedName (string svg, double size)
		{
			var name = Path.GetFileNameWithoutExtension (svg);
			var cachedName = $"{name}-{size}.png";
			var cachedImage = Path.Combine (Locations.ImageCache, cachedName);
			return cachedImage;
		}

	}
}

