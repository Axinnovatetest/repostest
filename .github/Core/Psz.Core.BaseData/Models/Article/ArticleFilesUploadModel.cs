using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleFilesUploadModel
	{
		public string ArtiKleNummer { get; set; }
		public string KundenIndex { get; set; }
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
	public class UpdateArticleAttachmentModel
	{
		public byte[] AttachmentFile { get; set; }

		public string AttachmentFileExtension { get; set; }
		public string ArtiKleNummer { get; set; }
		public string KundenIndex { get; set; }
		public string FileName { get; set; }



	}
	public class GetArticleAttachmentModel
	{
		public string ArtiKleNummer { get; set; }
		public string KundenIndex { get; set; }
	}
	public class DownloadArticleFileModel
	{
		public string ArtiKleNummer { get; set; }
		public string KundenIndex { get; set; }
		public string FileName { get; set; }

	}
	public class DownloadArticleFileResponseModel
	{
		public byte[] file { get; set; }
		public string extention { get; set; }
		public string MIMEtype { get; set; }
		public string FileName { get; set; }
	}
	public class DeleteArticleFileModel
	{
		public string ArtiKleNummer { get; set; }
		public string KundenIndex { get; set; }
		public string FileName { get; set; }

	}
}
