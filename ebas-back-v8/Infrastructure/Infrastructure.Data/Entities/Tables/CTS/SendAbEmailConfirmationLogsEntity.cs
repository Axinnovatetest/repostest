using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class SendAbEmailConfirmationLogsEntity
	{
		public int Id { get; set; }
		public string LastUserFullName { get; set; }
		public string LogDescription { get; set; }
		public string LogObject { get; set; }
		public int? LogObjectId { get; set; }
		public string SendGridId { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public SendAbEmailConfirmationLogsEntity() { }
		public SendAbEmailConfirmationLogsEntity(Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity _SendAbEmailConfirmationLogsEntity) {
			Id= _SendAbEmailConfirmationLogsEntity.Id;
			LastUserFullName = _SendAbEmailConfirmationLogsEntity.LastUserFullName;
			LogDescription = _SendAbEmailConfirmationLogsEntity.LogDescription;
			LogObject = _SendAbEmailConfirmationLogsEntity.LogObject;
			LogObjectId = _SendAbEmailConfirmationLogsEntity.LogObjectId;
			SendGridId = _SendAbEmailConfirmationLogsEntity.SendGridId;
			UserId = _SendAbEmailConfirmationLogsEntity.UserId;
			Username = _SendAbEmailConfirmationLogsEntity.Username;

		}
		public SendAbEmailConfirmationLogsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUserFullName = (dataRow["LastUserFullName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUserFullName"]);
			LogDescription = (dataRow["LogDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogDescription"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogObjectId = (dataRow["LogObjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LogObjectId"]);
			SendGridId = (dataRow["SendGridId"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SendGridId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public SendAbEmailConfirmationLogsEntity ShallowClone()
		{
			return new SendAbEmailConfirmationLogsEntity
			{
				Id = Id,
				LastUserFullName = LastUserFullName,
				LogDescription = LogDescription,
				LogObject = LogObject,
				LogObjectId = LogObjectId,
				SendGridId = SendGridId,
				UserId = UserId,
				Username = Username
			};
		}
	}
}
