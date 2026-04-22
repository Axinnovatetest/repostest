using System.Collections.Generic;

namespace Psz.Api.Areas.Settings.Models.Tools
{
	public class EmailNotificationModel
	{
		public bool? CCSender { get; set; }
		public string Title { get; set; }
		public string FromEmail { get; set; }
		public string ToEmail { get; set; }
		public string CCEmail { get; set; }
		public string Message { get; set; }
		public bool? FaSendChoice { get; set; } = false;
		public int? LanguageId { get; set; }
		public int? OrderId { get; set; }
		public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }

		//public Psz.Core.FinanceControl.Models.Budget.Order.PlaceModel ToBusinessModel()
		//{
		//    return new Infrastructure.Data.Entities.Tables.TLS.EmailEntity
		//    {
		//        ToEmail = SupplierEmail,
		//        EmailTitle = EmailTitle,
		//        EmailBody = EmailBody,
		//        OrderPlacementCCEmail = OrderPlacementCCEmail,
		//        CcIssuer = CcIssuer,
		//        IssuerEmail = IssuerEmail,
		//        Files = Files == null || Files.Count <= 0
		//        ? null
		//        : Files.Select(x => new Core.FinanceControl.Models.Budget.Files.FilesModel
		//        {
		//            actionFile = x.FileName, // I know it sucks, but I dont have time to change the DB columns name
		//            fileDate = DateTime.Now,
		//            DocumentData = getBytes(x),
		//            DocumentExtension = System.IO.Path.GetExtension(x.FileName) //AttachmentFileExtension,
		//        })?.ToList()
		//    };
		//}
		public static byte[] getBytes(Microsoft.AspNetCore.Http.IFormFile file)
		{
			if(file == null)
				return null;

			if(file.Length <= 0)
				return null;

			using(var ms = new System.IO.MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
