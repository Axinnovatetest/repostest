using System;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Budget.Files
{
	public class FilesModel
	{
		public int id { get; set; }
		public int? idOrder { get; set; }
		public int idOrderVersion { get; set; }
		public int fileId { get; set; }
		public string actionFile { get; set; }
		public int userId { get; set; }
		public DateTime? fileDate { get; set; }
		public byte[] DocumentData { get; set; }
		public string DocumentExtension { get; set; }


		public FilesModel()
		{
			//fileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(fileId, DocumentData, DocumentExtension);
		}
		public FilesModel(Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity budget_JointFile_OrderEntity)
		{
			if(budget_JointFile_OrderEntity == null)
				return;

			this.id = budget_JointFile_OrderEntity.Id_File;
			this.idOrder = budget_JointFile_OrderEntity.Id_Order;
			this.idOrderVersion = budget_JointFile_OrderEntity.Id_Order_Version;
			this.fileId = budget_JointFile_OrderEntity.FileId;
			this.actionFile = budget_JointFile_OrderEntity.Action_File;
			this.userId = budget_JointFile_OrderEntity.Id_User;
			this.fileDate = budget_JointFile_OrderEntity.File_date;
		}

		public async Task<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> ToFile_OrderEntity(int UserId)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity
			{
				Id_File = id,
				Id_Order = idOrder,
				Id_Order_Version = idOrderVersion,
				FileId = await Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(UserId, fileId, DocumentData, DocumentExtension, null),
				Action_File = actionFile,
				Id_User = userId,
				File_date = fileDate,
			};
		}


	}
}
