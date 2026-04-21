using System.IO;

namespace Psz.Business.Helpers
{
	public class FilesHelper
	{
		public static string GetSafeFilename(string filename, string replaceCharacter = "_")
		{
			return string.Join(replaceCharacter, filename.Split(Path.GetInvalidFileNameChars()));
		}
	}
}
