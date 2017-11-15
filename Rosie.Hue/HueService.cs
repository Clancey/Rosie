using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rosie.Services;

namespace Rosie.Hue
{
	public class HueService : IRosieService
	{
		ILogger<HueService> _logger;

		public string Domain => "HUE";

		public string Name => "HUESERVICE";

		public string Description => "Integration with Phillips Hue";

		public Task Send()
		{
			throw new NotImplementedException();
		}

		public Task Received(object data)
		{
			throw new NotImplementedException();
		}

		public Task Start()
		{
			return Task.Run(() =>
			{
				System.Diagnostics.Debug.WriteLine("Start");
			});
		}

		public Task Stop()
		{
			return Task.Run(() =>
			{
				System.Diagnostics.Debug.WriteLine("Stop");
			});
		}

		public void Setup(IServiceProvider serviceProvicer)
		{
			_logger = serviceProvicer.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug).CreateLogger<HueService>();
			_logger.LogDebug("Setup HUE lights");
		}
	}
}
