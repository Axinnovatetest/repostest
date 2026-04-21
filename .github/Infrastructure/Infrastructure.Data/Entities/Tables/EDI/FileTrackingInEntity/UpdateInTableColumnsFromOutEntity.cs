namespace Infrastructure.Data.Entities.Tables.EDI.FileTrackingInEntity
{
	public class UpdateInTableColumnsFromOutEntity
	{
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }
		public string DocumentNumber { get; set; }
		public DateTime? EbasGenerateTime { get; set; }
		public string FileName { get; set; }
		public string MsgType { get; set; }
		public UpdateInTableColumnsFromOutEntity() { }

		public UpdateInTableColumnsFromOutEntity(DataRow dataRow)
		{
			CustomerName = dataRow["CustomerName"] == DBNull.Value ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNumber = dataRow["CustomerNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["CustomerNumber"]);
			DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]);
			EbasGenerateTime = dataRow["EbasGenerateTime"] == DBNull.Value ? null : Convert.ToDateTime(dataRow["EbasGenerateTime"]);
			FileName = dataRow["FileName"] == DBNull.Value ? "" : Convert.ToString(dataRow["FileName"]);
			MsgType = dataRow["MsgType"] == DBNull.Value ? "" : Convert.ToString(dataRow["MsgType"]);
		}
	}

}
