using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class E_Rechnung_ConfigEntity
	{
		public int Id { get; set; }
		public string EmailBody { get; set; }
		public string EmailSubject { get; set; }
		public string CronJobFrequency { get; set; }

		public E_Rechnung_ConfigEntity()
		{

		}

		public E_Rechnung_ConfigEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			EmailBody = (dataRow["EmailBody"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailBody"]);
			EmailSubject = (dataRow["EmailSubject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailSubject"]);
			CronJobFrequency = (dataRow["CronJobFrequency"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CronJobFrequency"]);
		}
	}
}
