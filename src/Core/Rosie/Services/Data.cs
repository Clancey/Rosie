using System;
using System.Collections.Generic;
using System.Dynamic;

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

		public ExpandoObject DataS
		{
			get;
			set;
		}
	}
}
