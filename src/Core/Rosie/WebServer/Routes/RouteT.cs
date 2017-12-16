using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rosie.Server
{
	public abstract class Route<T> : Route
	{
		public abstract Task<T> GetResponse<T2>(HttpMethod method, T2 request, NameValueCollection queryString, string data);

		public override async Task<string> GetResponseString<T2> (HttpMethod method, T2 request, NameValueCollection queryString, string data)
		{
			var item = await GetResponse<T2> (method, request, queryString, data);
			return await Task.Run (() => JsonConvert.SerializeObject (item, new JsonSerializerSettings {
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			}));
		}
	}
}

