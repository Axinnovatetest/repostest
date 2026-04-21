using Cronos;
using Infrastructure.Services.Files.Parsing;

namespace PSZLogService
{
	public class Worker: BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private HttpClient client;
		private IConfiguration _configuration;

		public Worker(ILogger<Worker> logger, IConfiguration configuration)
		{
			_logger = logger;
			this._configuration = configuration;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			client = new HttpClient();
			return base.StartAsync(cancellationToken);
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			client.Dispose();
			_logger.LogInformation("The service has been stopped...");
			return base.StopAsync(cancellationToken);
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var cronExpression = _configuration["LogParserCronJob"];
			var schedule = CronExpression.Parse(cronExpression, CronFormat.Standard);

			while(!stoppingToken.IsCancellationRequested)
			{
				var nextRun = schedule.GetNextOccurrence(DateTime.UtcNow, TimeZoneInfo.Utc);

				if(nextRun.HasValue)
				{
					var delay = nextRun.Value - DateTime.UtcNow;
					if(delay > TimeSpan.Zero)
					{
						await Task.Delay(delay, stoppingToken);
					}
				}

				// Execute the log parsing task
				LogParser logParser = new LogParser(
					_configuration["LogFolderPath"],
					_configuration.GetSection("ExcludeUserIds").Get<List<int>>(),
					_configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>()
				);

				List<KeyValuePair<string, int>> result = await logParser.SaveFeedbacksLogsToDBAsync(new List<int> { 0 });

				if(result.Count > 0)
				{
					foreach(var item in result)
					{
						_logger.LogInformation("LogService is up. Result {StatusCode}", item.Key + " " + item.Value);
					}
				}
				else
				{
					_logger.LogError("LogService is down. Result {StatusCode}", result);
				}
			}
		}
	}
}
