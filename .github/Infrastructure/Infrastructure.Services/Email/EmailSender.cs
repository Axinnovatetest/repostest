using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Mail;
using System.Timers;

namespace Infrastructure.Services.Email
{
	public class EmailSender
	{
		private static bool _isBusy { get; set; } = false;
		private static bool _isFirst { get; set; } = true;
		private static System.Timers.Timer _timer { get; set; }
		private static bool _IsActivated { get; set; } = false;
		private static uint _sendFrequency { get; set; } = 2;

		private static string _clientHost { get; set; }
		private static int _clientPort { get; set; }
		private static string _clientUsername { get; set; }
		private static string _clientPassword { get; set; }
		private static bool _clientSslEnabled { get; set; }
		private static string _emailAdress { get; set; }
		private static string _emailDisplayName { get; set; }

		private static List<EmailModel> _waitingList { get; set; } = new List<EmailModel>();

		public EmailSender()
		{ }
		public EmailSender(string clientHost,
			int clientPort,
			string clientUsername,
			string clientPassword,
			bool clientSslEnabled,
			string emailAdress,
			string emailDisplayName)
		{
			try
			{
				#region > Validation
				if(string.IsNullOrEmpty(clientHost))
				{
					throw new Exception("clientHost is empty");
				}
				if(string.IsNullOrEmpty(clientUsername))
				{
					throw new Exception("clientUsername is empty");
				}
				if(string.IsNullOrEmpty(clientPassword))
				{
					throw new Exception("clientPassword is empty");
				}
				if(string.IsNullOrEmpty(emailAdress))
				{
					throw new Exception("emailAdress is empty");
				}
				if(string.IsNullOrEmpty(emailDisplayName))
				{
					throw new Exception("emailDisplayName is empty");
				}
				#endregion

				_clientHost = clientHost;
				_clientPort = clientPort;
				_clientUsername = clientUsername;
				_clientPassword = clientPassword;
				_clientSslEnabled = clientSslEnabled;
				_emailAdress = emailAdress;
				_emailDisplayName = emailDisplayName;

				double period = 1 / (double)_sendFrequency;
				initTimer(period);
			} catch(Exception e)
			{
				Logging.Logger.LogError("Initialisation of Email Sender failed, Error: " + e.Message);
			}
		}

		public static void Activate()
		{
			_IsActivated = true;
		}

		public static void SendEmail(EmailModel data)
		{
			#region > Validation
			if(data.To == null || data.To.Count == 0)
			{
				throw new Exception("No receiver email adress was found");
			}
			if(string.IsNullOrWhiteSpace(data.Subject) && string.IsNullOrWhiteSpace(data.Body))
			{
				throw new Exception("Subject and Body are empty");
			}
			#endregion

			// > Add to Waiting List
			lock(_waitingList)
			{
				_waitingList.Add(data);
			}
		}
		public static void SendDirectEmail(EmailModel data)
		{
			#region > Validation
			if(data.To == null || data.To.Count == 0)
			{
				throw new Exception("No receiver email adress was found");
			}
			if(string.IsNullOrWhiteSpace(data.Subject) && string.IsNullOrWhiteSpace(data.Body))
			{
				throw new Exception("Subject and Body are empty");
			}
			#endregion

			sendEmail(data);
		}

		private static void sendNextEmail()
		{
			if(!_IsActivated)
			{
				return;
			}

			if(_waitingList.Count == 0)
			{
				return;
			}

			lock(_waitingList)
			{
				var nextEmail = _waitingList
					.OrderBy(e => e.CreationTime)
					.FirstOrDefault();
				if(nextEmail == null)
				{
					return;
				}

				sendEmail(nextEmail);

				_waitingList.Remove(nextEmail);
			}
		}

		#region > Timer
		private static void initTimer(double interval)
		{
			_timer = new System.Timers.Timer();
			_timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
			_timer.Interval = interval * 1000; // 1000 = 1s
			_timer.Enabled = true;

			_timer.Start();
		}

		private static void onTimedEvent(object source, ElapsedEventArgs e)
		{
			if(!_isBusy)
			{
				_isBusy = true;

				try
				{
					if(_isFirst)
					{
						_isFirst = false;
					}

					sendNextEmail();
				} finally
				{
					_isBusy = false;
				}
			}
		}
		#endregion

		private static bool sendEmail(EmailModel data)
		{
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress(_emailDisplayName, _emailAdress));
				foreach(var to in data.To)
				{
					message.To.Add(new MailboxAddress("", to));
				}

				message.Subject = data.Subject;
				message.Body = new TextPart("plain")
				{
					Text = data.Body
				};
				message.Priority = (MessagePriority)(int)data.Priority;

				using(var client = new SmtpClient())
				{
					// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
					client.ServerCertificateValidationCallback = (s, c, h, e) => true;

					client.Connect(_clientHost, _clientPort, _clientSslEnabled);
					client.Authenticate(_clientUsername, _clientPassword);

					client.Send(message);
					client.Disconnect(true);
					return true;
				}
			} catch(Exception e)
			{
				Logging.Logger.Log(e);
				return false;
			}
		}
		public static void InitiateEmailSender(EmailParamtersModel emailParamters)
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

				_clientHost = emailParamters.Host;
				_clientPort = emailParamters.Port;
				_clientUsername = emailParamters.Username;
				_clientPassword = emailParamters.Password;
				_clientSslEnabled = emailParamters.SslEnabled;
				_emailAdress = emailParamters.Address;
				_emailDisplayName = emailParamters.DisplayName;

				double period = 1 / (double)_sendFrequency;
				initTimer(period);
			} catch(Exception e)
			{
				Logging.Logger.LogError("Initialisation of Email Sender failed, Error: " + e.Message);
			}
		}


	}

}
