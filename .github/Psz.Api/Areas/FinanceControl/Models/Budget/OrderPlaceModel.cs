using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.FinanceControl.Models.Budget
{
	public class OrderPlaceModel
	{
		public int OrderId { get; set; }
		public string SupplierEmail { get; set; }
		public string EmailTitle { get; set; }
		public string EmailBody { get; set; }
		public string OrderPlacementCCEmail { get; set; }
		public bool? CcIssuer { get; set; }
		public string IssuerEmail { get; set; }

		public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }

		public Psz.Core.FinanceControl.Models.Budget.Order.PlaceModel ToBusinessModel()
		{
			return new Psz.Core.FinanceControl.Models.Budget.Order.PlaceModel
			{
				OrderId = OrderId,
				SupplierEmail = SupplierEmail,
				EmailTitle = EmailTitle,
				EmailBody = EmailBody,
				OrderPlacementCCEmail = OrderPlacementCCEmail,
				CcIssuer = CcIssuer,
				IssuerEmail = IssuerEmail,
				Files = Files == null || Files.Count <= 0
				? null
				: Files.Select(x => new Core.FinanceControl.Models.Budget.Files.FilesModel
				{
					actionFile = x.FileName, // I know it sucks, but I dont have time to change the DB columns name
					fileDate = DateTime.Now,
					DocumentData = getBytes(x),
					DocumentExtension = System.IO.Path.GetExtension(x.FileName) //AttachmentFileExtension,
				})?.ToList()
			};
		}
		internal static byte[] getBytes(Microsoft.AspNetCore.Http.IFormFile file)
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
