using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class FilesModel
	{
		public int Id { get; set; }
		public int? AngeboteNr { get; set; }
		public string FileName { get; set; }
		public int userId { get; set; }
		public DateTime? fileDate { get; set; }
		public byte[] FileData { get; set; }
		public string FileExtension { get; set; }
		public int FileId { get; set; }
		public FilesModel()
		{
			//fileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(fileId, DocumentData, DocumentExtension);
		}
		public Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity ToFile_OrderEntity(int UserID)
		{
			return new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity
			{
				AngeboteNr = AngeboteNr,
				FileName = FileName,
				FileExtension = FileExtension,
				CreationTime = DateTime.Now,
				CreationUserId = UserID,
				Id = -1,
				FileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImageWithoutDelete(FileId, FileData, FileExtension, null),
			};
		}
	}
}
