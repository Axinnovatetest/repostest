using Serilog;
using Serilog.Events;
namespace PSZLogService
{
	public class Program
	{
		public static IConfiguration configuration { get; set; }

		public static void Main(string[] args)
		{
			configuration = new ConfigurationBuilder()
		  .SetBasePath(AppContext.BaseDirectory) // Set the base path for appsettings.json
		  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		  .Build();
			string loggingOutputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location), "temp\\LogFile.txt");
			Infrastructure.Data.Access.Settings.SetConnectionString(configuration.GetConnectionString("ConnectionString"));
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.WriteTo.File(loggingOutputPath)
				.CreateLogger();
			try
			{
				Log.Information("Starting up the service");
				CreateHostBuilder(args).Build().Run();
				Log.Information("Service has been Completed");
				return;
			} catch(Exception ex)
			{
				Log.Fatal(ex, "There was a problem starting the service.");
				Infrastructure.Services.Logging.Logger.Log(ex.Message + "||" + ex.StackTrace);
				throw;
			} finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.UseWindowsService(options =>
				{
					options.ServiceName = "PSZ Log Parser Service ";
				})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddHostedService<Worker>();
			})
			.UseSerilog();
		}

	}
}