using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Api.Hubs
{
	public class OrderHubService: IHostedService, IDisposable
	{
		public static IHubContext<OrderHub> HubContext;

		public OrderHubService(IHubContext<OrderHub> hubContext)
		{
			HubContext = hubContext;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			//TODO: your start logic, some timers, singletons, etc
			return Task.CompletedTask;
		}
		public Task StopAsync(CancellationToken cancellationToken)
		{
			//TODO: your stop logic
			return Task.CompletedTask;
		}
		public void Dispose()
		{ }
	}
}
