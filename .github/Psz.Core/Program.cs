using System.Collections.Generic;

namespace Psz.Core
{
	public class Program
	{
		public static string Version { get; set; } = "1.1.0-rc";
		public static string EdiOrderResponseArchiveDiretory { get; set; }
		public static Infrastructure.Services.Logging.Logger Logger { get; set; }
		public static Tools.Notifier Notifier { get; set; }
		public static Apps.EDI.Tools.FileWatcher PurchaseEdiFileWatcher { get; set; }
		public static Apps.EDI.Tools.FileWatcherDelfor EdiDelforFileWatcher { get; set; }
		public static Tools.ActiveDirectoryManager ActiveDirectoryManager { get; set; }

		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		public static Infrastructure.Services.Reporting.FastReport ReportingService { get; set; }
		//
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }

		public static string ADdwp { get; set; }
		public static Common.Models.AppSettingsModel.CTS CTS { get; set; }
		public static Common.Models.AppSettingsModel.BSD BSD { get; set; }
		public static Common.Models.AppSettingsModel.LGT LGT { get; set; }
		public static Common.Models.AppSettingsModel.AppSettingsEDIModel EDI { get; set; }
		public static int ID_ESD_logo { get; set; }
		public static List<int> LagersWithVersionning { get; set; }
		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		//fileInPaths
		public static string CustomerDownloadFilePath { get; set; }
		public static string DocumentNumberPath { get; set; }

		public static void Initiate(
			string databaseConnectionStringBudget,
			string databaseConnectionStringMTM,
			string databaseConnectionString,
			string databaseConnectionStringEDIPlatform,
			string securityToken,
			string applicationPhysicalPath,
			string filesPhysicalPath,
			string tempFilesPhysicalPath,
			string reportsPhysicalPath,
			string ediNewOrdersDirectory,
			string ediErrorOrderDirectory,
			string ediProcessedOrdersDirectory,
			string ediOrderResponseArchiveDiretory,
			string adPath,
			string adUsername,
			string adPassword,
			string dnPath,
			string adDwp,
			Infrastructure.Services.FileServer.MinioParamtersModel miniomodel,
			Common.Models.AppSettingsModel.AppSettingsEDIModel ediSettings,
			Common.Models.AppSettingsModel.CTS cts,
			Common.Models.AppSettingsModel.BSD bsd,
			Infrastructure.Services.Email.EmailParamtersModel emailParamters,
			int id_esd_logo, List<int> lagers,
			string customerDownloadFilePath,
			string documentNumberPath
			)
		{
			// -
			CTS = cts;
			BSD = bsd;
			EDI = ediSettings;

			initLogger();
			setSecretToken(securityToken);
			setDatabaseConnectionStringBudget(databaseConnectionStringBudget);
			setDatabaseConnectionStringMTM(databaseConnectionStringMTM);
			setDatabaseConnectionString(databaseConnectionString);
			setDatabaseConnectionStringEDI(databaseConnectionStringEDIPlatform);

			initNotifier();
			initFilesManager(filesPhysicalPath, tempFilesPhysicalPath, miniomodel.Minioaccesskey, miniomodel.Miniosecretkey, miniomodel.Minioendpoint, miniomodel.Miniobucket);
			initEdiFileWatcher(ediNewOrdersDirectory, ediErrorOrderDirectory, ediProcessedOrdersDirectory);
			initActiveDirectoryVerifier(adPath, adUsername, adPassword);


			EdiOrderResponseArchiveDiretory = ediOrderResponseArchiveDiretory;

			initReportingService(reportsPhysicalPath);
			ADdwp = adDwp;
			//
			LagersWithVersionning = lagers;
			//
			EmailingService = new Infrastructure.Services.Email.MailKit(); // - new Infrastructure.Services.Email.NetMail();
			EmailingService.InitiateEmailSender(emailParamters);
			ID_ESD_logo = id_esd_logo;


			// -
			EdiDelforFileWatcher = new Apps.EDI.Tools.FileWatcherDelfor(ediSettings?.Delfor?.NewDirectoryName, ediSettings?.Delfor?.ErrorDirectoryName, ediSettings?.Delfor?.ProcessedDirectoryName, ediSettings?.Delfor?.ArchiveDirectoryName);
			CustomerDownloadFilePath = customerDownloadFilePath;
			DocumentNumberPath = documentNumberPath;

		}

		private static void initReportingService(string reportTemplatePath)
		{
			ReportingService = new Infrastructure.Services.Reporting.FastReport(reportTemplatePath);
			Infrastructure.Services.Reporting.ReportGenerator.SetTemplatesPath(reportTemplatePath);
		}
		private static void setSecretToken(string securityToken)
		{
			Apps.Main.Handlers.Security.TokensManager.SetSecret(securityToken);
			//Psz.Core.Identity.Handlers.User.Tokens.SetSecret(securityToken);
			Psz.Core.Identity.Handlers.User.GetHandler.SetSecret(securityToken);
		}

		private static void initLogger()
		{
			Logger = new Infrastructure.Services.Logging.Logger();
		}
		private static void initNotifier()
		{
			Notifier = new Tools.Notifier();
		}
		private static void setDatabaseConnectionStringBudget(string connectionString)
		{
			Infrastructure.Data.Access.Settings.SetConnectionStringBudget(connectionString);
		}
		private static void setDatabaseConnectionString(string connectionString)
		{
			Infrastructure.Data.Access.Settings.SetConnectionString(connectionString);
		}
		private static void setDatabaseConnectionStringMTM(string connectionString)
		{
			Infrastructure.Data.Access.Settings.SetConnectionStringMTM(connectionString);
		}
		private static void setDatabaseConnectionStringEDI(string connectionString)
		{
			Infrastructure.Data.Access.Settings.SetEdiPlatformCnxEBAS(connectionString);
		}
		private static void initFilesManager(string filesPhysicalPath, string tempfilesPhysicalPath, string accesskey, string secretkey, string endpoint, string bucket)
		{
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempfilesPhysicalPath, accesskey, secretkey, endpoint, bucket);
		}
		private static void initEdiFileWatcher(
				string newOrdersDirectory,
				string errorOrderDirectory,
				string processedOrdersDirectory)
		{
			PurchaseEdiFileWatcher = new Core.Apps.EDI.Tools.FileWatcher(newOrdersDirectory,
				errorOrderDirectory,
				processedOrdersDirectory);
		}
		private static void initActiveDirectoryVerifier(string path,
			string username,
			string password)
		{
			ActiveDirectoryManager = new Tools.ActiveDirectoryManager(path, username, password);
		}
	}
}
