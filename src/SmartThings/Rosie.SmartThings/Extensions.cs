using System;


namespace Rosie.SmartThings
{
	public static class Extensions 
	{
		public static string CreateApiUrl (this UserLocation location, string path)
		{
			return new Uri (location.Shard.Api.Uri, path).AbsoluteUri;
		}
	}
}

