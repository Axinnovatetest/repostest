using System;

namespace Infrastructure.Services.BackgroundWorker
{
	using FluentScheduler;
	public class FluentSchedulerService
	{
		public void Initiate()
		{
			//JobManager.Initialize(MainRegistry);
			JobManager.AddJob(() => Console.WriteLine("Late job!"), (s) => s.ToRunEvery(5).Seconds());
		}
		public class MainRegistry: Registry
		{
			public MainRegistry()
			{
				// Run at an interval in days
				Schedule<OrderLeasing>().ToRunNow().AndEvery(2).Days();

				// Run now, and then every first monday of any other month
				//Schedule(() => new OrderLeasing()).ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(2, 0);
			}

		}

		// - 
		public class OrderLeasing: IJob
		{
			// -
			public OrderLeasing() { }

			void IJob.Execute()
			{
				throw new NotImplementedException();
			}
		}

	}
}
