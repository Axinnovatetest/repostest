using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class CTSBlanketFilesEntity
	{
		public int? AngeboteNr { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public int? FileId { get; set; }

		public CTSBlanketFilesEntity() { }

		public CTSBlanketFilesEntity(DataRow dataRow)
		{
			AngeboteNr = (dataRow["AngeboteNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AngeboteNr"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			FileExtension = (dataRow["FileExtension"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileExtension"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			FileId = (dataRow["FileName"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FileId"]);
		}
	}
}

