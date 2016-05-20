using System;
using Xamarin.Forms;
namespace Rosie.Mobile
{
	public static class FormsExtensions
	{
		public static TabbedPage AddTab (this TabbedPage tabPage, Page page, bool needsNavigation = true)
		{
			if (needsNavigation) {
				tabPage.Children.Add (new NavigationPage (page) {
					Title = page.Title,
					Icon = page.Icon,
				});
			} else {
				tabPage.Children.Add (page);
			}
			return tabPage;
		}
	}
}

