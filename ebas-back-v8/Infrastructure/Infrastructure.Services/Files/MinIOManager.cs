using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services.Files;

namespace Infrastructure.Services.Files
{
	public class MinIOManager
	{
		public static List<GetFailedFileModel> GetAllFailedFiles()
		{
			try
			{
				var fetchedData = Infrastructure.Data.Access.Tables.FNC.MinIO.PSZ_FileServer_RetryDataAccess.GetAllFailedFile();
				if(fetchedData is null || fetchedData.Count == 0)
					return null;

				var write = fetchedData.Where(x => x.ErrorLevel == -1).ToList().Count;
				var read = fetchedData.Where(x => x.ErrorLevel == -2).ToList().Count;
				var restoreturn = fetchedData.Select(x => new GetFailedFileModel(x, read, write)).ToList();

				return restoreturn;
			} catch(Exception ex)
			{
				Logging.Logger.Log(ex);
				return null;
			}
		}
	}
}
