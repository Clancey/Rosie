using System;
using Rosie.Resources;
using System.IO;

namespace Rosie.Mobile
{
	public static class ImageHelper
	{
		public static void SaveImage (string svg, double size, string cachedPath)
		{
			var svgStream = new StreamReader(ResourceHelper.GetEmbeddedResourceStream (svg));
			var image = svgStream.LoadImageFromSvgStream (size);
			image.AsPNG ().Save (cachedPath, false);
		}

		public static string GetCachedImagedName (string svg, double size)
		{
			var name = Path.GetFileNameWithoutExtension (svg);
			string scaleModifier = NGraphicsExtensions.Scale > 1 ? $"@{NGraphicsExtensions.Scale}x" :"";
			var cachedName = $"{name}-{size}{scaleModifier}.png";
			var cachedImage = Path.Combine (Locations.ImageCache, cachedName);
			return cachedImage;
		}


	}
}

