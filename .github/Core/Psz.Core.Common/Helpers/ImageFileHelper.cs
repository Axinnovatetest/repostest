using Infrastructure.Services.Utils;
using System.Threading.Tasks;

namespace Psz.Core.Common.Helpers
{
	public class ImageFileHelper
	{
		private string TempFolder { get; set; }
		public static int updateImage(int? fileId, byte[] fileData, string fileExtension, int? fileModule = null, string fileName = null)
		{
			if(fileId.HasValue && fileId > 0)
				Program.FilesManager.DeleteFile(fileId.Value);

			var newFileId = fileId.HasValue ? fileId.Value : -1;
			if(fileData != null && fileData.Length > 10) // 10 is totally ARBITRARY; 
				newFileId = Program.FilesManager.NewFile(fileData, fileExtension, fileModule, fileName);

			return newFileId;
		}
		public static async Task<int> updateImageAsync(int UserId, int? fileId, byte[] fileData, string fileExtension, int? fileModule = null, string fileName = null)
		{
			if(fileId.HasValue && fileId > 0)
				Program.FilesManager.DeleteFile(fileId.Value);

			var newFileId = fileId.HasValue ? fileId.Value : -1;
			if(fileData != null && fileData.Length > 10) // 10 is totally ARBITRARY; 
				newFileId = await Program.FilesManager.NewFileWithMinio(UserId, fileData, fileExtension, fileModule, fileName);

			return newFileId;
		}
		public static int updateImageWithoutDelete(int? fileId, byte[] fileData, string fileExtension, int? fileModule = null)
		{
			var newFileId = fileId.HasValue ? fileId.Value : -1;
			if(fileData != null && fileData.Length > 10) // 10 is totally ARBITRARY; 
				newFileId = Program.FilesManager.NewFile(fileData, fileExtension, fileModule);

			return newFileId;
		}
		//public static int updateImageWithoutDelete(int? fileId, byte[] fileData, string fileExtension, int? fileModule = null)
		//{
		//    var newFileId = fileId.HasValue ? fileId.Value : -1;
		//    if (fileData != null && fileData.Length > 10) // 10 is totally ARBITRARY; 
		//        newFileId = Program.FilesManager.NewFile(fileData, fileExtension, fileModule);

		//    return newFileId;
		//}

		public static int NewTempFile(byte[] fileData, string fileExtension)
		{
			int newFileId = -1;
			if(fileData != null && fileData.Length > 0) // 10 is totally ARBITRARY; 
				newFileId = Program.FilesManager.NewTempFile(fileData, fileExtension);

			return newFileId;
		}
		public static int NewTempFileMinio(byte[] fileData, string fileExtension)
		{
			int newFileId = -1;
			if(fileData != null && fileData.Length > 0) // 10 is totally ARBITRARY; 
				newFileId = Program.FilesManager.NewTempFileMinio(fileData, fileExtension).Result;

			return newFileId;
		}

		public static int PersistTempFile(int tempFileId, int? module)
		{
			if(tempFileId <= 0)
				return tempFileId;

			return Program.FilesManager.PersistTempFile(tempFileId, module);
		}
		public static async Task<int> PersistTempFileasync(int UserId, int tempFileId, int? module)
		{
			if(tempFileId <= 0)
				return tempFileId;

			return await Program.FilesManager.PersistTempFileasync(UserId, tempFileId, module);
		}

		public static Infrastructure.Services.Files.Models.FileContent getFileData(int fileId)
		{
			return Program.FilesManager.GetFile(fileId);
		}

		public static void DeleteFile(int id)
		{
			var fileEntity = Infrastructure.Data.Access.Tables.FileAccess.Get(id);
			if(fileEntity != null)
			{
				Program.FilesManager.DeleteFile(id);
				Infrastructure.Data.Access.Tables.FileAccess.Delete(id);
			}
		}
		public static void DeleteFile(int id, TransactionsManager botransaction)
		{
			var fileEntity = Infrastructure.Data.Access.Tables.FileAccess.GetWithTransaction(id, botransaction.connection, botransaction.transaction);
			if(fileEntity != null)
			{
				Program.FilesManager.DeleteFile(id, botransaction);
				Infrastructure.Data.Access.Tables.FileAccess.DeleteWithTransaction(id, botransaction.connection, botransaction.transaction);
			}
		}
		public static async Task<int> AddNewFileAsync(int UserId, byte[] fileData, string fileExtension, int? fileModule = null, string fileName = null)
		{
			var newFileId = -1;
			if(fileData != null && fileData.Length > 10)
				newFileId = await Program.FilesManager.NewFileWithMinio(UserId, fileData, fileExtension, fileModule, fileName);

			return newFileId;
		}
	}
}
