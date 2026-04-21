using System;
using System.Collections.Generic;
using SendGrid.Helpers.Mail;
using SendGrid;
namespace Infrastructure.Services.Email
{
	using global::MailKit.Net.Smtp;
	using global::MailKit.Security;
	using MimeKit;
	using System.Linq;
	using Org.BouncyCastle.Cms;
	using iText.Html2pdf.Attach;
	using System.IO;
	using System.Threading.Tasks;
	using Geocoding;
	using Microsoft.AspNetCore.Mvc;
	using iText.Layout.Element;
	using global::Geocoding.Microsoft.Json;
	using Infrastructure.Services.Email.Models;
	using Newtonsoft.Json;
	using static Infrastructure.Services.Reporting.IText;
	using System.Reflection;
	using Infrastructure.Data.Entities.Tables.TLS;

	public class MailKit
	{
		private static int MAXEMAILCONTENTLENGTH = 3999;
		public EmailParamtersModel EmailParamtersModel { get; set; }
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
		public void SendBomEmail(int type, string title, string content, List<string> toAddresses, string attachment_path, List<string> toAddressesCC = null)
		{
			try
			{

				toAddresses = toAddresses?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				toAddressesCC = toAddressesCC?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				// - 2025-02-17
				toAddressesCC?.RemoveAll(item => toAddresses.Contains(item, StringComparer.CurrentCultureIgnoreCase));

				// 2025-01-18 - remove non-valida email addresses
				toAddresses = toAddresses?.Where(x => Helpers.IsValidEmail(x)).ToList();
				toAddressesCC = toAddressesCC?.Where(x => Helpers.IsValidEmail(x)).ToList();

				if(EmailParamtersModel != null /*&& EmailParamtersModel.BOMEmailDestinations != null && EmailParamtersModel.BOMEmailDestinations.Count > 0*/)
				{
					var EmailSubject = title;
					string EmailHead = (type == 0) ? $"[BOM CHANGES]" : $"[LAGER BESTAND CHANGES]";
					var EmailMessageBody = getBodyContent(content);

					var mailMessage = new MimeMessage();
					mailMessage.From.Add(new MailboxAddress(EmailParamtersModel.Username, EmailParamtersModel.Address));
					List<string> _destinations = (type == 0) ? toAddresses : EmailParamtersModel.LagerEmailDestinations;
					_destinations?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.To.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					toAddressesCC?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.Cc.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					// - 2022-01-27
					//if(Helpers.IsValidEmail(EmailParamtersModel.AdminEmail))
					//{ mailMessage.Bcc.Add(new MailboxAddress(EmailParamtersModel.AdminEmail)); }
					if(mailMessage.To != null && mailMessage.To.Count > 0)
					{
						mailMessage.Subject = EmailSubject;
						var bodyBuilder = new BodyBuilder();
						bodyBuilder.HtmlBody = EmailMessageBody;
						if(!string.IsNullOrWhiteSpace(attachment_path))
						{
							bodyBuilder.Attachments.Add(attachment_path);
							//attachments.ForEach(a => bodyBuilder.Attachments.Add(a.Key, a.Value));
						}
						mailMessage.Body = bodyBuilder.ToMessageBody();

						// - 2022-08-23 - failover - retry scheme
						retrySchemeAsync(mailMessage);
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
			}
		}
		public void SendBomEmailAsync(int type, string title, string content, List<string> toAddresses, string attachment_path, List<string> toAddressesCC = null)
		{
			try
			{
				toAddresses = toAddresses?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				if(toAddressesCC is null)
					toAddressesCC = new List<string>();

				toAddressesCC = toAddressesCC?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				// - 2025-02-17
				toAddressesCC?.RemoveAll(item => toAddresses.Contains(item, StringComparer.CurrentCultureIgnoreCase));

				// 2025-01-18 - remove non-valida email addresses
				toAddresses = toAddresses?.Where(x => Helpers.IsValidEmail(x)).ToList();
				toAddressesCC = toAddressesCC?.Where(x => Helpers.IsValidEmail(x)).ToList();

				if(EmailParamtersModel != null /*&& EmailParamtersModel.BOMEmailDestinations != null && EmailParamtersModel.BOMEmailDestinations.Count > 0*/)
				{
					var EmailSubject = title;
					string EmailHead = (type == 0) ? $"[BOM CHANGES]" : $"[LAGER BESTAND CHANGES]";
					var EmailMessageBody = getBodyContent(content);

					var mailMessage = new MimeMessage();
					mailMessage.From.Add(new MailboxAddress(EmailParamtersModel.Username, EmailParamtersModel.Address));
					//
					FilterUnactiveEmails(toAddresses);
					//
					List<string> _destinations = (type == 0) ? toAddresses : EmailParamtersModel.LagerEmailDestinations;
					_destinations?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.To.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					toAddressesCC?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.Cc.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					// - 2022-01-27
					//if(Helpers.IsValidEmail(EmailParamtersModel.AdminEmail))
					//{ mailMessage.Bcc.Add(new MailboxAddress(EmailParamtersModel.AdminEmail)); }
					if(mailMessage.To != null && mailMessage.To.Count > 0)
					{
						mailMessage.Subject = EmailSubject;
						var bodyBuilder = new BodyBuilder();
						bodyBuilder.HtmlBody = EmailMessageBody;
						if(!string.IsNullOrWhiteSpace(attachment_path))
						{
							bodyBuilder.Attachments.Add(attachment_path);
							//attachments.ForEach(a => bodyBuilder.Attachments.Add(a.Key, a.Value));
						}
						mailMessage.Body = bodyBuilder.ToMessageBody();

						// - 2022-08-23 - failover - retry scheme
						retrySchemeAsync(mailMessage);
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
			}
		}
		[Obsolete("SendEmail is deprecated, please use SendEmailAsync instead.")]
		public bool SendEmail(string title, string content, List<string> toAddresses, List<KeyValuePair<string, System.IO.Stream>> attachments = null, List<string> toAddressesCC = null,
			bool saveHistory = true, string senderEmail = "", string senderName = "", int senderId = 0, bool? senderCC = null, List<int> attachmentIds = null, string emailHead = "")
		{
			try
			{
				toAddresses = toAddresses?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				toAddressesCC = toAddressesCC?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				// - 2025-02-17
				toAddressesCC?.RemoveAll(item => toAddresses.Contains(item, StringComparer.CurrentCultureIgnoreCase));

				// 2025-01-18 - remove non-valida email addresses
				toAddresses = toAddresses?.Where(x => Helpers.IsValidEmail(x)).ToList();
				toAddressesCC = toAddressesCC?.Where(x => Helpers.IsValidEmail(x)).ToList();

				if(saveHistory)
				{
					try
					{
						Infrastructure.Data.Access.Tables.TLS.EmailHistoryAccess.Insert(new Infrastructure.Data.Entities.Tables.TLS.EmailHistoryEntity
						{
							AttachmentIds = attachmentIds != null ? string.Join("|", attachmentIds) : "", // - pipe-separated ids
							CCEmails = toAddressesCC != null ? string.Join("|", toAddressesCC) : "",
							EmailMessage = content,
							EmailTitle = title,
							Id = -1,
							SenderCC = senderCC,
							SenderUserEmail = senderEmail,
							SenderUserId = senderId,
							SenderUserName = senderName,
							SendingTime = DateTime.Now,
							ToEmail = toAddresses != null ? string.Join("|", toAddresses) : ""
						});
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(ex);
						return false;
					}
				}

				if(!string.IsNullOrWhiteSpace(EmailParamtersModel.ServerEnvironment))
					emailHead = $"[{EmailParamtersModel.ServerEnvironment}]";

				FilterUnactiveEmails(toAddresses);
				if(toAddresses != null && toAddresses.Count > 0)
				{
					//
					//
					var EmailSubject = title;
					string EmailHead = emailHead;
					var EmailMessageBody = getBodyContent(content);

					var mailMessage = new MimeMessage();
					mailMessage.From.Add(new MailboxAddress(EmailParamtersModel.Username, EmailParamtersModel.Address));
					toAddresses?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.To.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					toAddressesCC?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.Cc.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					// - 2022-01-27
					//if(Helpers.IsValidEmail(EmailParamtersModel.AdminEmail))
					//{ mailMessage.Bcc.Add(new MailboxAddress(EmailParamtersModel.AdminEmail)); }
					if(mailMessage.To != null && mailMessage.To.Count > 0)
					{
						mailMessage.Subject = EmailSubject;
						var bodyBuilder = new BodyBuilder();
						bodyBuilder.HtmlBody = EmailMessageBody;
						if(attachments != null && attachments.Count > 0)
						{
							attachments.ForEach(a => bodyBuilder.Attachments.Add(a.Key, a.Value));
						}
						mailMessage.Body = bodyBuilder.ToMessageBody();

						// - 2022-08-23 - failover - retry scheme
						var ex = retrySchemeAsync(mailMessage).Result;
						if(ex == null)
							return true;
						return false;
					}
					return false;
				}
				return false;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return false;
			}
		}
		public async Task<bool> SendEmailAsync(string title, string content, List<string> toAddresses, List<KeyValuePair<string, System.IO.Stream>> attachments = null, List<string> toAddressesCC = null,
			bool saveHistory = true, string senderEmail = "", string senderName = "", int senderId = 0, bool? senderCC = null, List<int> attachmentIds = null, string emailHead = "", bool IsTable = false)
		{
			try
			{
				toAddresses = toAddresses?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				toAddressesCC = toAddressesCC?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				// - 2025-02-17
				toAddressesCC?.RemoveAll(item => toAddresses.Contains(item, StringComparer.CurrentCultureIgnoreCase));

				// 2025-01-18 - remove non-valida email addresses
				toAddresses = toAddresses?.Where(x => Helpers.IsValidEmail(x)).ToList();
				toAddressesCC = toAddressesCC?.Where(x => Helpers.IsValidEmail(x)).ToList();

				if(saveHistory)
				{
					try
					{
						var datatoinsert = new EmailHistoryEntity
						{
							AttachmentIds = attachmentIds != null ? string.Join("|", attachmentIds) : "", // - pipe-separated ids
							CCEmails = toAddressesCC != null ? string.Join("|", toAddressesCC) : "",
							EmailMessage = content,
							EmailTitle = title,
							Id = -1,
							SenderCC = senderCC,
							SenderUserEmail = senderEmail,
							SenderUserId = senderId,
							SenderUserName = senderName,
							SendingTime = DateTime.Now,
							ToEmail = toAddresses != null ? string.Join("|", toAddresses) : ""
						};
						datatoinsert.TruncateToColumnLimits();
						Infrastructure.Data.Access.Tables.TLS.EmailHistoryAccess.Insert(datatoinsert);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(ex);
					}
				}

				if(!string.IsNullOrWhiteSpace(EmailParamtersModel.ServerEnvironment))
					emailHead = $"[{EmailParamtersModel.ServerEnvironment}]";

				FilterUnactiveEmails(toAddresses);
				if(toAddresses != null && toAddresses.Count > 0)
				{
					//
					//
					var EmailSubject = title;
					string EmailHead = emailHead;
					var EmailMessageBody = getBodyContent(content, IsTable);

					var mailMessage = new MimeMessage();
					mailMessage.From.Add(new MailboxAddress(EmailParamtersModel.Username, EmailParamtersModel.Address));
					toAddresses?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.To.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					toAddressesCC?.Distinct()?.ToList()?.ForEach(ma =>
					{
						if(!string.IsNullOrEmpty(ma) && !string.IsNullOrWhiteSpace(ma) && Helpers.IsValidEmail(ma.Trim()))
						{ mailMessage.Cc.Add(new MailboxAddress(ma.Trim(), ma.Trim())); }
					});
					// - 2022-01-27
					//if(Helpers.IsValidEmail(EmailParamtersModel.AdminEmail))
					//{ mailMessage.Bcc.Add(new MailboxAddress(EmailParamtersModel.AdminEmail)); }
					if(mailMessage.To != null && mailMessage.To.Count > 0)
					{
						mailMessage.Subject = EmailSubject;
						var bodyBuilder = new BodyBuilder();
						bodyBuilder.HtmlBody = EmailMessageBody;
						if(attachments != null && attachments.Count > 0)
						{
							attachments.ForEach(a => bodyBuilder.Attachments.Add(a.Key, a.Value));
						}
						mailMessage.Body = bodyBuilder.ToMessageBody();

						// - 2022-08-23 - failover - retry scheme
						var ex = await retrySchemeAsync(mailMessage);
						if(ex == null)
							return true;
						return false;
					}
					return false;
				}
				return false;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return false;
			}
		}
		public string GetSendGridLicense()
		{
			return EmailParamtersModel.SendGridLicense;
		}
		public string GetAdminEmail()
		{
			return EmailParamtersModel.AdminEmail;
		}
		public string GetMainAdress()
		{
			return EmailParamtersModel.Address;
		}
		/// <summary>
		/// /
		/// </summary>
		/// <param name="model"></param>
		/// <param name="from"></param>
		/// <param name="toAddresses"></param>
		/// <param name="attachments"></param>
		/// <param name="toAddressesCC"></param>
		/// <param name="saveHistory"></param>
		/// <param name="senderEmail"></param>
		/// <param name="senderName"></param>
		/// <param name="senderId"></param>
		/// <param name="senderCC"></param>
		/// <param name="attachmentIds"></param>
		/// <param name="emailHead"></param>
		/// <returns></returns>
		public async Task<bool> SendEmailSendGridWithDynamicTmeplate(object models, string content, string subject, GlobalModel model, string from, List<string> toAddresses, List<KeyValuePair<string, System.IO.Stream>> attachments = null, List<string> toAddressesCC = null,
			bool saveHistory = true, string senderEmail = "", string senderName = "", int senderId = 0, bool? senderCC = null, List<int> attachmentIds = null, string emailHead = "")
		{
			try
			{
				var ToReceivers = new List<EmailAddress>();
				var attachements = new List<Attachment>();

				toAddresses = toAddresses?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				toAddressesCC = toAddressesCC?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				// - 2025-02-17
				toAddressesCC?.RemoveAll(item => toAddresses.Contains(item, StringComparer.CurrentCultureIgnoreCase));

				// 2025-01-18 - remove non-valida email addresses
				toAddresses = toAddresses?.Where(x => Helpers.IsValidEmail(x)).ToList();
				toAddressesCC = toAddressesCC?.Where(x => Helpers.IsValidEmail(x)).ToList();


				//var model = (SendgridTemplateModel)models;

				if(saveHistory)
				{
					try
					{
						Infrastructure.Data.Access.Tables.TLS.EmailHistoryAccess.Insert(new Infrastructure.Data.Entities.Tables.TLS.EmailHistoryEntity
						{
							AttachmentIds = attachmentIds != null ? string.Join("|", attachmentIds) : "", // - pipe-separated ids
							CCEmails = toAddressesCC != null ? string.Join("|", toAddressesCC) : "",
							EmailMessage = content,
							EmailTitle = subject,
							Id = -1,
							SenderCC = senderCC,
							SenderUserEmail = senderEmail,
							SenderUserId = senderId,
							SenderUserName = senderName,
							SendingTime = DateTime.Now,
							ToEmail = toAddresses != null ? string.Join("|", toAddresses) : ""
						});
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(ex);
					}
				}
				var client = new SendGridClient(EmailParamtersModel.SendGridLicense);
				var fromSender = new EmailAddress(EmailParamtersModel.Address, EmailParamtersModel.Username);
				foreach(var item in toAddresses)
				{
					ToReceivers.Add(new EmailAddress(item, ""));
				}
				var msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(fromSender, ToReceivers, model.TemplateID, models);
				if(toAddressesCC?.Count > 0)
				{
					msg.AddCcs(toAddressesCC.Distinct().Select(x => new EmailAddress(x)).ToList());
				}
				if(attachments is not null)
				{
					foreach(var item in attachments)
					{
						attachements.Add(
							new Attachment()
							{
								Content = Convert.ToBase64String(Helpers.ReadFullStream(item.Value)),
								Type = @$"application/{Path.GetExtension(item.Key).Replace(".", "")}",
								Disposition = "attachment",
								Filename = @$"{item.Key}"
							}
							);
					}
					msg.Attachments = attachements;
				}

				var response = await client.SendEmailAsync(msg);

				return (response.StatusCode == System.Net.HttpStatusCode.Accepted);

			} catch(Exception)
			{

				throw;
			}
		}

		public async Task<Tuple<bool, string>> SendEmailSendGridWithStaticTemplate(string content, string subject, List<string> toAddresses, List<KeyValuePair<string, System.IO.Stream>> attachments = null, List<string> toAddressesCC = null,
			bool saveHistory = true, string senderEmail = "", string senderName = "", int senderId = 0, bool? senderCC = null, List<int> attachmentIds = null, string emailHead = "")
		{

			////return await SendEmailAsync(subject, content, toAddresses, attachments, toAddressesCC,
			//// saveHistory, senderEmail, senderName, senderId, senderCC, attachmentIds, emailHead);
			var EbasUI = Guid.NewGuid().ToString();
			var result = false;
			try
			{
				toAddresses = toAddresses?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				toAddressesCC = toAddressesCC?.Distinct(StringComparer.CurrentCultureIgnoreCase)?.ToList();
				// - 2025-02-17
				toAddressesCC?.RemoveAll(item => toAddresses.Contains(item, StringComparer.CurrentCultureIgnoreCase));

				// 2025-01-18 - remove non-valida email addresses
				toAddresses = toAddresses?.Where(x => Helpers.IsValidEmail(x)).ToList();
				toAddressesCC = toAddressesCC?.Where(x => Helpers.IsValidEmail(x)).ToList();

				var ToReceivers = new List<EmailAddress>();
				var attachements = new List<Attachment>();

				if(saveHistory)
				{
					try
					{
						Infrastructure.Data.Access.Tables.TLS.EmailHistoryAccess.Insert(new Infrastructure.Data.Entities.Tables.TLS.EmailHistoryEntity
						{
							AttachmentIds = attachmentIds != null ? string.Join("|", attachmentIds) : "", // - pipe-separated ids
							CCEmails = toAddressesCC != null ? string.Join("|", toAddressesCC) : "",
							EmailMessage = content,
							EmailTitle = subject,
							Id = -1,
							SenderCC = senderCC,
							SenderUserEmail = senderEmail,
							SenderUserId = senderId,
							SenderUserName = senderName,
							SendingTime = DateTime.Now,
							ToEmail = toAddresses != null ? string.Join("|", toAddresses) : ""
						});
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(ex);
					}
				}
				var client = new SendGridClient(EmailParamtersModel.SendGridLicense);

				var EmailMessageBody = getBodyContent(content);
				EmailAddress fromSender;
				if(senderEmail is null || senderEmail == "")
					fromSender = new EmailAddress(EmailParamtersModel.Address, EmailParamtersModel.Username);
				else
					fromSender = new EmailAddress(senderEmail, senderName);

				foreach(var item in toAddresses)
				{
					ToReceivers.Add(new EmailAddress(item, ""));
				}
				var msg = MailHelper.CreateSingleEmailToMultipleRecipients(fromSender, ToReceivers, subject, "", EmailMessageBody);

				if(toAddresses?.Count > 0 && toAddressesCC?.Count > 0)
				{
					toAddressesCC = toAddressesCC.Where(x => !toAddresses.Exists(y => y == x)).ToList();
				}
				if(toAddressesCC?.Count > 0)
				{
					msg.AddCcs(toAddressesCC.Distinct().Select(x => new EmailAddress(x)).ToList());
				}

				//Infrastructure.Services.Logging.Logger.Log(Logging.Logger.Levels.Trace, Newtonsoft.Json.JsonConvert.SerializeObject(msg));

				if(attachments is not null)
				{
					foreach(var item in attachments)
					{
						attachements.Add(
							new Attachment()
							{
								Content = Convert.ToBase64String(Helpers.ReadFullStream(item.Value)),
								Type = @$"{Files.MimeHelper.GetMimeType(Path.GetExtension(item.Key))}",
								Disposition = "attachment",
								Filename = @$"{item.Key}"
							}
							);
					}
					msg.Attachments = attachements;
				}
				var response = await client.SendEmailAsync(msg);

				if(!response.IsSuccessStatusCode)
				{
					SaveEmailHistory(
						EbasUI,
						userId: senderId,
						MessageId: EbasUI,
						EmailFrom: EmailParamtersModel.Address,
						EmailTo: toAddresses,
						ccs: toAddressesCC,
						Subject: subject,
						failed: 1,
						EmailContent: content
						);
					var responseBody = await response.Body.ReadAsStringAsync();
					Logging.Logger.LogError($"{response.StatusCode}| email from {msg?.From.Email} to {string.Join(", ", toAddresses)} |  with subject  {subject}  has {attachements?.Count} attached files failed to be sent, Reason--->{responseBody}");
					return new Tuple<bool, string>(result, EbasUI);
				}
				SaveEmailHistory(
					ui: EbasUI,
					userId: senderId,
					MessageId: Infrastructure.Services.Helpers.SendGridHelper.GetMessageIdFromHttpResponse(response),
					EmailFrom: EmailParamtersModel.Address,
					EmailTo: toAddresses,
					ccs: toAddressesCC,
					Subject: subject,
					failed: -1,
					EmailContent: content
					);
				Logging.Logger.LogFatal($"{response.StatusCode}| email from {msg?.From.Email} to {string.Join(", ", toAddresses)} |  with subject  {subject}  has {attachements?.Count} attached files was sent Successfully");

				result = true;
				return new Tuple<bool, string>(result, EbasUI);

			} catch(Exception e)
			{
				SaveEmailHistory(
					ui: EbasUI,
					userId: senderId,
					MessageId: EbasUI,
					EmailFrom: EmailParamtersModel.Address,
					EmailTo: toAddresses,
					ccs: toAddressesCC,
					Subject: subject,
					failed: 1, Exception: e.Message,
					EmailContent: content
					);
				Logging.Logger.Log("Unable to send Email an error has occured : " + e);
				throw;
			} finally
			{
				if(!result)
					NotifyUserForInternalFailedEmail(EbasUI);
			}

		}
		private void SaveEmailHistory(string ui, int userId, string MessageId, string EmailFrom, List<string> EmailTo, List<string> ccs, string Subject, int EmailStatus = 0, string Exception = "", string EmailContent = "", int failed = -1)
		{
			try
			{


				if(EmailTo is null || EmailTo.Count == 0)
					return;

				foreach(var item in EmailTo)
				{
					var res = Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.Insert(
									new Data.Entities.Tables.FNC.SendGridEmailing.
										PSZ_SendGrid_Email_Not_DeliveredEntity()
									{
										UserId = userId,
										MessageId = MessageId,
										EmailFrom = EmailFrom,
										EmailTo = item,
										EmailStatus = EmailStatus,
										Subject = Subject,
										EmailContent = (EmailContent.Length < 1000 ? EmailContent : EmailContent.Substring(0, 200)),
										Failed = failed,
										Exception = Exception,
										AddedOn = DateTime.Now,
										UserNotifiedByEmail = -1,
										ViewedByTheUser = -1,
										EBASUI = ui,
										Iscc = -1
									});
				}

				if(ccs is null || ccs.Count == 0)
					return;

				foreach(var item in ccs)
				{
					var res = Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.Insert(
									new Data.Entities.Tables.FNC.SendGridEmailing.
										PSZ_SendGrid_Email_Not_DeliveredEntity()
									{
										UserId = userId,
										MessageId = MessageId,
										EmailFrom = EmailFrom,
										EmailTo = item,
										EmailStatus = EmailStatus,
										Subject = Subject,
										EmailContent = (EmailContent.Length < 1000 ? EmailContent : EmailContent.Substring(0, 200)),
										Failed = failed,
										Exception = Exception,
										AddedOn = DateTime.Now,
										ViewedByTheUser = -1,
										UserNotifiedByEmail = -1,
										EBASUI = ui,
										Iscc = 1
									});
				}
			} catch(Exception e)
			{
				Logging.Logger.Log(e);
			}
		}
		public void NotifyUserForInternalFailedEmail(string messageId)
		{
			try
			{
				var fetchetData = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetInternalFailedEmailNotNotifiedUser(messageId);
				if(fetchetData is null || fetchetData.Count == 0)
					return;
				List<NotifyUserByEmailModel> messageInfo = fetchetData.Select(x => new Infrastructure.Services.Email.Models.NotifyUserByEmailModel(x)).ToList();

				if(messageInfo is null || messageInfo.Count == 0)
					return;

				var user = Infrastructure.Data.Access.Tables.FNC.UserAccess.Get(messageInfo.FirstOrDefault().UserId);
				var emails = messageInfo.Select(x => x.EmailTo).ToList();
				var emailContent = Infrastructure.Services.Email.Helpers.GetNotificationEmailContent(user.Name, messageInfo.FirstOrDefault().Subject, emails, (DateTime)messageInfo.FirstOrDefault().AddedOn, messageInfo.FirstOrDefault().link);

				SendEmailAsync(messageInfo.FirstOrDefault().Subject, emailContent, new List<string> { user.Email, GetAdminEmail() },
					   saveHistory: true, senderEmail: GetMainAdress(), senderName: "", senderId: user.Id, senderCC: false, attachmentIds: null);

				Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.UpdateMessageNotificationStatus(messageId, messageInfo.Select(x => x.EmailTo).ToList());

			} catch(Exception ex)
			{
				Logging.Logger.Log(ex);
			}
		}
		private async Task<Exception> retrySchemeAsync(MimeMessage mailMessage)
		{
			int retryMaxCount = 5;
			Exception retryEx = null;
			while(retryMaxCount > 0)
			{
				try
				{
					using(var client = new SmtpClient())
					{
						await client.ConnectAsync(EmailParamtersModel.Host, EmailParamtersModel.Port, EmailParamtersModel.SslEnabled ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable);
						await client.AuthenticateAsync(EmailParamtersModel.Address, EmailParamtersModel.Password);
						await client.SendAsync(mailMessage);
						await client.DisconnectAsync(true);
					}
					// - exit, on success
					retryMaxCount = -1;
					retryEx = null;
				} catch(Exception ex)
				{
					retryMaxCount--;
					retryEx = ex;
				}
			}

			// - save exception on failure
			if(retryEx != null)
			{
				Infrastructure.Services.Logging.Logger.Log(retryEx);
			}

			return retryEx;
		}
		string getFooterSignature()
		{
			var footer = "";
			if(EmailParamtersModel != null && EmailParamtersModel.FooterExternalSignature != null && EmailParamtersModel.FooterExternalSignature.Count > 0)
			{
				foreach(var footerParagraph in EmailParamtersModel.FooterExternalSignature)
				{
					if(footerParagraph != null && footerParagraph.Count > 0)
					{
						foreach(var footerLine in footerParagraph)
						{
							footer += $"<span>{footerLine}</span>";
							footer += "<br/>";
						}
						footer += "<br/>";
					}
				}
			}

			if(!string.IsNullOrWhiteSpace(footer))
				footer = "<table><tr><td style='font-size: 8pt;color:#1f497d;'>"
						+ "<hr style='border-top-width:1px;border-top-style:solid; border-top-color:#1f497d !important;'>"
						+ $"<span style='font-weight:600;'>Note:&nbsp;</span>{footer}"
						+ "</td></tr></table>";

			if(!string.IsNullOrWhiteSpace(EmailParamtersModel.ServerEnvironment))
				footer += $"<hr/><div>{EmailParamtersModel.ServerEnvironment}</div>";

			return footer;
		}
		string getBodyContent(string emailContent, bool IsTable = false)
		{
			// footer: background-color:#e8f1ff;
			if(IsTable)
				return "<table>"
					+ "<tr><td style='background-color:#fff; padding:15px; color:#222;'>" + (string.IsNullOrWhiteSpace(emailContent) ? "" : string.Join(" ", emailContent?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))) + "</td></tr>"
					+ "<tr><td style='padding:15px; color:#333;'>" + getFooterSignature() + "</td></tr>"
					+ " </table>";


			return "<table>"
					+ "<tr><td style='background-color:#fff; padding:15px; color:#222;'>" + (string.IsNullOrWhiteSpace(emailContent) ? "" : string.Join("<br/>", emailContent?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))) + "</td></tr>"
					+ "<tr><td style='padding:15px; color:#333;'>" + getFooterSignature() + "</td></tr>"
					+ " </table>";
		}
		private List<string> FilterUnactiveEmails(List<string> recievers)
		{
			if(recievers == null || recievers.Count == 0)
				return null;

			var deactivatedUsersEmail = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByMails(recievers)
				?.Where(x => x.IsActivated == true)
				?.Select(x => x.Email)?.ToList();
			if(deactivatedUsersEmail != null && deactivatedUsersEmail.Count > 0)
			{
				recievers = recievers.Where(x => !deactivatedUsersEmail.Exists(y => y == x))?.ToList();
			}

			return recievers;
		}
	}
}