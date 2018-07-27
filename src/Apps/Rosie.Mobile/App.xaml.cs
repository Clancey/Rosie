using System;
using Xamarin.Forms;
using System.Threading;

namespace Rosie.Mobile
{
	public partial class App : Application
	{
		static Thread mainThread;
		public static App Main { get; private set;}
		public App ()
		{
			mainThread = Thread.CurrentThread;
			Main = this;
			InitializeComponent ();

			MainPage = 
				new TabbedPage()
				//.AddTab (new OverviewPage ())
				//.AddTab (new ScenesPage ())
				//.AddTab (new RoomsPage())
				.AddTab (new DevicesPage ())
			//	.AddTab (new SettingsPage());
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

		public static void RunOnMainThread (Action action)
		{
			if (mainThread == Thread.CurrentThread)
				action.Invoke ();
			else
				Xamarin.Forms.Device.BeginInvokeOnMainThread (action);
		}

		public static void ShowSpinner (string text)
		{

		}

		public static void DismissSpinner ()
		{

		}
	}
}

