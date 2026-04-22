using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class __BSD_TempFilesEntity
	{
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public string FileExtension { get; set; }
		public int? FileId { get; set; }
		public int Id { get; set; }
		public int? LastModifiedBy { get; set; }
		public DateTime? LastModifiedDate { get; set; }
		public string TempFileName { get; set; }

		public __BSD_TempFilesEntity() { }

		public __BSD_TempFilesEntity(DataRow dataRow)
		{
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUserName = (dataRow["CreationUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUserName"]);
			FileExtension = (dataRow["FileExtension"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileExtension"]);
			FileId = (dataRow["FileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FileId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastModifiedBy = (dataRow["LastModifiedBy"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastModifiedBy"]);
			LastModifiedDate = (dataRow["LastModifiedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastModifiedDate"]);
			TempFileName = (dataRow["TempFileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TempFileName"]);
		}

		public __BSD_TempFilesEntity ShallowClone()
		{
			return new __BSD_TempFilesEntity
			{
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CreationUserName = CreationUserName,
				FileExtension = FileExtension,
				FileId = FileId,
				Id = Id,
				LastModifiedBy = LastModifiedBy,
				LastModifiedDate = LastModifiedDate,
				TempFileName = TempFileName
			};
		}
	}

}
