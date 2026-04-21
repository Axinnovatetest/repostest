using System;

namespace Psz.Core.Helpers
{
	public class ImageFileHelper
	{
		public static int updateImage(int fileId, byte[] fileData, string fileExtension)
		{
			try
			{
				if(fileId > 0)
					Program.FilesManager.DeleteFile(fileId);

				var newFileId = fileId;
				if(fileData != null && fileData.Length > 0)
					newFileId = Program.FilesManager.NewFile(fileData, fileExtension, null);

				return newFileId;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return -1;
			}
		}
	}
}
