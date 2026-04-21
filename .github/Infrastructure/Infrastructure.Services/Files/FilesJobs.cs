using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Files
{
	public class FilesJobs
	{

		public static int GetFilesErrorsCount()
		{
			var result = 0;
			var count = Infrastructure.Data.Access.Tables.FNC.MinIO.PSZ_FileServer_RetryDataAccess.GetFilesErrorsCount();
			if(count is not null && count.Count() > 0)
				result = (int)count.FirstOrDefault().TotalCount;
			return result;
		}
	}
}
