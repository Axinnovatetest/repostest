using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Data.Access;

//using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Psz.Api.Extensions;
using Psz.Core.Apps.Support.Handlers.Feedback;
using Psz.Core.Apps.Support.Handlers.Logs;
using Psz.Core.Apps.Support.Interfaces;
using Psz.Core.BaseData.Handlers.Article;
using Psz.Core.BaseData.Handlers.ArticleLifeCycle;
using Psz.Core.BaseData.Handlers.ProjectManagment;
using Psz.Core.BaseData.Handlers.ROH;
using Psz.Core.BaseData.Interfaces;
using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.BaseData.Interfaces.ROH;
using Psz.Core.CapitalRequests.Handlers;
using Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles;
using Psz.Core.CapitalRequests.Services;
using Psz.Core.CRP.Handlers.Administration.AccessProfiles;
using Psz.Core.CRP.Handlers.Delfor;
using Psz.Core.CRP.Handlers.FA.DatesHistoryChanges;
using Psz.Core.CRP.Handlers.FA.Update;
using Psz.Core.CRP.Handlers.FAPlannung;
using Psz.Core.CRP.Handlers.Forecasts;
using Psz.Core.CRP.Handlers.Preview;
using Psz.Core.CRP.Handlers.Statistics;
using Psz.Core.CRP.Handlers.UBGPlannung;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CustomerService.Handlers.InsideSales;
using Psz.Core.CustomerService.Handlers.InsideSalesChecks;
using Psz.Core.CustomerService.Handlers.InsideSalesHistory;
using Psz.Core.CustomerService.Handlers.InsideSalesTotalDemandPlanning;
using Psz.Core.CustomerService.Handlers.InsideSalesWerkstermin;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.Logistics.Handlers.InventoryStockHandlers;
using Psz.Core.Logistics.Handlers.PlantBookings;
using Psz.Core.Logistics.Interfaces;
using Psz.Core.ManagementOverview.Production.Handlers;
using Psz.Core.ManagementOverview.Production.Interfaces;
using Psz.Core.MaterialManagement.Dashboard;
using Psz.Core.MaterialManagement.Interfaces;
using Psz.Core.MaterialManagement.Orders.Handlers;
using Psz.Core.Purchase.Handlers.StockWarnings;
using Psz.Core.Purchase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public static NotificationsSender Notifier { get; set; }
		readonly string PszCustomAllowOrigin = "PszCustomAllowOrigin";
		public const int MAX_REQUEST_LIMIT = 51428800;
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			var tempFolderPath = getTempFolderPath();
			Core.Program.Initiate(
				Configuration.GetConnectionString("ConnectionStringKWS"),
				Configuration.GetConnectionString("ConnectionStringKCZ"),
				Configuration.GetConnectionString("ConnectionStringKGZ"),
				Configuration.GetConnectionString("ConnectionStringKAL"),
				getConnectionStringBudget(),
				getConnectionStringMTM(),
				getConnectionString(),
				getEdiPlatformCnxEBAS(),
				getConnectionStringNlog(),
				Configuration["TokenSecret"],
				environment.ContentRootPath,
				Configuration["FilesPath"],
				tempFolderPath,
				Configuration["ReportsPath"],
				Configuration["NewOrderDiretory"],
				Configuration["ErrorOrderDiretory"],
				Configuration["ProcessedOrderDiretory"],
				Configuration["OrderResponseArchiveDiretory"],
				Configuration["ADPath"],
				Configuration["ADUsername"],
				Configuration["ADPassword"],
				Configuration["DeliveryNoteFilesPath"],
				Configuration["ADDWP"],
				// minio configuration
				Configuration.GetSection("Minio").Get<Infrastructure.Services.FileServer.MinioParamtersModel>(),
				Configuration.GetSection("EdiSettings").Get<Core.Common.Models.AppSettingsModel.AppSettingsEDIModel>(),
				Configuration.GetSection("CTS").Get<Core.Common.Models.AppSettingsModel.CTS>(),
				Configuration.GetSection("BSD").Get<Core.Common.Models.AppSettingsModel.BSD>(),
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				Convert.ToInt32(Configuration["ID_ESD_logo"]),
				Configuration.GetSection("LagersWithVersionning").Get<List<int>>(),
				Configuration["CustomerDownloadFilePath"],
				Configuration["DocumentNumberPath"]
				);

			Core.Common.Program.Initiate(Configuration["FilesPath"],
				tempFolderPath,
				Configuration.GetSection("Minio").Get<Infrastructure.Services.FileServer.MinioParamtersModel>(),
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>());
			Core.FinanceControl.Module.Initiate(Configuration["ProjectTemplatePath"],
				Configuration["ReportsPath"],
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				Configuration["ADPath"],
				Configuration["ADUsername"],
				Configuration["ADPassword"],
				Configuration.GetSection("fnc").Get<Core.Common.Models.AppSettingsModel.FNCAppSettings>()
				);
			Core.BaseData.Module.Initiate(
				Configuration["ReportsPath"],
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				Configuration["ADPath"],
				Configuration["ADUsername"],
				Configuration["ADPassword"],
				Configuration.GetSection("BSD").Get<Core.Common.Models.AppSettingsModel.BSD>(),
				Configuration.GetSection("CTS").Get<Core.Common.Models.AppSettingsModel.CTS>(),
				Configuration.GetSection("HauplagerCTS").Get<List<int>>(),
				Configuration.GetSection("LagersWithVersionning").Get<List<int>>(),
				Configuration.GetSection("ServerPath").Get<Core.Common.Models.AppSettingsModel.ServerPath>(),
				Configuration.GetSection("Impersonate").Get<Core.Common.Models.AppSettingsModel.Impersonate>()
				);
			Core.MaterialManagement.Module.Initiate(Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				Configuration.GetSection("MTM").Get<Psz.Core.MaterialManagement.Models.AppSettingsModel.MTM>(),
				Configuration["ReportsPath"],
				  Configuration.GetSection("BSD").Get<Psz.Core.MaterialManagement.Models.AppSettingsModel.BSD>(),
				  Configuration["FilesPath"], tempFolderPath,
				  Configuration.GetSection("Minio").Get<Infrastructure.Services.FileServer.MinioParamtersModel>()
				  );
			Core.CustomerService.Module.Initiate(
				Configuration["ReportsPath"],
			   	Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				 Configuration.GetSection("LagersWithVersionning").Get<List<int>>(),
				 Configuration.GetSection("HauplagerCTS").Get<List<int>>(),
				 Configuration.GetSection("CTS").Get<Core.Common.Models.AppSettingsModel.CTS>(),
				 Configuration.GetSection("BSD").Get<Core.Common.Models.AppSettingsModel.BSD>(),
				 Configuration["FilesPath"],
		 		tempFolderPath,
				Configuration.GetSection("Minio").Get<Infrastructure.Services.FileServer.MinioParamtersModel>(),
				Configuration.GetSection("EdiSettings").Get<Core.Common.Models.AppSettingsModel.AppSettingsEDIModel>(),
				Configuration.GetSection("MTM").Get<Core.Common.Models.AppSettingsModel.MTM>()
				);
			Core.Logistics.Module.Initiate(
				Configuration["ReportsPath"],
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>()
				, Configuration.GetSection("LGT").Get<Core.Common.Models.AppSettingsModel.LGT>(),
				Configuration["FilesPath"],
				tempFolderPath,
				Configuration.GetSection("Impersonate").Get<Core.Common.Models.AppSettingsModel.Impersonate>()
				, Configuration.GetSection("BSD").Get<Core.Common.Models.AppSettingsModel.BSD>());
			Core.CRP.Module.Initiate(Configuration["ReportsPath"],
			   	Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				 Configuration.GetSection("LagersWithVersionning").Get<List<int>>(),
				 Configuration.GetSection("HauplagerCTS").Get<List<int>>(),
				 Configuration.GetSection("CTS").Get<Core.Common.Models.AppSettingsModel.CTS>(),
				 Configuration.GetSection("BSD").Get<Core.Common.Models.AppSettingsModel.BSD>(),
				 Configuration["FilesPath"],
		 		tempFolderPath,
				Configuration.GetSection("Minio").Get<Infrastructure.Services.FileServer.MinioParamtersModel>(),
				Configuration.GetSection("EdiSettings").Get<Core.Common.Models.AppSettingsModel.AppSettingsEDIModel>(),
				Configuration.GetSection("MTM").Get<Core.Common.Models.AppSettingsModel.MTM>(),
					Convert.ToInt32(Configuration["ID_ESD_logo"]),
					Convert.ToInt32(Configuration["CRPAgentsRefreshThreshold"]));
			Core.CapitalRequests.Module.Initiate(Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>());
			Core.ManagementOverview.Module.Initiate(
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				Configuration.GetSection("BSD").Get<Core.Common.Models.AppSettingsModel.BSD>(), Configuration.GetSection("LGT").Get<Core.Common.Models.AppSettingsModel.LGT>(), Configuration.GetSection("CTS").Get<Core.Common.Models.AppSettingsModel.CTS>());

			var impersonateData = Configuration.GetSection("Impersonate").Get<Core.Common.Models.AppSettingsModel.Impersonate>();
			var ctsData = Configuration.GetSection("CTS").Get<Core.Common.Models.AppSettingsModel.CTS>();
			Infrastructure.Services.Module.Initiate(Configuration["FilesPath"], tempFolderPath,
				Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>(),
				ctsData.FAHorizons.H1LengthInDays,
				Convert.ToBoolean(Configuration["SaveLogsToSqlite"]),
				impersonateData.ImpersonateUsername,
				impersonateData.ImpersonatePassword, impersonateData.ImpersonateDomain);
			//
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(getTokenSecret()));

			services.AddMvc(options => options.EnableEndpointRouting = false)
				.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
			//.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddAuthentication(options =>
			{
				// Identity made Cookie authentication the default.
				// However, we want JWT Bearer Auth to be the default.
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = securityKey,

					ValidateIssuer = true,//true
					ValidIssuer = "Issuer", //_configuration["Jwt:Issuer"],
					ValidAudience = "Audience", //_configuration["Jwt:Issuer"],
					ValidateAudience = true,//true
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					RequireExpirationTime = false,
					//ClockSkew = TimeSpan.FromMinutes(5)
				};
				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
						return Task.CompletedTask;
					},
					OnTokenValidated = context =>
					{
						Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
						return Task.CompletedTask;
					},
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						if(!string.IsNullOrEmpty(accessToken))

							//    If the request is for our hub...
							if(!string.IsNullOrEmpty(accessToken)
								&& context.HttpContext.Request.Path.StartsWithSegments("/chathub"))
							{
								//  Read the token out of the query string
								context.Token = accessToken;
							}
						return Task.CompletedTask;
					},
				};
			});

			services.AddAuthorization(options =>
			{
			});

			services.AddHostedService<Hubs.OrderHubService>();
			Notifier = new NotificationsSender();

			services.AddCors(
				options =>
				{
					options.AddPolicy(PszCustomAllowOrigin,
						builder => builder.AllowAnyOrigin()
										   .AllowAnyMethod()
										   .AllowAnyHeader()
										   .SetPreflightMaxAge(TimeSpan.FromHours(2))
						);
				});
			services.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				});
			services.AddSignalR();

			services.Configure<IISOptions>(options =>
			{
				options.ForwardClientCertificate = false;
			});

			// If using Kestrel
			services.Configure<KestrelServerOptions>(options =>
			{
				options.Limits.MaxRequestBodySize = MAX_REQUEST_LIMIT;
			});
			services.Configure<FormOptions>(x =>
			{
				x.ValueLengthLimit = MAX_REQUEST_LIMIT;
				x.MultipartBodyLengthLimit = MAX_REQUEST_LIMIT;
				x.MultipartHeadersLengthLimit = MAX_REQUEST_LIMIT;
			});

			services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

			services.AddSwaggerGen(e =>
			{
				e.SwaggerDoc("v1", new OpenApiInfo() { Title = "API Docs", Version = $"v{Core.Program.Version}"/**/ });
				e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					//Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					//In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme."
				});
				e.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new  List<string>()
					}
				});
				e.EnableAnnotations();
				e.CustomSchemaIds(x => x.FullName);
				e.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}_{Array.IndexOf(["get", "post", "put", "patch", "delete", "options", "trace"], apiDesc.HttpMethod.ToLower())}");
			});

			// - Hangfire
			services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("ConnectionString"), new SqlServerStorageOptions
			{
				PrepareSchemaIfNecessary = false // Prevents schema creation
			}));
			services.AddHangfireServer();

			//get the logsDirectory
			string logsDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Logs");

			// - Access AppSetting in Controller
			services.AddSingleton<IConfiguration>(Configuration);
			services.AddSingleton<Psz.Core.BaseData.Interfaces.ISettings, Psz.Core.BaseData.Interfaces.Settings>();
			services.AddSingleton<Infrastructure.Services.Files.Parsing.ILogParser>(x => new Infrastructure.Services.Files.Parsing.LogParser(Configuration["LogFolderPath"], Configuration["LogDateTimeRange"], Configuration.GetSection("ExcludeUserIds").Get<List<int>>(), Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>()));
			services.AddScoped<IDelforService, DelforService>();
			services.AddScoped<ICrpStatisticsService, CrpStatisticsService>();
			services.AddScoped<ICrpAdministrationService, CrpAdministrationService>();
			services.AddScoped<ICrpForecastsService, CrpForecastsService>();
			services.AddScoped<ICrpFAPlannung, CrpFAPlannung>();
			services.AddScoped<IArticleService, ArticleService>();
			services.AddScoped<ICrpUBGPlannung, CrpUBGPlannung>();
			services.AddScoped<IInsideSalesChecks, InsideSalesChecks>();
			services.AddScoped<IInsideSalesChecksArchive, InsideSalesChecksArchive>();
			services.AddScoped<IInsideSalesWerksterminUpdates, InsideSalesWerksterminUpdates>();
			services.AddScoped<ICapitalRequestsService, CapitalRequestsService>();
			services.AddScoped<ICapitalRequestsAdminstrationService, CapitalRequestsAdminstrationService>();
			services.AddScoped<IRohArtikelnummer, RohArtikelnummer>();
			services.AddScoped<IInsideSalesTotalDemandPlanning, InsideSalesTotalDemandPlanning>();

			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IDashboardService, DashboardService>();

			services.AddScoped<IProductionService, ProductionService>();
			services.AddScoped<IArticleLifeCycleService, ArticleLifeCycleService>();
			services.AddScoped<IFeedbacksService, FeedbacksService>();
			services.AddScoped<ILogService, LogService>();

			services.AddScoped<IProjectManagmentService, ProjectManagmentService>();
			services.AddScoped<IPlantBookingService, PlantBookingService>();

			services.AddScoped<IPRSService, PRSService>();
			services.AddScoped<IInsideSalesOveview, InsideSalesOveview>();
			services.AddScoped<ICrpFaChangesHistoryService, CrpFaChangesHistoryService>();

			services.AddScoped<IPreviewService, PreviewService>();
			//services.AddScoped<IInventoryStockService, InventoryStockService>();

			services.AddScoped<IFAService, FAService>();
			//DbExecution.Logger = new Infrastructure.Services.Logging.DbLogger();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCustomRequestMiddleware();

			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseSwagger();

			app.UseSwaggerUI(e =>
			{
				e.SwaggerEndpoint("swagger/v1/swagger.json", "API V1");
				e.RoutePrefix = string.Empty;
			});

			app.UseRouteTaggingForMetricsFiltering();
			app.UseStaticFiles();
			string tempFolderPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "StaticFiles");
			if(!System.IO.Directory.Exists(tempFolderPath))
				System.IO.Directory.CreateDirectory(tempFolderPath);
			app.UseFileServer(new FileServerOptions
			{
				FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(tempFolderPath),
				RequestPath = "/StaticFiles",
				EnableDefaultFiles = true
			});
			app.UseCookiePolicy();
			app.UseRouting();
			app.UseAuthentication();
			app.UseCors(PszCustomAllowOrigin);

			app.UseEndpoints(routes =>
			{
				routes.MapHub<Hubs.ChatHub>("/chatHub");
				routes.MapHub<Hubs.OrderHub>("/orderHub");
				routes.MapHub<Hubs.LoopyHub>("/loopy");
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "area",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

				routes.MapRoute(
					name: "default",
					template: "api/{controller}/{action}/{id?}"
				);
			});

			// - Hangfire
			var hgSettings = Configuration.GetSection("Hangfire").Get<Core.Common.Models.AppSettingsModel.HangfireSettings>();
			var options = new DashboardOptions { };
			app.UseHangfireDashboard($"/{hgSettings?.Path ?? "hg"}", options);
			// - 2023-04-25 - prevent Jobs to immediately run on Deploy/Restart/ ... but wait until their scheduled time
			GlobalJobFilters.Filters.Add(new NoMissedRunsAttribute(), order: int.MaxValue);
			//if(env.IsProduction())
			//{
			try
			{
				//// - Hangfire - Set recurrent Jobs
				RecurringJob.AddOrUpdate(() => Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.applyLeasingFees(true), Configuration["OrderLeasingFrequency"]);
				RecurringJob.AddOrUpdate(() => Infrastructure.Services.Files.FilesManager.cleanTempFolder(getTempFolderPath()), Configuration["TempFolderCleaningFrequency"]);

				// - Cleaning yesterday's temp reports every day at 1:00 AM
				RecurringJob.AddOrUpdate(() => Psz.Core.BaseData.Helpers.CleanTempFiles.CleanArticleStatisticsReports(tempFolderPath, $"{DateTime.Today.AddDays(-1).ToString("yyyyMMdd*")}"), Configuration["BSDTempFolderCleaningFrequency"]);
				//RecurringJob.AddOrUpdate(() => Psz.Core.MaterialManagement.CRP.CronJobs.Configuration.Main(), Configuration["CRPConfigurationUpdateFrequency"]);
				RecurringJob.AddOrUpdate(() => Psz.Core.CustomerService.CronJobs.Configuration.NotifyExpiredNearlyRahmens(), Configuration["CTSRahmenExpiryNotificationFrequency"]);
				RecurringJob.AddOrUpdate(() => Psz.Core.FinanceControl.Helpers.SendGridEmailJobs.UpdateMessagesStatus(Configuration["Email:SendGridLicense"], 1000), "0/30 * * * *");

				// perf Logs Parsing 
				RecurringJob.AddOrUpdate(() => Infrastructure.Services.Files.Parsing.PerfLogParsingUtilities.SavePerfLogsToDb(Configuration["LogFolderPath"], Configuration.GetSection("ExcludeUserIds").Get<List<int>>()), Configuration["PerfLogsCronJob"]);

				// - 2023-05-31 - disable auto-RG Heidenreich
				//var rechnungCronFrequency = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_ConfigAccess.Get()?[0].CronJobFrequency;
				//RecurringJob.AddOrUpdate(() => Psz.Core.CustomerService.CronJobs.Configuration.CreateInvoices(), rechnungCronFrequency);

				// - background tasks
				//RecurringJob.AddOrUpdate(() => Infrastructure.Services.BackgroundWorker.CustomerService.WelcomeMadAsync(Configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>()), Configuration["BackgroundTasksFrequency"]);

				// - LGT
				RecurringJob.AddOrUpdate(() => Psz.Core.Logistics.CronJobs.Jobs.ArticleCustomsNumberChecksLastExec(), Configuration.GetSection("LGT").GetSection("ArticleCustomsMaxRenewalDaysFrequency").Value);
				// - MTD
				RecurringJob.AddOrUpdate(() => Psz.Core.BaseData.CronJobs.Jobs.UpdateEKForecast(), Configuration.GetSection("BSD").GetSection("EKForecastUpdateFrequency").Value);

				//RecurringJob.AddOrUpdate<Psz.Core.Support.CronJobs.Jobs>(
				//"SetApisCallsCountJob",
				//j => Psz.Core.Support.CronJobs.Jobs.SetApisCallsCount(),
				//Cron.Daily
				//);

				//RecurringJob.AddOrUpdate<Psz.Core.Support.CronJobs.Jobs>(
				//"SetLogsForToday",
				//j => Psz.Core.Support.CronJobs.Jobs.SetLogsForToday(),
				//Cron.Daily
				//);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			//}
		}

		#region > Helpers
		private string getConnectionString()
		{
			return Configuration.GetConnectionString("ConnectionString");
		}
		private string getConnectionStringNlog()
		{
			return Configuration.GetConnectionString("ConnectionStringNlog");
		}
		private string getConnectionStringBudget()
		{
			return Configuration.GetConnectionString("ConnectionStringFNC");
		}
		private string getConnectionStringMTM()
		{
			return Configuration.GetConnectionString("ConnectionStringMTM");
		}
		private string getEdiPlatformCnxEBAS()
		{
			return Configuration.GetConnectionString("ConnectionStringEdiPlatformCnxEBAS");
		}
		private string getTokenSecret()
		{
			return Configuration.GetValue<string>("TokenSecret");
		}

		public class NameUserIdProvider: IUserIdProvider
		{
			public string GetUserId(HubConnectionContext connection)
			{
				var value = connection.User?.Claims.FirstOrDefault(e => e.Type == "Name")?.Value;
				return value;
			}
		}
		private string getTempFolderPath()
		{
			string tempFolderPath = System.IO.Path.Combine(System.IO.Path.GetTempPath());
			if(!System.IO.Directory.Exists(tempFolderPath))
				System.IO.Directory.CreateDirectory(tempFolderPath);

			return tempFolderPath;
		}

		#endregion
	}
}