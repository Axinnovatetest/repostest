using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class KundenExtensionEntity
	{
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public int Id { get; set; }
		public int ImageId { get; set; }
		public bool? IsArchived { get; set; }
		public int Nr { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }

		public KundenExtensionEntity() { }

		public KundenExtensionEntity(DataRow dataRow)
		{
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImageId = Convert.ToInt32(dataRow["ImageId"]);
			IsArchived = (dataRow["IsArchived"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["IsArchived"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
		}
	}
}

