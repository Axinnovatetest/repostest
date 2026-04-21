using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace Psz.Core.Helpers
{
	public class SMTP
	{
		private bool _asyncEmailSent { get; set; } = false;
		private MailMessage _mailMessage { get; set; }

		public void SendEmail(SmtpServerSettings smtpServerSettings, EmailModel email)
		{
			if(smtpServerSettings == null)
			{
				throw new Exception("smtpServerSettings must have a value");
			}

			if(email == null)
			{
				throw new Exception("email must have a value");
			}

			var smtpClient = new SmtpClient(smtpServerSettings.Host, smtpServerSettings.Port);
			smtpClient.EnableSsl = smtpServerSettings.EnableSsl;
			smtpClient.Credentials = new NetworkCredential(smtpServerSettings.Username, smtpServerSettings.Password);

			_mailMessage = new MailMessage();
			_mailMessage.From = new MailAddress(smtpServerSettings.Adress, smtpServerSettings.DispalyName);
			_mailMessage.To.Add(email.Adress);
			_mailMessage.Subject = email.Subject;
			_mailMessage.Body = email.Body;
			_mailMessage.Priority = smtpServerSettings.Priority;
			_mailMessage.IsBodyHtml = email.IsBodyHtml;

			if(!email.Async)
			{
				smtpClient.Send(_mailMessage);
				_mailMessage.Dispose();
			}
			else
			{
				smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

				string userState = email.UniqueId;
				smtpClient.SendAsync(_mailMessage, userState);
			}
		}

		private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			string token = (string)e.UserState;

			if(e.Cancelled)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, string.Format("[{0}] Send canceled.", token));
			}

			if(e.Error != null)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, string.Format("[{0}] {1}", token, e.Error.ToString()));
			}

			_asyncEmailSent = true;
			_mailMessage.Dispose();
		}
	}

	public class EmailModel
	{
		public string UniqueId { get; set; }

		public string Adress { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool IsBodyHtml { get; set; }
		public bool Async { get; set; }
	}

	public class SmtpServerSettings
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Adress { get; set; }
		public string DispalyName { get; set; }
		public System.Net.Mail.MailPriority Priority { get; set; }
	}
}