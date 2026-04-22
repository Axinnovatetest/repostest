namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class LogsEntity
	{
		public int Id { get; set; }
		public int LagerId { get; set; }
		public string LogDescription { get; set; }
		public int? LogsType { get; set; }
		public DateTime? LogTime { get; set; }
		public int? LogUserId { get; set; }
		public string LogUserName { get; set; }
		public int? ObjectId { get; set; }
		public string ObjectName { get; set; }

		public LogsEntity() { }

		public LogsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			LogDescription = (dataRow["LogDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogDescription"]);
			LogsType = (dataRow["LogsType"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LogsType"]);
			LogTime = (dataRow["LogTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LogTime"]);
			LogUserId = (dataRow["LogUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LogUserId"]);
			LogUserName = (dataRow["LogUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogUserName"]);
			ObjectId = (dataRow["ObjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ObjectId"]);
			ObjectName = (dataRow["ObjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ObjectName"]);
		}
	}
}
