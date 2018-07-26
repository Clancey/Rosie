using Ooui;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Rosie.Frontend
{
	public class OverviewPage : ContentPage, IBasePage
	{
		public OverviewPage()
		{
			var label = new Xamarin.Forms.Label {  Text = "Hello from Rosie Forms"};
			Content = label;
		}
	}
}
