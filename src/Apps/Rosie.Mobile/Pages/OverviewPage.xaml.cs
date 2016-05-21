using System;
using System.Collections.Generic;
using D = Xamarin.Forms.Device;
using Xamarin.Forms;

namespace Rosie.Mobile
{
	public partial class OverviewPage : ContentPage
	{
		public OverviewPage ()
		{
			InitializeComponent ();
			Icon = D.OnPlatform (new FileImageSource { File = Images.GetCachedImagePath(Images.OverviewImageName,24) }, null, null);
		}
	}
}

