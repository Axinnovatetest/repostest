using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Core.BaseData.Models.Files;

public class SupplierFilesUploadModel
{
	public int ModuleId { get; set; }
	public int Module { get; set; }
	public IFormFileCollection AttachmentFile { get; set; }
	internal static byte[] getBytes(IFormFile file)
	{
		if(file == null)
			return null;

		if(file.Length <= 0)
			return null;

		using(var ms = new MemoryStream())
		{
			file.CopyTo(ms);
			return ms.ToArray();
		}
	}
	internal static string getFileName(IFormFile file)
	{
		if(file == null)
			return null;

		if(file.Length <= 0)
			return null;
		return file.FileName;
	}
}
public class UpdateSupplierAttachmentModel
{
	public byte[] AttachmentFile { get; set; }

	public string AttachmentFileExtension { get; set; }
	public string SupplierName { get; set; }
	public int SupplierNr { get; set; }
	public string FileName { get; set; }



}
public class GetSupplierAttachmentModel
{
	public int ModuleId { get; set; }
	public int Module { get; set; }
}
public class DownloadSupplierFileModel
{
	public int ModuleId { get; set; }
	public int Module { get; set; }

}
public class DownloadSupplierFileResponseModel
{
	public byte[] file { get; set; }
	public string extension { get; set; }
	public string MIMEtype { get; set; }
	public string FileName { get; set; }
}
public class DeleteSupplierFileModel
{
	public string SupplierName { get; set; }
	public int SupplierNr { get; set; }
	public string FileName { get; set; }
	public int Id { get; set; }
}
