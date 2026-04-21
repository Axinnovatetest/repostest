using Infrastructure.Services.Email.Models;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;
using PdfSharp.Charting;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Helpers
{
	public class SendGridEmailJobs
	{
		private static List<NotifyUserByEmailModel> GetEmailContent(string msg_id)
		{
			var fetchetData = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetUndeliveredEmailsNotNotifiedUser(msg_id);
			if(fetchetData is null || fetchetData.Count == 0)
				return null;
			List<NotifyUserByEmailModel> emails = fetchetData.Select(x => new Infrastructure.Services.Email.Models.NotifyUserByEmailModel(x)).ToList();
			return emails;
		}
		//Infrastructure.Services.Email
		public static async Task NotifyUserForFailedEmail(List<Infrastructure.Services.Email.Models.messagesModel> messages)
		{
			try
			{
				if(messages is null || messages.Count == 0)
					return;

				var messagesId = messages.Select(x => x.msg_id).ToList();
				messagesId = messagesId.Distinct().ToList();

				foreach(var item in messagesId)
				{
					var messageInfo = GetEmailContent(item);

					if(messageInfo is null || messageInfo.Count == 0)
						return;

					var user = Infrastructure.Data.Access.Tables.FNC.UserAccess.Get(messageInfo.FirstOrDefault().UserId);
					var emails = messageInfo.Select(x => x.EmailTo).ToList();
					var emailContent = Infrastructure.Services.Email.Helpers.GetNotificationEmailContent(user.Name, messageInfo.FirstOrDefault().Subject, emails, (DateTime)messageInfo.FirstOrDefault().AddedOn, messageInfo.FirstOrDefault().link);

					await Module.EmailingService.SendEmailAsync(messageInfo.FirstOrDefault().Subject, emailContent, new List<string> { user.Email, Module.EmailingService.GetAdminEmail() },
							saveHistory: true, senderEmail: Module.EmailingService.GetMainAdress(), senderName: "", senderId: user.Id, senderCC: false, attachmentIds: null);

					Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.UpdateMessageNotificationStatus(item, messageInfo.Select(x => x.EmailTo).ToList());
				}
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
			}
		}
		private static async Task<List<messagesModel>> RetrieveEmailDeliveries(string apiKey, int limit)
		{
			try
			{
				var client = new SendGridClient(apiKey);
				var queryBody = @$"status=""not_delivered"" AND  last_event_time BETWEEN TIMESTAMP ""{DateTime.Now.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ssZ")}"" AND TIMESTAMP ""{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")}"" ";
				//var queryBody = @$"status=""not_delivered"" AND  last_event_time BETWEEN TIMESTAMP ""{DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddTHH:mm:ssZ")}"" AND TIMESTAMP ""{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")}"" ";
				var query = Infrastructure.Services.Helpers.SendGridHelper.BuildSendGridQuery(queryBody, limit);

				var response = await client.RequestAsync(
					 method: SendGridClient.Method.GET,
					 urlPath: "messages",
					 queryParams: query
				 );
				var body = await response.Body.ReadAsStringAsync();
				var data = JsonConvert.DeserializeObject<EmailStatusModel>(body);

				return data.messages.Select(x => new messagesModel(x)).ToList();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}

			return null;
		}
		public static string GetSendGridLicense()
		{
			return Module.EmailingService.GetSendGridLicense();
		}
		public static async Task<int> UpdateMessagesStatus(string apiKey, int limit = 1000)
		{
			int result = -1;
			try
			{
				var messages = await RetrieveEmailDeliveries(apiKey, limit);
				if(messages is null)
					return 0;
				if(messages.Count == 0)
					return 0;

				foreach(var item in messages)
				{
					result = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess
					   .UpdateMessageStatusToNotDelivered(MessageId: item.msg_id, EmailTo: item.to_email, EmailFrom: item.from_email);
				}
				await NotifyUserForFailedEmail(messages);

				return result;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return -1;
		}
		public static async Task<int> UpdateMessagesStatusSilent(string apiKey, int limit = 1000)
		{
			int result = -1;
			try
			{
				var messages = await RetrieveEmailDeliveries(apiKey, limit);
				if(messages is null)
					return 0;
				if(messages.Count == 0)
					return 0;

				foreach(var item in messages)
				{
					result = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess
					   .UpdateMessageStatusToNotDelivered(MessageId: item.msg_id, EmailTo: item.to_email, EmailFrom: item.from_email);
				}

				return result;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return -1;
		}
		public static async Task<int?> GetUndelivredMessagesCount(int UserId)
		{
			int? result = 0;
			try
			{
				var data = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess
				   .GetUndeliveredEmailsNotViewedByUserCount(UserId);
				if(data is not null && data.Count > 0)
					result = data?.FirstOrDefault().TotalCount;

				return result;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return -1;
		}
	}
}