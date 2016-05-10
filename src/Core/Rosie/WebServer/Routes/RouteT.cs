//using System;
//using System.Collections.Specialized;
//using Newtonsoft.Json;

//namespace AmazonEchoBridge.Server
//{
//	public abstract class Route<T> : Route
//	{
//		public abstract T GetResponse (string method, NameValueCollection queryString, string data);

//		public override string GetResponseString (string method, NameValueCollection queryString, string data)
//		{
//			return JsonConvert.SerializeObject(GetResponse(method,queryString,data),new JsonSerializerSettings { 
//				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//			});
//		}

//	}
//}

