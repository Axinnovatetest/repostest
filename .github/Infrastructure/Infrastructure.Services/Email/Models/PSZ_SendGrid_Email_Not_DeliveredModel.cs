using Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services.Email.Models
{
	public class PSZ_SendGrid_Email_Not_DeliveredModel
	{
		public DateTime? AddedOn { get; set; }
		public string EmailContent { get; set; }
		public string EmailFrom { get; set; }
		public string link { get; set; }
		public int? EmailStatus { get; set; }
		public string EmailTo { get; set; }
		public string Exception { get; set; }
		public int? Failed { get; set; }
		public int Id { get; set; }
		public string MessageId { get; set; }
		public string Subject { get; set; }
		public int? UserId { get; set; }
		public int? Iscc { get; set; }
		public int? ViewedByTheUser { get; set; }
		public int? TotalCount { get; set; }

		public PSZ_SendGrid_Email_Not_DeliveredModel(Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2 data)
		{
			AddedOn = data.AddedOn;
			EmailContent = data.EmailContent;
			EmailFrom = data.EmailFrom;
			EmailStatus = data.EmailStatus;
			EmailTo = data.EmailTo;
			Exception = data.Exception;
			Failed = data.Failed;
			Id = data.Id;
			MessageId = data.MessageId;
			Subject = data.Subject;
			UserId = data.UserId;
			ViewedByTheUser = data.ViewedByTheUser;
			TotalCount = data.TotalCount;
			Iscc = data.Iscc;
			link = Infrastructure.Services.Helpers.SendGridHelper.GetLinkFromString(data.EmailContent);
		}
	}
	public class PSZ_SendGrid_Email_Not_DeliveredMinimalModel2
	{
		public DateTime? AddedOn { get; set; }
		public string link { get; set; }
		public string EmailTo { get; set; }
		public string Subject { get; set; }
		public PSZ_SendGrid_Email_Not_DeliveredMinimalModel2() { }
		public PSZ_SendGrid_Email_Not_DeliveredMinimalModel2(PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2 data)
		{
			AddedOn = data.AddedOn;
			link = Infrastructure.Services.Helpers.SendGridHelper.GetLinkFromString(data.EmailContent);
			EmailTo = data.EmailTo;
			Subject = data.Subject;
		}
	}
	public class FilteredUndeliveredEmails
	{
		public string UserName { get; set; }
		public string MessageId { get; set; }
		public int? userId { get; set; }
		public string EmailFrom { get; set; }
		public string EmailContent { get; set; }
		public string Subject { get; set; }
		public DateTime? AddedOn { get; set; }
		public List<PSZ_SendGrid_Email_Not_DeliveredModel> ccs { get; set; }
		public List<PSZ_SendGrid_Email_Not_DeliveredModel> emailto { get; set; }
		public FilteredUndeliveredEmails(List<PSZ_SendGrid_Email_Not_DeliveredModel> data, string userName)
		{
			MessageId = data.FirstOrDefault().MessageId;
			userId = data.FirstOrDefault().UserId;
			EmailFrom = data.FirstOrDefault().EmailFrom;
			EmailContent = data.FirstOrDefault().EmailContent;
			Subject = data.FirstOrDefault().Subject;
			AddedOn = data.FirstOrDefault().AddedOn;
			UserName = userName;
			ccs = data.Where(x => x.Iscc == 1).ToList();
			emailto = data.Where(x => x.Iscc != 1).ToList();

		}
	}
	public class EmailToModel
	{
		public string EmailTo { get; set; }
		public EmailToModel(EmailToEntity data)
		{
			EmailTo = data.EmailTo;
		}
	}
	public class SetEmailStatus
	{
		public string MessageId { get; set; }
		public string EmailTo { get; set; }
		public string EmailFrom { get; set; }
	}
	public class PSZ_SendGrid_Email_Not_DeliveredMinimalModel
	{
		public DateTime? AddedOn { get; set; }
		public string EmailFrom { get; set; }
		public string link { get; set; }
		public int? EmailStatus { get; set; }
		public string EmailTo { get; set; }
		public string MessageId { get; set; }
		public string Subject { get; set; }
		public int? UserId { get; set; }
		public int? Iscc { get; set; }
		public int? Failed { get; set; }
		public int? ViewedByTheUser { get; set; }

		public PSZ_SendGrid_Email_Not_DeliveredMinimalModel(PSZ_SendGrid_Email_Not_DeliveredMinimalEntity data)
		{
			Failed = data.Failed;
			AddedOn = data.AddedOn;
			EmailFrom = data.EmailFrom;
			EmailStatus = data.EmailStatus;
			EmailTo = data.EmailTo;
			MessageId = data.MessageId;
			Subject = data.Subject;
			UserId = data.UserId;
			ViewedByTheUser = data.ViewedByTheUser;
			Iscc = data.Iscc;
			link = Infrastructure.Services.Helpers.SendGridHelper.GetLinkFromString(data.EmailContent);
		}
	}
}
