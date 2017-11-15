using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Rosie.Services;

namespace Rosie.Hue
{
	public class HueService : IRosieService, IDisposable
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

		public async Task Start()
		{

			try
			{
				IBridgeLocator locator = new HttpBridgeLocator();
				var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
				ILocalHueClient client = new LocalHueClient("ip");
				var appKey = await client.RegisterAsync(Domain, Name);
			}
			catch (Exception ex)
			{

			}


		}

		public Task Stop()
		{
			return Task.Run(() =>
			{
				_logger.LogInformation("Stop");
			});
		}

		public void Setup(IServiceProvider serviceProvicer)
		{
			_logger = serviceProvicer.GetService<ILoggerFactory>().AddConsole(LogLevel.Information).CreateLogger<HueService>();
			_logger.LogInformation("Setup HUE lights");
			_logger.LogInformation(Description);

		}

		public void Dispose()
		{
			
		}
	}
}
