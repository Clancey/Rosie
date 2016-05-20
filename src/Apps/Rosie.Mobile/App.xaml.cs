using Xamarin.Forms;

namespace Rosie.Mobile
{
	public partial class App : Application
	{
		public static App Main { get; private set;}
		public App ()
		{
			Main = this;
			InitializeComponent ();

			MainPage = new TabbedPage()
				.AddTab (new OverviewPage ())
				.AddTab (new ScenesPage ())
				.AddTab (new RoomsPage())
				.AddTab (new SettingsPage());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

