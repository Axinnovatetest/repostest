using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Files
{
	public class GetFailedFileModel
	{
		public int Write { get; set; }
		public int Read { get; set; }
		public DateTime? AddedOn { get; set; }
		public int? ErrorLevel { get; set; }
		public string Exception { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public string UserName { get; set; }
		public int? UserId { get; set; }
		public int? TotalCount { get; set; }
		public GetFailedFileModel(Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3 data, int read, int write)
		{
			if(data is not null)
			{
				AddedOn = (data.AddedOn is null) ? null : data.AddedOn;
				ErrorLevel = (data.ErrorLevel is null) ? null : data.ErrorLevel;
				Exception = (data.Exception is null) ? null : data.Exception;
				FileExtension = (data.FileExtension is null) ? null : data.FileExtension;
				FileName = (data.FileName is null) ? null : data.FileName;
				Id = data.Id;
				UserId = (data.UserId is null) ? 0 : data.UserId;
				TotalCount = data.TotalCount;
				Read = read;
				Write = write;
				UserName = Infrastructure.Data.Access.Tables.FNC.UserAccess.Get(data.Id)?.Username;
			}
		}
	}
}
