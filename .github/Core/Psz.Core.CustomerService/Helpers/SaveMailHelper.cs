using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Psz.Core.CustomerService.Helpers
{
	using System;
	//using MimeKit;
	using System.IO;
	using System.Linq;
	using Outlook = Microsoft.Office.Interop.Outlook;
	public class SaveMailHelper
	{
		public static byte[] SaveMailMessage(string fileName, string From, List<string> To, string subject, string Body, string attachementPath, bool autoBCC = true)
		{
			try
			{

				//return getData3();

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress(From, "Invoice PSZ");
				To?.ForEach(x => mail.To.Add(x));
				mail.CC.Add(new MailAddress(From));
				if(autoBCC)
				{
					mail.Bcc.Add(new MailAddress(From));
				}
				mail.Subject = subject;
				mail.Body = Body;

				//Create the file attachment for this email message.
				Attachment data = new Attachment(attachementPath, MediaTypeNames.Application.Octet);
				// Add time stamp information for the file.
				ContentDisposition disposition = data.ContentDisposition;
				disposition.CreationDate = System.IO.File.GetCreationTime(attachementPath);
				disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachementPath);
				disposition.ReadDate = System.IO.File.GetLastAccessTime(attachementPath);
				// Add the file attachment to this email message.
				mail.Attachments.Add(data);
				var path = ToEMLStream(mail, fileName);
				var result = File.ReadAllBytes(path);
				File.Delete(path);
				return result;
			} catch(Exception e)
			{

				throw;
			}

		}
		public static string ToEMLStream(MailMessage msg, string fileName)
		{
			using(var client = new SmtpClient())
			{
				msg.Headers.Add("X-Unsent", "1");
				msg.IsBodyHtml = true;
				var tempFolder = Path.GetTempPath();
				client.UseDefaultCredentials = false;
				client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
				client.PickupDirectoryLocation = tempFolder;
				client.Send(msg);
				//file moving
				var defaultMsgPath = new DirectoryInfo(tempFolder).GetFiles()
				.OrderByDescending(f => f.LastWriteTime)
				.First();
				var realMsgPath = Path.Combine(tempFolder, fileName);
				try
				{
					File.Move(defaultMsgPath.FullName, realMsgPath);
				} catch(System.IO.IOException e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
				return realMsgPath;
			}
		}
		public static byte[] getData()
		{
			// Specify the sender, recipient, and "on behalf of" information
			string senderEmail = "no-reply@psz-electronic.com";
			string senderPassword = "Login$2022!PSZ";
			string recipientEmail = "Sani.ChaibouSalaou@psz-electronic.com";
			string actualSenderEmail = "Sani.ChaibouSalaou@psz-electronic.com";
			string actualSenderDisplayName = "Actual Sender Name";

			// Create a new MailMessage
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(senderEmail);
			mail.To.Add(recipientEmail);
			mail.Subject = "Test email";
			mail.Body = "This is a test email.";

			// Add a "Sender" header to specify the actual sender's email address
			mail.Headers.Add("Sender", $"Sani <{actualSenderEmail}>");

			// Create a SmtpClient for sending the email
			SmtpClient smtpClient = new SmtpClient("pszelectronic-com0i.mail.protection.outlook.com", 25);
			smtpClient.UseDefaultCredentials = false;
			smtpClient.EnableSsl = true;
			smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

			// Send the email
			//smtpClient.Send(mail);

			// Save the email as an EML file
			byte[] emlBytes;
			string emlFilePath = Path.Combine(Path.GetTempPath(), @"email.eml"); // Specify the file path where you want to save the EML file
			using(MemoryStream memoryStream = new MemoryStream())
			{
				using(TextWriter tw = new StreamWriter(memoryStream))
				{
					tw.Write(mail.Headers.ToString());
					tw.Write(mail.Body);
				}
				emlBytes = memoryStream.ToArray();
			}


			// -
			return emlBytes;
		}
		public static void getData2(string fileName, string From, List<string> To, string subject, string Body, string attachementPath, bool autoBCC = true)
		{
			var app = new Outlook.Application();
			Microsoft.Office.Interop.Outlook.Application olkApp1 = new Microsoft.Office.Interop.Outlook.Application();
			Microsoft.Office.Interop.Outlook.MailItem olkMail1 = olkApp1.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
			olkMail1.To = To[0];
			olkMail1.CC = "";
			olkMail1.SentOnBehalfOfName = "";
			olkMail1.Subject = "Assignment note";
			olkMail1.Body = "Assignment note";
			olkMail1.Save();


			//// -
			//Outlook.MailItem mail = olkApp1.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
			//mail.Subject = "Our itinerary";
			////mail.Attachments.Add(@"c:\travel\itinerary.doc", Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
			//Outlook.Account account = olkApp1.Session.Accounts["Hotmail"];
			//mail.SendUsingAccount = account;
			//mail.Send();



			//// ----
			//MailMessage mail = new MailMessage();
			//mail.From = new MailAddress(From);
			//To?.ForEach(x => mail.To.Add(x));
			//if(autoBCC)
			//{
			//	mail.Bcc.Add(new MailAddress(From));
			//}
			//mail.Subject = subject;
			//mail.Body = Body;

			//// - 
			//string sentOnBehalfOfEmail = "Sani.ChaibouSalaou@psz-electronic.com";
			//string sentOnBehalfOfDisplayName = "Sani";
			//// Add a "Sender" header to specify the actual sender's email address
			//mail.Headers.Add("Sender", $"{sentOnBehalfOfDisplayName} <{sentOnBehalfOfEmail}>");

			////Create the file attachment for this email message.
			//Attachment data = new Attachment(attachementPath, MediaTypeNames.Application.Octet);
			//// Add time stamp information for the file.
			//ContentDisposition disposition = data.ContentDisposition;
			//disposition.CreationDate = System.IO.File.GetCreationTime(attachementPath);
			//disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachementPath);
			//disposition.ReadDate = System.IO.File.GetLastAccessTime(attachementPath);
			//// Add the file attachment to this email message.
			//mail.Attachments.Add(data);
			//var path = ToEMLStream(mail, fileName);
			//var result = File.ReadAllBytes(path);
			//File.Delete(path);
			//return result;
		}
		public static byte[] getData3()
		{
			// Create an instance of Outlook.Application
			//var outlookApp = new Outlook.Application();


			// Create a MailMessage object
			MailMessage mail = new MailMessage();
			mail.Subject = "Testemail";
			mail.Body = "This is a test email.";
			mail.To.Add(new MailAddress("no-reply@psz-electronic.com"));
			mail.From = new MailAddress("ERP-Feedback@psz-electronic.com");

			// Set the "Sent on behalf of" header
			mail.Headers.Add("X-Sender", "Sani.ChaibouSalaou@psz-electronic.com");
			mail.Headers.Add("X-Unsent", "1");
			mail.IsBodyHtml = true;

			// Create a SmtpClient object
			SmtpClient smtpClient = new SmtpClient("pszelectronic-com0i.mail.protection.outlook.com", 25);
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential("no-reply@psz-electronic.com", "Login$2022!PSZ");
			smtpClient.EnableSsl = true;

			// Send the email
			smtpClient.Send(mail);
			var r = ConvertMailMessageToEmlByteArray(mail, "Test.eml");
			return r;
			//// Create a new MailItem
			//Outlook.MailItem mail = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
			//Outlook.Account account = outlookApp.Session.Accounts[0];
			//mail.SendUsingAccount = account;
			//mail.Subject = "Test email";
			//mail.Body = "This is a test email.";
			//mail.To = "recipient@example.com";

			//// Add a "Sender" property to specify the actual sender's email address
			//mail.SentOnBehalfOfName = "Sani.ChaibouSalaou@psz-electronic.com";

			//// Save the MailItem as an EML file
			//string emlFilePath = Path.Combine(Path.GetTempPath(), @"email.eml"); // Specify the path where the EML file will be saved
			//mail.SaveAs(emlFilePath, Outlook.OlSaveAsType.olMSG);

			//// Read the EML file as a byte array
			//byte[] emlBytes = File.ReadAllBytes(emlFilePath);

			//// -
			//return emlBytes;
		}
		public static byte[] ConvertMailMessageToEmlByteArray(MailMessage mailMessage, string fileName)
		{
			using(MemoryStream memoryStream = new MemoryStream())
			{
				using(TextWriter textWriter = new StreamWriter(memoryStream))
				{
					using(SmtpClient smtpClient = new SmtpClient())
					{
						smtpClient.UseDefaultCredentials = true; // Use your SMTP configuration here
						smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
						smtpClient.PickupDirectoryLocation = Path.GetTempPath(); // Specify a directory to save the EML file

						smtpClient.Send(mailMessage); // Save the MailMessage as an EML file

						var defaultMsgPath = new DirectoryInfo(Path.GetTempPath()).GetFiles()
						.OrderByDescending(f => f.LastWriteTime)
						.First();

						using(FileStream fileStream = new FileStream(defaultMsgPath.FullName, FileMode.Open, FileAccess.Read))
						{
							fileStream.CopyTo(memoryStream); // Copy the EML file content to the MemoryStream
						}
					}
				}

				return memoryStream.ToArray(); // Return the byte array of the EML file
			}
		}

	}
}