using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class ProcessedFileEntity
	{
		public string FileName { get; set; }
		public int Id { get; set; }
		public int ProcessStatus { get; set; }
		public string ProcessStatusName { get; set; }
		public string ProcessStatusTrace { get; set; }
		public DateTime ProcessTime { get; set; }

		public ProcessedFileEntity() { }

		public ProcessedFileEntity(DataRow dataRow)
		{
			FileName = Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProcessStatus = Convert.ToInt32(dataRow["ProcessStatus"]);
			ProcessStatusName = Convert.ToString(dataRow["ProcessStatusName"]);
			ProcessStatusTrace = (dataRow["ProcessStatusTrace"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProcessStatusTrace"]);
			ProcessTime = Convert.ToDateTime(dataRow["ProcessTime"]);
		}
	}
}

