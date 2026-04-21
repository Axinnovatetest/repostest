using System;
using System.Data.SqlClient;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class OrderErrorEntity
	{
		public string Error { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public bool Validated { get; set; }
		public int CustomerId { get; set; }
		public int? ValidationUserId { get; set; }
		public DateTime? ValidationTime { get; set; }

		public string CustomerNumber { get; set; }
		public DateTime CreationTime { get; set; }

		public OrderErrorEntity() { }
		public OrderErrorEntity(SqlDataReader dataReader)
		{
			Error = Convert.ToString(dataReader["Error"]);
			FileName = Convert.ToString(dataReader["FileName"]);
			Id = Convert.ToInt32(dataReader["Id"]);
			CustomerName = Convert.ToString(dataReader["ClientName"]);
			Validated = Convert.ToBoolean(dataReader["Validated"]);
			CustomerId = Convert.ToInt32(dataReader["ClientId"]);
			ValidationUserId = (dataReader["ValidationUserId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ValidationUserId"]);
			ValidationTime = (dataReader["ValidationTime"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataReader["ValidationTime"]);

			CustomerNumber = Convert.ToString(dataReader["CustomerNumber"]);
			CreationTime = Convert.ToDateTime(dataReader["CreationTime"]);
		}
	}
}
