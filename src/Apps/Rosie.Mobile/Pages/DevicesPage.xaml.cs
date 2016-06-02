using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Rosie.Mobile
{
	public partial class DevicesPage : BasePage
	{
		public DevicesPage ()
		{
			InitializeComponent ();
			BindingContext = new DeviceListViewModel ();
		}
	}
}

