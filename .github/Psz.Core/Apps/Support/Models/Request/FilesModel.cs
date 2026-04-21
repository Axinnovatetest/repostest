using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Psz.Core.Apps.Support.Models.Request
{
	public class FilesModel
	{
		public IFormFileCollection Files { get; set; }
		public int RequestId { get; set; }
		public List<int> FileIds { get; set; }
	}

	public class FileDownloadModel
	{
		public byte[] FileContent { get; set; }
		public string Name { get; set; }
		public string ContentType { get; set; }
	}
}

