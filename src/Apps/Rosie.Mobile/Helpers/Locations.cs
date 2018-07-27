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
				Directory.CreateDirectory(Locations.DocumentsDir);
			}
			catch (Exception ex)
			{

			}
		}
		static string _baseDir;
		public static string BaseDir
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_baseDir))
				{
					_baseDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					if (_baseDir.EndsWith("Documents"))
						_baseDir = Directory.GetParent(_baseDir).ToString();
				}
				return _baseDir;
			}
		}
		public static readonly string LibDir = Path.Combine(BaseDir, "Library/");
		public static readonly string ImageCache = Path.Combine(LibDir, "Images");
		public static readonly string TmpDownloadDir = Path.Combine(BaseDir, "tmp");
		public static readonly string DocumentsDir = Path.Combine(BaseDir, "Documents/");
	}
}