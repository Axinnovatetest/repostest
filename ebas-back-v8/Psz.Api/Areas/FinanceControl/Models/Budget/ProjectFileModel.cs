using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.FinanceControl.Models.Budget
{
	public class ProjectFileModel
	{
		public int ProjectId { get; set; }
		public List<int> FileIds { get; set; }

		public List<Microsoft.AspNetCore.Http.IFormFile> AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }


		public Psz.Core.FinanceControl.Models.Budget.Project.ProjectFileModel ToBusinessModel()
		{
			return new Psz.Core.FinanceControl.Models.Budget.Project.ProjectFileModel
			{
				ProjectId = ProjectId,
				ProjectFileIds = FileIds,
				Files = AttachmentFile == null || AttachmentFile.Count <= 0
				? null
				: AttachmentFile.Select(x => new Core.FinanceControl.Models.Budget.Project.FileModel
				{
					FileName = x.FileName, // 
					CreationTime = DateTime.Now,
					FileData = getBytes(x),
					FileExtension = System.IO.Path.GetExtension(x.FileName) //AttachmentFileExtension,
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
