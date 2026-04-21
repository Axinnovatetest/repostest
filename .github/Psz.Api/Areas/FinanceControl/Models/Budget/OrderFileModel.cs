using System;
using System.Collections.Generic;
using System.Linq;
namespace Psz.Api.Areas.FinanceControl.Models.Budget
{
	public class OrderFileModel
	{
		public int Id_Order { get; set; }
		public List<int> OrderFileIds { get; set; }

		public List<Microsoft.AspNetCore.Http.IFormFile> AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }


		public Psz.Core.FinanceControl.Models.Budget.Order.OrderFileModel OrderToBussModel()
		{
			return new Psz.Core.FinanceControl.Models.Budget.Order.OrderFileModel
			{
				Id_Order = Id_Order,
				OrderFileIds = OrderFileIds,
				Files = AttachmentFile == null || AttachmentFile.Count <= 0
				? null
				: AttachmentFile.Select(x => new Core.FinanceControl.Models.Budget.Files.FilesModel
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
