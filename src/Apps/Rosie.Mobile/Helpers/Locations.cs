using System;
using System.IO;

namespace Rosie.Mobile
{
	internal static class Locations
	{
		static Locations()
		{
			try
			{
				Directory.CreateDirectory(Locations.ImageCache);
			}
			catch (Exception ex)
			{

			}

#if __IOS__
			try
			{
				Foundation.NSFileManager.SetSkipBackupAttribute(Locations.ImageCache, true);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
#endif
		}

#if __OSX__
		public static string BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "Documents/");
#elif __LINUX__
		public static string BaseDir = "";
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "");
#else
		public static string BaseDir = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString();
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "Documents/");
#endif

		public static readonly string LibDir = Path.Combine(BaseDir, "Library/");
		public static readonly string ImageCache = Path.Combine(LibDir, "Images");
		public static readonly string TmpDownloadDir = Path.Combine(BaseDir, "tmp");
	}
}