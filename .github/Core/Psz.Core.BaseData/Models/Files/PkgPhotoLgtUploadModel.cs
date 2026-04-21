using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Psz.Core.BaseData.Models.Files;

public class PkgPhotoLgtUploadModel
{
	public int ArticleId { get; set; }
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
public class UpdatePkgPhotoAttachmentModel
{
	public byte[] AttachmentFile { get; set; }
	public string AttachmentFileExtension { get; set; }
	public string FileName { get; set; }

}
public class DownloadPkgPhotoFileResponseModel
{
	public byte[] file { get; set; }
	public string extension { get; set; }
	public string MIMEtype { get; set; }
	public string FileName { get; set; }
}
public class DeletePkgPhotoFileModel
{
	public int ArticleId { get; set; }
	public string FileName { get; set; }
	public int Id { get; set; }
}
public class ResponsePkgPhotoFileModelWithId
{
	public int FileId { get; set; }
	public string FileName { get; set; }
	public int Id { get; set; }
	public DateTime? UploadedDate { get; set; }
	public ResponsePkgPhotoFileModelWithId(int fileId, string fileName, int id, DateTime? uploadedDate)
	{
		FileId = fileId;
		FileName = fileName;
		Id = id;
		UploadedDate = uploadedDate;
	}
}
