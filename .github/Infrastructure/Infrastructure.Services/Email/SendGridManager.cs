using Infrastructure.Services.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services.Email
{
	public class SendGridManager
	{
		public static List<PSZ_SendGrid_Email_Not_DeliveredMinimalModel> GetMinimalPSZ_SendGrid_Email_Not_DeliveredByUser(int UserId)
		{
			try
			{
				var fetchedmessages = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetMinimalUndeliveredEmailsNotViewedByUser(UserId);
				if(fetchedmessages is null || fetchedmessages.Count == 0)
					return null;

				var restoreturn = fetchedmessages.Select(x => new PSZ_SendGrid_Email_Not_DeliveredMinimalModel(x)).ToList();

				return restoreturn.OrderByDescending(x => x.AddedOn).ToList();

			} catch(Exception ex)
			{
				Logging.Logger.Log(ex);
				return null;
			}
		}
		public static List<FilteredUndeliveredEmails> GetPSZ_SendGrid_Email_Not_DeliveredByUser(int UserId, string UserName, string filter)
		{
			try
			{
				var datatoreturn = new List<FilteredUndeliveredEmails>();
				var fetchedmessagesId = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetUndeliveredEmailsNotViewedByUserMessagesId(UserId, filter);
				if(fetchedmessagesId is null || fetchedmessagesId.Count == 0)
					return null;

				var messagesId = fetchedmessagesId.Select(x => x.MessageId).ToList();
				var fetchedData = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetByMessagesIds(messagesId);
				var restoreturn = fetchedData?.Select(x => new PSZ_SendGrid_Email_Not_DeliveredModel(x)).ToList();

				var messagesID = restoreturn.Select(x => x.MessageId).Distinct().ToList();
				foreach(var item in messagesID)
				{
					var databyMessageId = restoreturn.Where(x => x.MessageId == item).ToList();
					datatoreturn.Add(new FilteredUndeliveredEmails(databyMessageId, UserName));
				}

				return datatoreturn.OrderByDescending(x => x.AddedOn).ToList();

			} catch(Exception ex)
			{
				Logging.Logger.Log(ex);
				return null;
			}
		}
		public static int SetEmailStatus(string MessageId, string EmailTo, string EmailFrom)
		{
			try
			{
				return Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.SetMessageStatus(MessageId, EmailTo, EmailFrom);

			} catch(Exception ex)
			{
				Logging.Logger.Log(ex);
				return -1;
			}
		}
		public static List<EmailToModel> GetPSZ_SendGrid_Email_To_FiltersByUser(int UserId)
		{
			try
			{
				var fetchedData = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetUndeliveredEmailsNotViewedByUserEmailTo(UserId);
				if(fetchedData is null || fetchedData.Count == 0)
					return null;

				var restoreturn = fetchedData?.Select(x => new EmailToModel(x)).ToList();

				return restoreturn;
			} catch(Exception ex)
			{
				Logging.Logger.Log(ex);
				return null;
			}
		}

	}
}
