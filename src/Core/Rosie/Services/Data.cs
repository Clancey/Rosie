using System;
using System.Collections.Generic;

namespace Rosie.Services
{
	public partial class Data
	{
		public string Description
		{
			get;
			set;
		}

		public IList<object> Fields
		{
			get;
			set;
		}
	}
}
