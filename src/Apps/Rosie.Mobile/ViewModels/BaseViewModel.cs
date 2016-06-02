using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rosie.Mobile
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void SetupEvents ()
		{
			Refresh ();
		}

		public virtual void TearDownEvents ()
		{

		}
		public virtual void Refresh ()
		{

		}
		protected bool NotifyPropertyChanged<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			return PropertyChanged.SetProperty (this, ref currentValue, newValue, propertyName);
		}
		protected void NotifyPropertyChanged (string propertyName)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}
	}
}

