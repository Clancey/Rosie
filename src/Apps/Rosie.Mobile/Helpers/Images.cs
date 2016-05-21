using System;
using System.IO;
namespace Rosie.Mobile
{
	public static class Images
	{
		public static string GetCachedImagePath (string svg, double size)
		{
			var cachedImage = ImageHelper.GetCachedImagedName (svg, size);
			if (File.Exists (cachedImage))
				return cachedImage;
			ImageHelper.SaveImage (svg, size, cachedImage);
			return cachedImage;
		}
		public static string CurrentOverviewImageName => Settings.CurrentUsage == Usage.Low ? OverviewImageNameLow : Settings.CurrentUsage == Usage.Medium ? OverviewImageNameMedium : OverviewImageNameHigh;
		public const string OverviewImageNameLow = "overview-low.svg";
		public const string OverviewImageNameMedium = "overview-med.svg";
		public const string OverviewImageNameHigh = "overview-high.svg";
	}
}

