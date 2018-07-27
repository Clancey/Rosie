using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rosie.Mobile
{
	public static class Locations
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



		public static string BaseDir = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString();


		public static readonly string LibDir = Path.Combine(BaseDir, "Library/");
		public static readonly string ImageCache = Path.Combine(LibDir, "Images");
		public static readonly string TmpDownloadDir = Path.Combine(BaseDir, "tmp");
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "Documents/");
	}
}