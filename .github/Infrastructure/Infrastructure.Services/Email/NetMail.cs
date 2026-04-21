using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Infrastructure.Services.Email
{
	public class NetMail
	{
		public EmailParamtersModel EmailParamtersModel { get; set; }
		public void SendEmail(string title, string content, List<string> toAddresses, List<KeyValuePair<string, System.IO.Stream>> attachments = null)
		{
			if(toAddresses != null && toAddresses.Count > 0)
			{
				var EmailSubject = title;
				string EmailHead = $"[Budget]";
				var EmailMessageBody = content;


				var mail = new System.Net.Mail.MailMessage();
				toAddresses.ForEach(ma =>
				{
					if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma))
					{ mail.To.Add(ma); }
				});
				mail.From = new MailAddress(EmailParamtersModel.Address, EmailHead, System.Text.Encoding.UTF8);
				mail.Subject = EmailSubject;
				mail.SubjectEncoding = System.Text.Encoding.UTF8;
				mail.Body = EmailMessageBody;
				mail.BodyEncoding = System.Text.Encoding.UTF8;
				mail.IsBodyHtml = true;
				mail.Priority = MailPriority.High;
				mail.IsBodyHtml = true;
				if(attachments != null && attachments.Count > 0)
				{
					attachments.ForEach(a => mail.Attachments.Add(new Attachment(a.Value, a.Key)));
				}
				SmtpClient client = new SmtpClient();
				client.Credentials = new System.Net.NetworkCredential(EmailParamtersModel.Address, EmailParamtersModel.Password);
				client.Port = EmailParamtersModel.Port;
				client.Host = EmailParamtersModel.Host;
				client.EnableSsl = EmailParamtersModel.SslEnabled;
				try
				{
					client.Send(mail);
				} catch(Exception e)
				{
					Logging.Logger.LogError("Sending Email failed, Error: " + e.Message);
					throw;
				}
			}
		}

		public void InitiateEmailSender(EmailParamtersModel emailParamters)
		{
			try
			{
				#region > Validation
				if(string.IsNullOrEmpty(emailParamters.Host))
				{
					throw new Exception("clientHost is empty");
				}
				if(string.IsNullOrEmpty(emailParamters.Username))
				{
					throw new Exception("clientUsername is empty");
				}
				if(string.IsNullOrEmpty(emailParamters.Password))
				{
					throw new Exception("clientPassword is empty");
				}
				if(string.IsNullOrEmpty(emailParamters.Address))
				{
					throw new Exception("emailAdress is empty");
				}
				if(string.IsNullOrEmpty(emailParamters.DisplayName))
				{
					throw new Exception("emailDisplayName is empty");
				}
				#endregion

				EmailParamtersModel = emailParamters;

			} catch(Exception e)
			{
				Logging.Logger.LogError("Initialisation of Email Sender failed, Error: " + e.Message);
			}
		}
	}
}
