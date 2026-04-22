using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Files
{
	public class FileHelper
	{
		public static string GetFileExtension(IFormFile file)
		{
			if(file == null)
			{
				throw new ArgumentNullException(nameof(file), "File cannot be null.");
			}
			string fileName = file.FileName;
			return System.IO.Path.GetExtension(fileName);
		}

		public static string ReplaceInvalidChars(string input)
		{
			char[] invalidChars = Path.GetInvalidFileNameChars();
			return new string(input.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());
		}

		public static string GenerateFolderNameForSupplierFiles(string SupplierNr ,string  SupplierName)
		{
			return SupplierNr +"_"+ SupplierName;
		}
		public static string GenerateFolderNameForSupplierFiles(string SupplierNr, string SupplierName,string filename)
		{
			return  SupplierNr + "_" + SupplierName + "\\" + filename;
		}

	}
}
