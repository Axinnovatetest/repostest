using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing
{



	public class PSZ_SendGrid_Email_Not_DeliveredEntity
	{
		public DateTime? AddedOn { get; set; }
		public string EBASUI { get; set; }
		public string EmailContent { get; set; }
		public string EmailFrom { get; set; }
		public int? EmailStatus { get; set; }
		public string EmailTo { get; set; }
		public string Exception { get; set; }
		public int? Failed { get; set; }
		public int Id { get; set; }
		public int? Iscc { get; set; }
		public string MessageId { get; set; }
		public string Subject { get; set; }
		public int? UserId { get; set; }
		public int? UserNotifiedByEmail { get; set; }
		public int? ViewedByTheUser { get; set; }

		public PSZ_SendGrid_Email_Not_DeliveredEntity() { }

		public PSZ_SendGrid_Email_Not_DeliveredEntity(DataRow dataRow)
		{
			AddedOn = (dataRow["AddedOn"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddedOn"]);
			EBASUI = (dataRow["EBASUI"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EBASUI"]);
			EmailContent = (dataRow["EmailContent"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailContent"]);
			EmailFrom = (dataRow["EmailFrom"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailFrom"]);
			EmailStatus = (dataRow["EmailStatus"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EmailStatus"]);
			EmailTo = (dataRow["EmailTo"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailTo"]);
			Exception = (dataRow["Exception"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Exception"]);
			Failed = (dataRow["Failed"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Failed"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Iscc = (dataRow["Iscc"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Iscc"]);
			MessageId = (dataRow["MessageId"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MessageId"]);
			Subject = (dataRow["Subject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Subject"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			UserNotifiedByEmail = (dataRow["UserNotifiedByEmail"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserNotifiedByEmail"]);
			ViewedByTheUser = (dataRow["ViewedByTheUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ViewedByTheUser"]);
		}

		public PSZ_SendGrid_Email_Not_DeliveredEntity ShallowClone()
		{
			return new PSZ_SendGrid_Email_Not_DeliveredEntity
			{
				AddedOn = AddedOn,
				EBASUI = EBASUI,
				EmailContent = EmailContent,
				EmailFrom = EmailFrom,
				EmailStatus = EmailStatus,
				EmailTo = EmailTo,
				Exception = Exception,
				Failed = Failed,
				Id = Id,
				Iscc = Iscc,
				MessageId = MessageId,
				Subject = Subject,
				UserId = UserId,
				UserNotifiedByEmail = UserNotifiedByEmail,
				ViewedByTheUser = ViewedByTheUser
			};
		}
	}
	public class PSZ_SendGrid_Email_Not_DeliveredMinimalEntity
	{
		public DateTime? AddedOn { get; set; }
		public string EmailContent { get; set; }
		public string EmailFrom { get; set; }
		public int? EmailStatus { get; set; }
		public string EmailTo { get; set; }
		public int? Failed { get; set; }
		public int? Iscc { get; set; }
		public string MessageId { get; set; }
		public string Subject { get; set; }
		public int? UserId { get; set; }
		public int? ViewedByTheUser { get; set; }

		public PSZ_SendGrid_Email_Not_DeliveredMinimalEntity() { }

		public PSZ_SendGrid_Email_Not_DeliveredMinimalEntity(DataRow dataRow)
		{
			AddedOn = (dataRow["AddedOn"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddedOn"]);
			EmailContent = (dataRow["EmailContent"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailContent"]);
			EmailStatus = (dataRow["EmailStatus"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EmailStatus"]);
			Failed = (dataRow["Failed"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Failed"]);
			EmailTo = (dataRow["EmailTo"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailTo"]);
			EmailFrom = (dataRow["EmailFrom"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailFrom"]);
			Iscc = (dataRow["Iscc"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Iscc"]);
			MessageId = (dataRow["MessageId"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MessageId"]);
			Subject = (dataRow["Subject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Subject"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			ViewedByTheUser = (dataRow["ViewedByTheUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ViewedByTheUser"]);
		}
	}

	public class PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2
	{
		public DateTime? AddedOn { get; set; }
		public string EmailContent { get; set; }
		public string EmailTo { get; set; }
		public string Subject { get; set; }
		public int? UserId { get; set; }
		public PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2() { }
		public PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2(DataRow dataRow)
		{
			AddedOn = (dataRow["AddedOn"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddedOn"]);
			EmailContent = (dataRow["EmailContent"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailContent"]);
			EmailTo = (dataRow["EmailTo"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailTo"]);
			Subject = (dataRow["Subject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Subject"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}
	}
	public class PSZ_SendGrid_Email_Not_DeliveredEntity2: PSZ_SendGrid_Email_Not_DeliveredEntity
	{
		public int? TotalCount { get; set; }
		public PSZ_SendGrid_Email_Not_DeliveredEntity2(DataRow dataRow) : base(dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
		}
	}
	public class MessagesIdEntity
	{
		public string MessageId { get; set; }
		public MessagesIdEntity(DataRow dataRow)
		{
			MessageId = (dataRow["MessageId"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MessageId"]);
		}
	}
	public class EmailToEntity
	{
		public string EmailTo { get; set; }
		public EmailToEntity(DataRow dataRow)
		{
			EmailTo = (dataRow["EmailTo"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailTo"]);
		}
	}
	public class MessagesIdCountEntity
	{
		public int? TotalCount { get; set; }
		public MessagesIdCountEntity(DataRow dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
		}
	}
}
