using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Support.Request
{
	public class FilesEntity
	{
		public string FileExtention { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public int RequestId { get; set; }
		public string ContentType { get; set; }
		public int FileId { get; set; }
		public FilesEntity() { }

		public FilesEntity(DataRow dataRow)
		{
			FileExtention = Convert.ToString(dataRow["FileExtention"]);
			FileName = Convert.ToString(dataRow["FileName"]);
			ContentType = Convert.ToString(dataRow["ContentType"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			RequestId = Convert.ToInt32(dataRow["RequestId"]);
			FileId = Convert.ToInt32(dataRow["FileId"]);
		}

		public FilesEntity ShallowClone()
		{
			return new FilesEntity
			{
				FileExtention = FileExtention,
				ContentType = ContentType,
				FileName = FileName,
				Id = Id,
				RequestId = RequestId
			};
		}
	}
}
