namespace Psz.Core.Common
{
	public class Program
	{
		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }

		public static void Initiate(string filesPhysicalPath, string tempfilesPhysicalPath, Infrastructure.Services.FileServer.MinioParamtersModel miniomodel, Infrastructure.Services.Email.EmailParamtersModel emailParamters)
		{
			initFilesManager(filesPhysicalPath, tempfilesPhysicalPath, miniomodel);

			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit(); // - new Infrastructure.Services.Email.NetMail();
			EmailingService.InitiateEmailSender(emailParamters);
		}
		private static void initFilesManager(string filesPhysicalPath, string tempfilesPhysicalPath, Infrastructure.Services.FileServer.MinioParamtersModel miniomodel)
		{
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempfilesPhysicalPath, miniomodel.Minioaccesskey, miniomodel.Miniosecretkey, miniomodel.Minioendpoint, miniomodel.Miniobucket);
		}
	}
}
