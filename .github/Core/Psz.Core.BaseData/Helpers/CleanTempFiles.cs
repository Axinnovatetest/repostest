using System;

namespace Psz.Core.BaseData.Helpers
{
	public static class CleanTempFiles
	{
		public static void CleanArticleStatisticsReports(string folderPath, string dirtyFilesPrefixPattern)
		{
			try
			{
				var dirtyFileNames = System.IO.Directory.GetFiles(folderPath, dirtyFilesPrefixPattern);
				if(dirtyFileNames != null && dirtyFileNames.Length > 0)
				{
					foreach(var fileName in dirtyFileNames)
					{
						try
						{
							System.IO.File.Delete(System.IO.Path.Combine(folderPath, fileName));
						} catch(Exception e)
						{
							Infrastructure.Services.Logging.Logger.Log(e);
						}
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}
	}
}
