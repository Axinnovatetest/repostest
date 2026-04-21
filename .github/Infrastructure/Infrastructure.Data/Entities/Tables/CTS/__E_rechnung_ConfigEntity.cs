using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class __E_rechnung_ConfigEntity
	{
		public string CronJobFrequency { get; set; }
		public string EmailBody { get; set; }
		public string EmailSubject { get; set; }
		public int Id { get; set; }

		public __E_rechnung_ConfigEntity() { }

		public __E_rechnung_ConfigEntity(DataRow dataRow)
		{
			CronJobFrequency = (dataRow["CronJobFrequency"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CronJobFrequency"]);
			EmailBody = (dataRow["EmailBody"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailBody"]);
			EmailSubject = (dataRow["EmailSubject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailSubject"]);
			Id = Convert.ToInt32(dataRow["Id"]);
		}

		public __E_rechnung_ConfigEntity ShallowClone()
		{
			return new __E_rechnung_ConfigEntity
			{
				CronJobFrequency = CronJobFrequency,
				EmailBody = EmailBody,
				EmailSubject = EmailSubject,
				Id = Id
			};
		}
	}
}

