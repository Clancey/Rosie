using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Rosie.Services
{
	public class BaseEvenArgs : EventArgs
	{
		public object Data
		{
			get;
			set;
		}
	}

	public interface IRosieService : IDeviceService
	{
		string Domain { get; }
		string Name { get; }
		string Description { get; }
		Task Start();
		Task Stop();
		Task Send();
		Task Received(object data);
	}
}
