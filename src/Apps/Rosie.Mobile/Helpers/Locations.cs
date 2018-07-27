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



#if __OSX__
		//[System.Runtime.InteropServices.DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
		//public static extern IntPtr NSHomeDirectory();

		//public static string ContainerDirectory {
		//	get {
		//		var val = ((Foundation.NSString)ObjCRuntime.Runtime.GetNSObject(NSHomeDirectory())).ToString ();

		//		if(val.Contains("Container"))
		//			return val;
		//		return Path.Combine (val, "Library/Application Support/gMusic");
		//	}
		//}
		public static string BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
		public static string BaseDir = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString();
#endif


		public static readonly string LibDir = Path.Combine(BaseDir, "Library/");
		public static readonly string ImageCache = Path.Combine(LibDir, "Images");
		public static readonly string TmpDownloadDir = Path.Combine(BaseDir, "tmp");
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "Documents/");
	}
}