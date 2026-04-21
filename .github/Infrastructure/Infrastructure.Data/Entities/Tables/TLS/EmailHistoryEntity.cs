using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.TLS
{
	public class EmailHistoryEntity
	{
		public string AttachmentIds { get; set; }
		public string CCEmails { get; set; }
		public string EmailMessage { get; set; }
		public string EmailTitle { get; set; }
		public int Id { get; set; }
		public bool? SenderCC { get; set; }
		public string SenderUserEmail { get; set; }
		public int? SenderUserId { get; set; }
		public string SenderUserName { get; set; }
		public DateTime? SendingTime { get; set; }
		public string ToEmail { get; set; }

		public EmailHistoryEntity() { }

		public EmailHistoryEntity(DataRow dataRow)
		{
			AttachmentIds = (dataRow["AttachmentIds"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AttachmentIds"]);
			CCEmails = (dataRow["CCEmails"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CCEmails"]);
			EmailMessage = (dataRow["EmailMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailMessage"]);
			EmailTitle = (dataRow["EmailTitle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailTitle"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			SenderCC = (dataRow["SenderCC"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SenderCC"]);
			SenderUserEmail = (dataRow["SenderUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SenderUserEmail"]);
			SenderUserId = (dataRow["SenderUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SenderUserId"]);
			SenderUserName = (dataRow["SenderUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SenderUserName"]);
			SendingTime = (dataRow["SendingTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SendingTime"]);
			ToEmail = (dataRow["ToEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ToEmail"]);
		}
	}
}

