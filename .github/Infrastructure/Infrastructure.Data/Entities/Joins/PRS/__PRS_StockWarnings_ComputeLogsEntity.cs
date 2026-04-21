using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class __PRS_StockWarnings_ComputeLogsEntity
	{
		public DateTime? Date { get; set; }
		public int Id { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }
		public __PRS_StockWarnings_ComputeLogsEntity() { }

		public __PRS_StockWarnings_ComputeLogsEntity(DataRow dataRow, bool withName = false)
		{
			Date = (dataRow["Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			if(withName)
				Username = (dataRow["Username"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Username"]);
		}

		public __PRS_StockWarnings_ComputeLogsEntity ShallowClone()
		{
			return new __PRS_StockWarnings_ComputeLogsEntity
			{
				Date = Date,
				Id = Id,
				UserId = UserId
			};
		}
	}
}

