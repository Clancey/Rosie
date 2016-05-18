using System;
namespace Rosie
{
	public class Enum
	{
		public static T Parse<T> (string value, bool ignoreCase = true)
		{
			return (T)System.Enum.Parse (typeof (T), value,ignoreCase);
		}
	}
}

