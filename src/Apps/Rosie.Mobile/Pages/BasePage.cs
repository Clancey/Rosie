using System;

using Xamarin.Forms;

namespace Rosie.Mobile
{
	public class BasePage : ContentPage
	{
		public BasePage ()
		{
			
		}
		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			(BindingContext as BaseViewModel)?.SetupEvents ();
		}
		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			(BindingContext as BaseViewModel)?.TearDownEvents ();
		}
	}
}


