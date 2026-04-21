using Infrastructure.Data.Access;
using Infrastructure.Services.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Psz.Api;
using Psz.Api.Extensions;
using Psz.Api.Middlewares;
using static Psz.Core.Common.Models.AppSettingsModel;
var builder = WebApplication.CreateBuilder(args);
// Re-add only what you want
builder.Configuration
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Host.UseWindowsService();
builder.Services.AddWindowsService();

// - activate memory cache for caching data in memory (e.g. for caching user permissions, etc.)
builder.Services.AddMemoryCache();

try
{
	var _dbOptions = builder.Configuration
		.GetSection("Telemetry:Database");
		.Get<TelemetrySettings.BaseSettings>();

	DbExecution.Enabled = _dbOptions?.Enabled ?? false;
	DbExecution.SlowThresholdMs = _dbOptions?.SlowThresholdMs ?? 3000;
	DbExecution.Logger = new DbLogger();
} catch(System.Exception)
{
}

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);
builder.Services.AddOpenTelemetryERP();
var app = builder.Build();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

try
{

	var _epOptions = builder.Configuration
		.GetSection("Telemetry:Endpoint")
		.Get<TelemetrySettings.BaseSettings>();
	// - Register Telemetry middleware
	if (_epOptions?.Enabled == true)
		app.UseMiddleware<RequestTelemetryMiddleware>(_epOptions?.SlowThresholdMs ?? 3000);
} catch(System.Exception)
{
}
startup.Configure(app, builder.Environment);
app.Run();
