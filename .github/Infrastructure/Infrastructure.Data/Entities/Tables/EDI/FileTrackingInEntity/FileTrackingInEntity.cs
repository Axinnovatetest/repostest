namespace Infrastructure.Data.Entities.Tables.EDI.FileTrackingInEntity
{

	public class FileTrackingInEntity
	{
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }
		public string DocumentcNumber { get; set; }
		public DateTime? EbasImportTime { get; set; }
		public string FileName { get; set; }
		public int? FileSize { get; set; }
		public int Id { get; set; }
		public bool? IsArchived { get; set; }
		public bool? IsCreated { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public string MsgType { get; set; }
		public DateTime? ReceivedTime { get; set; }

		public FileTrackingInEntity() { }

		public FileTrackingInEntity(DataRow dataRow)
		{
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerNumber"]);
			DocumentcNumber = (dataRow["DocumentcNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentcNumber"]);
			EbasImportTime = (dataRow["EbasImportTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EbasImportTime"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			FileSize = (dataRow["FileSize"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FileSize"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = (dataRow["IsArchived"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsArchived"]);
			IsCreated = (dataRow["IsCreated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsCreated"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			MsgType = (dataRow["MsgType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MsgType"]);
			ReceivedTime = (dataRow["ReceivedTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ReceivedTime"]);
		}

		public FileTrackingInEntity ShallowClone()
		{
			return new FileTrackingInEntity
			{
				CustomerName = CustomerName,
				CustomerNumber = CustomerNumber,
				DocumentcNumber = DocumentcNumber,
				EbasImportTime = EbasImportTime,
				FileName = FileName,
				FileSize = FileSize,
				Id = Id,
				IsArchived = IsArchived,
				IsCreated = IsCreated,
				LastUpdateTime = LastUpdateTime,
				MsgType = MsgType,
				ReceivedTime = ReceivedTime
			};
		}
	}

}
