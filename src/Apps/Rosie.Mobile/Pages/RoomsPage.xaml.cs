using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Rosie.Mobile
{
	public partial class RoomsPage : BasePage
	{
		public RoomsPage ()
		{
			InitializeComponent ();
			BindingContext = new RoomsListViewModel ();
		}
	}
}

