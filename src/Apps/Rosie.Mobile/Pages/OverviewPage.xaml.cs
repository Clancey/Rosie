using System;
using System.Collections.Generic;
using D = Xamarin.Forms.Device;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace Rosie.Mobile
{
	public partial class OverviewPage : ContentPage
	{
		public OverviewPage ()
		{
			InitializeComponent ();
			this.BindingContext = new OverviewViewModel ();
			Icon = D.OnPlatform (new FileImageSource { File = Images.GetCachedImagePath(Images.CurrentOverviewImageName,24) }, null, null);
			ChangeIcon ();
		}

		async void ChangeIcon ()
		{
			//This is just here for testing purposes. Going to tie into a live setting instead
			var max = Enum.GetValues (typeof (Usage)).Cast<int> ().Max ();
			int current = (int)Settings.CurrentUsage;              
			while (true) {
				await Task.Delay (1000);
				current++;
				if (current > max)
					current = 0;
				Settings.CurrentUsage = (Usage)current;
				var nav = this.Parent as NavigationPage;
				var icon = D.OnPlatform (new FileImageSource { File = Images.GetCachedImagePath (Images.CurrentOverviewImageName, 24) }, null, null);
				if (nav != null)
					nav.Icon = icon;
				else
					Icon = icon;
			}
		}
	}
}

