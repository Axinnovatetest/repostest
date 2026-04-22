using System;

namespace Psz.Core.CustomerService.Helpers
{
	public class ImageFileHelper
	{
		internal static int updateImage(int fileId, byte[] fileData, string fileExtension)
		{
			try
			{
				if(fileId > 0)
					Module.FilesManager.DeleteFile(fileId);

				var newFileId = fileId;
				if(fileData != null && fileData.Length > 0)
					newFileId = Module.FilesManager.NewFile(fileData, fileExtension, null);

				return newFileId;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return -1;
			}
		}
	}
}
