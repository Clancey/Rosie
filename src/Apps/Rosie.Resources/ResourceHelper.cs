﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rosie.Resources
{
	public static class ResourceHelper
	{
		public static Stream GetEmbeddedResourceStream (string resourceFileName)
		{
			var assembly = typeof (ResourceHelper).GetTypeInfo ().Assembly;
			return GetEmbeddedResourceStream (assembly, resourceFileName);
		}

		public static Stream GetEmbeddedResourceStream (Assembly assembly, string resourceFileName)
		{
			var resourceNames = assembly.GetManifestResourceNames ();

			var resourcePaths = resourceNames
				.Where (x => x.EndsWith (resourceFileName, StringComparison.CurrentCultureIgnoreCase))
				.ToArray ();
			
			if (!resourcePaths.Any ()) {
				throw new Exception (string.Format ("Resource ending with {0} not found.", resourceFileName));
			}

			return assembly.GetManifestResourceStream (resourcePaths.FirstOrDefault ());
		}


		public static string GetEmbeddedResourceString (string resourceFileName)
		{
			var assembly = typeof (ResourceHelper).GetTypeInfo ().Assembly;
			return GetEmbeddedResourceString (assembly, resourceFileName);
		}

		public static string GetEmbeddedResourceString (Assembly assembly, string resourceFileName)
		{
			var stream = GetEmbeddedResourceStream (assembly, resourceFileName);

			using (var streamReader = new StreamReader (stream)) {
				return streamReader.ReadToEnd ();
			}
		}
	}
}

