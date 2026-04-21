using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class __E_rechnung_ErrorsEntity
	{
		public string Error { get; set; }
		public DateTime? ErrorTime { get; set; }
		public int Id { get; set; }

		public __E_rechnung_ErrorsEntity() { }

		public __E_rechnung_ErrorsEntity(DataRow dataRow)
		{
			Error = (dataRow["Error"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Error"]);
			ErrorTime = (dataRow["ErrorTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ErrorTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
		}

		public __E_rechnung_ErrorsEntity ShallowClone()
		{
			return new __E_rechnung_ErrorsEntity
			{
				Error = Error,
				ErrorTime = ErrorTime,
				Id = Id
			};
		}
	}
}

