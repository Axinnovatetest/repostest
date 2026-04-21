using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using Hangfire;
using Microsoft.AspNetCore.Http.Features;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Psz.Api.Extensions;

public static class IServiceCollectionExtension
{
	public static void AddOpenTelemetryERP(this IServiceCollection services)
	{
		services.AddOpenTelemetry()
			.ConfigureResource(resource =>
				resource.AddService("Psz.Api"))
			.WithTracing(tracerProviderBuilder =>
			{
				tracerProviderBuilder
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.AddSqlClientInstrumentation(options =>
				{
					options.SetDbStatementForText = true;
					options.Enrich = (activity, eventName, rawObject) =>
					{
						if(rawObject is SqlCommand sqlCommand)
						{
							activity.SetTag("db.operation", sqlCommand.CommandType.ToString());
							activity.SetTag("db.statement", sqlCommand.CommandText);
							activity.SetTag("custom.sql.identifier", "SqlQuery");
						}
					};
				});
				})
				.WithMetrics(metricsBuilder =>
				{
					metricsBuilder
					.AddAspNetCoreInstrumentation()
					.AddMeter("Microsoft.AspNetCore.Hosting")
					.AddMeter("Microsoft.AspNetCore.Server.Kestrel")
					.AddMeter("Microsoft.AspNetCore.Routing")
					.AddMeter("Microsoft.AspNetCore.Diagnostics")
					.AddMeter("Microsoft.AspNetCore.Http.Connections")
					.AddMeter("System.Net.Http")
					.AddMeter("System.IO.Pipelines")
					.AddMeter("System.Threading.Channels")
					.AddHttpClientInstrumentation()
					.AddRuntimeInstrumentation()
					.AddSqlClientInstrumentation()
					.AddRuntimeInstrumentation()
					.AddPrometheusExporter();
				});
	}
	public static IApplicationBuilder UseRouteTaggingForMetricsFiltering(this IApplicationBuilder app)
	{
		return app.Use(async (context, next) =>
		{
			var tagsFeature = context.Features.Get<IHttpMetricsTagsFeature>();
			if(tagsFeature != null)
			{
				try
				{
					var routePath = context.Request.Path.HasValue ? context.Request.Path.Value : string.Empty;
					var segments = routePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

					if(segments.Length == 4 && segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
					{
						tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.area", segments[1]));
						tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.controller", segments[2]));
						tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.action", segments[3]));
					}
					else
					{
						tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.area", "unknown"));
						tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.controller", "unknown"));
						tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.action", "unknown"));
					}
				} catch(Exception)
				{
					tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.area", "unknown"));
					tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.controller", "unknown"));
					tagsFeature.Tags.Add(new KeyValuePair<string, object?>("http.route.action", "unknown"));
				}
			}
			await next.Invoke();
		});
	}
}
