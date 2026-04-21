namespace Infrastructure.Data.Entities.Tables.Support.Feedback
{
	public class ERP_LogsEntity
	{
		public string EndpointMethod { get; set; }
		public string EndpointName { get; set; }
		public int Id { get; set; }
		public DateTime? LogCaptureDate { get; set; }
		public string LogLevel { get; set; }
		public string LogMessage { get; set; }
		public string Module { get; set; }
		public bool? Treated { get; set; } = false;
		public int? UserId { get; set; }
		public string UserName { get; set; }

		public ERP_LogsEntity() { }

		public ERP_LogsEntity(DataRow dataRow)
		{
			EndpointMethod = (dataRow["EndpointMethod"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EndpointMethod"]);
			EndpointName = (dataRow["EndpointName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EndpointName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LogCaptureDate = (dataRow["LogCaptureDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LogCaptureDate"]);
			LogLevel = (dataRow["LogLevel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogLevel"]);
			LogMessage = (dataRow["LogMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogMessage"]);
			Module = (dataRow["Module"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Module"]);
			Treated = (dataRow["Treated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Treated"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}

		public ERP_LogsEntity ShallowClone()
		{
			return new ERP_LogsEntity
			{
				EndpointMethod = EndpointMethod,
				EndpointName = EndpointName,
				Id = Id,
				LogCaptureDate = LogCaptureDate,
				LogLevel = LogLevel,
				LogMessage = LogMessage,
				Module = Module,
				Treated = Treated,
				UserId = UserId,
				UserName = UserName
			};
		}
	}
}
