using Infrastructure.Data.Entities.Joins.CTS;
using Infrastructure.Data.Entities.Tables.PRS;
using Minio.DataModel.Notification;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using static QRCoder.PayloadGenerator;

namespace Infrastructure.Services.Email
{
	public static class Helpers
	{
		public static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			} catch
			{
				return false;
			}
		}

		public static byte[] ReadFullStream(Stream input)
		{
			using(MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
		public static string GetNotificationEmailContent(string User, string subject, List<string> emails, DateTime addedon, string link)
		{
			string concatintedEmail = "";
			foreach(var email in emails)
			{
				concatintedEmail += $"   - <span> {email} </span> <br/> ";
			}
			var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")} {User},</span><br/>"
				+ $"<br/><span style='font-size:1.15em;'>The Email with the subject :  <strong>{subject}</strong> failed to be delivered to the following recipients  : <br/> {concatintedEmail} <br/> <label>  on {addedon.ToString("dd.MM.yyyy HH:mm")} <label/>.<br/>";
			if(link is not null && link.Length > 2)
			{
				emailContent += $"<br/> Navigate to Data From <a href=\"{link}\">here</a> .<br/>";
			}
			emailContent += $" <br/> Please Verify the used email addresses and never hesitate to contact your IT team .<br/>";
			emailContent += $"<br/> Regards, <br/>";
			emailContent += $" IT Department <br/>";

			return emailContent;
		}
		public static string GetInvoiceCreationSummaryContent(List<CreatedInvoiceModels.CreatedInvoiceModel> CreatedInvoices, List<string> errors)
		{
			string errorcontent = "";
			string tablePart2 = "";

			if(errors != null && errors.Count > 0)
			{
				errorcontent = $"<span style='font-size:1.15em;'>Following is the List of Failed Invoices ({errors.Count} Total):</span><br/>" +
					$"<ul>{string.Join("", errors.Select(x => $"<li>{x}</li>"))}</ul>";
			}

			foreach(var createdInvoice in CreatedInvoices)
			{
				var sent = "";
				if(createdInvoice.Sent)
				{
					sent = @$" <td style=""border: 1px solid #000; padding: 1px;color:green;"">{createdInvoice.Sent}</td>" +
							" </tr>";
				}
				else
				{
					sent = @$" <td style=""border: 1px solid #000; padding: 1px; color:red;"">{createdInvoice.Sent}</td>" +
							"  </tr>";
				}
				;
				tablePart2 += @$"
				<tr>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.CustomerNumber}, {createdInvoice.Customer}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.ForfallNr}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.RgNumber} {createdInvoice.TypeInvoice}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{(createdInvoice.TypeInvoice?.Trim()?.ToLower() != "sammelrechnung" ? createdInvoice.LsNumber : $"{createdInvoice.SammelItems?.Count} LS: " + string.Join(" | ", createdInvoice.SammelItems?.Select(z => $"{z.LSForfallNr}&nbsp;&nbsp;")))}</td>
                <td style=""border: 1px solid #000; padding: 1px;"">{createdInvoice.Amount} €</td>
				" + sent;

			}

			string tablepart1 = $"<span style='font-size:1.15em;'>Following is the List of created Invoices being sent automatically ({CreatedInvoices.Count} Total):</span>" +
				@$"<table style=""width: 100%; border-collapse: collapse;"">
            <tr>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Customer</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Betreff</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">RG</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">LS-Nummer</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Amount</th>
                <th style=""border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;"">Being Sent</th>
            </tr>
            {tablePart2}
        </table>";

			var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>" +
				$"{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>" +
				$"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>" +
				$"{(CreatedInvoices.Count > 0 ? tablepart1 : string.Empty)}" +
				$"{(errors.Count > 0 ? errorcontent : string.Empty)}" +
				$"<br/> <label>on {DateTime.Now.ToString("dd.MM.yyyy HH:mm")} <label/>.<br/>";

			emailContent += $"<br/> Regards, <br/>";
			emailContent += $" IT Department <br/>";
			return emailContent;
		}
		public static byte[] GetExcelFile(
		   List<CreatedInvoiceModels.CreatedInvoiceModel> CSContactFAExcelEntity)
		{
			try
			{
				ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"data");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 8;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Customer-Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Customer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Betreff";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "RG-Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "RG-Type";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "LS-Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "RG-Amount";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Being sent";


					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(CSContactFAExcelEntity != null && CSContactFAExcelEntity.Count > 0)
					{
						foreach(var p in CSContactFAExcelEntity)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = p.CustomerNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Customer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.ForfallNr;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.RgNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.TypeInvoice;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = $"{(p.TypeInvoice?.Trim()?.ToLower() != "sammelrechnung" ? p.LsNumber : $"{p.SammelItems?.Count} LS: " + string.Join(" | ", p.SammelItems?.Select(z => $"{z.LSForfallNr}  ")))}";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Amount;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = $"{p.Sent}";
							// - 
							rowNumber += 1;
						}
					}
					// Doc content
					if(CSContactFAExcelEntity != null && CSContactFAExcelEntity.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}
					// - color cells
					if(CSContactFAExcelEntity != null && CSContactFAExcelEntity.Count > 0)
					{
						var n = headerRowNumber + 1;
						foreach(var p in CSContactFAExcelEntity)
						{
							// - styles
							worksheet.Cells[n, startColumnNumber + 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
							worksheet.Cells[n, startColumnNumber + 7].Style.Fill.BackgroundColor.SetColor(p.Sent ? System.Drawing.ColorTranslator.FromHtml("#32f517") : System.Drawing.ColorTranslator.FromHtml("#f51717"));
							n += 1;
						}
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Set some document properties
					package.Workbook.Properties.Title = "data";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public static void StatusEmailNotification(Enums.RahmenAction action, string _user, int RahmenId, Enums.RahmenRedirect redirect)
		{
			try
			{
				var body = "";
				//var content = "";
				var title = "";
				var addresses = new List<string>();
				var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(RahmenId);
				var RahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(RahmenId);

				#region // - 2022-12-01 - send Mail to Assigned Employee
				var assignedEmployeeAddresses = new List<string>();
				var pCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(true, RahmenExtensionEntity.CustomerId ?? -1);
				var npCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(false, RahmenExtensionEntity.CustomerId ?? -1);
				if(npCustomersNumbers != null)
				{
					if(DateTime.Now >= npCustomersNumbers.ValidFromTime.Date && DateTime.Now <= npCustomersNumbers.ValidIntoTime.Date.AddDays(1))
					{
						if(pCustomersNumbers != null)
						{
							assignedEmployeeAddresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(npCustomersNumbers.UserId)?.Email ?? "");
						}
					}
					else
					{
						npCustomersNumbers = null;
					}
				}
				if(pCustomersNumbers != null)
				{
					assignedEmployeeAddresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(pCustomersNumbers.UserId)?.Email ?? "");
				}
				#endregion

				switch(action)
				{
					case Enums.RahmenAction.SubmitValidate:
						var userValidationEmail = Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.GetUserEmailsWithRahmenValidation();

						body = Template("request_validation", RahmenEntity, null, _user, redirect);
						title = $"Validation Request – Rahmen | Document No. {RahmenEntity.Bezug}";
						if(redirect == Enums.RahmenRedirect.INS)
						{
							addresses.AddRange(
							Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
							Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.GetByAccessProfileIds(
							Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetAdminBlanket()?.Select(x => x.Id)?.ToList())?.Select(x => x.UserId)?.ToList())?.Select(x => x.Email)?.ToList());
						}
						else
						{
							addresses.AddRange(userValidationEmail);
						}

						break;
					case Enums.RahmenAction.Valider:
						body = Template("confirm_validation", RahmenEntity, null, _user, redirect);
						title = $"Confirmation of Validation – Rahmen | Document No. {RahmenEntity.Bezug}";
						addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
						// - 2022-12-01 send Mail to Assigned Employee
						if(assignedEmployeeAddresses.Count > 0)
						{
							addresses.AddRange(assignedEmployeeAddresses);
						}
						break;
					case Enums.RahmenAction.Rejeter:
						body = Template("reject_validation", RahmenEntity, "rejected", _user, redirect);
						title = $"Cancellation Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
						addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
						// - 2022-12-01 send Mail to Assigned Employee
						if(assignedEmployeeAddresses.Count > 0)
						{
							addresses.AddRange(assignedEmployeeAddresses);
						}
						break;
					case Enums.RahmenAction.Fermer:
						body = Template("closing", RahmenEntity, null, _user, redirect);
						title = $"Closing Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
						addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
						// - 2022-12-01 send Mail to Assigned Employee
						if(assignedEmployeeAddresses.Count > 0)
						{
							addresses.AddRange(assignedEmployeeAddresses);
						}
						break;
					case Enums.RahmenAction.Annuler:
						body = Template("reject_validation", RahmenEntity, "cancelled", _user, redirect);
						title = $"Cancellation Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
						addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
						// - 2022-12-01 send Mail to Assigned Employee
						if(assignedEmployeeAddresses.Count > 0)
						{
							addresses.AddRange(assignedEmployeeAddresses);
						}
						break;
					default:
						break;
				}
				Module.EmailingService.SendEmailAsync(title, body, addresses, null, IsTable: true);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static string Template(string template, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity rahmen, string action, string username, Enums.RahmenRedirect redirect)
		{
			if(!string.IsNullOrEmpty(template))
			{
				string templatePath = @"Email\Templates\" + template + ".html";
				string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), templatePath);
				var link = $"{(redirect == Enums.RahmenRedirect.INS ? Module.INSEmailAppDomaineName : Module.MTMEmailAppDomaineName)}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/{(redirect == Enums.RahmenRedirect.INS ? "order-responses" : "rahmen/info")}/{rahmen.Nr}";
				if(File.Exists(path))
				{
					template = File.ReadAllText(path);
					template = template.Replace("{{date}}", DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US")));
					template = template.Replace("{{salutation}}", $"Good {(DateTime.Now.Hour <= 12 ? "Morning" : " Afternoon")}");
					template = template.Replace("{{username}}", username);
					template = template.Replace("{{action}}", action);
					template = template.Replace("{{Angebot_Nr}}", rahmen.Angebot_Nr.ToString());
					template = template.Replace("{{Projekt_Nr}}", rahmen.Projekt_Nr);
					template = template.Replace("{{Bezug}}", rahmen.Bezug);
					template = template.Replace("{{link}}", link);
				}

			}
			return template;
		}
		public static void SendConsumptionEmail(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity rahmenPos, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity rahmen, decimal percenatge)
		{
			var emailTitle = $"Rahmen position consumption notification";
			var emailBody = Template("rahmen_consumption", rahmen, rahmenPos, percenatge);
			// Send email notification
			var adressesEntities = Infrastructure.Data.Access.Tables.PRS.RahmenConsumptionNotificationMailAdressesAccess.Get();
			var adresses = adressesEntities.Select(x => x.Mail).ToList();
			Module.EmailingService.SendEmailAsync(
				emailTitle,
				emailBody,
				  adresses, null, null,
			   true, IsTable: true);
		}
		public static string Template(string template, AngeboteEntity rahmen, AngeboteneArtikelEntity rahmenPos, decimal consumption)
		{
			if(!string.IsNullOrEmpty(template))
			{
				string templatePath = @"Email\Templates\" + template + ".html";
				string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), templatePath);
				var link = $"{Module.PurchaseEmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/rahmen/info/{rahmen.Nr}";
				if(File.Exists(path))
				{
					template = File.ReadAllText(path);
					template = template.Replace("{{date}}", DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US")));
					template = template.Replace("{{salutation}}", $"Good {(DateTime.Now.Hour <= 12 ? "Morning" : " Afternoon")}");
					template = template.Replace("{{position}}", rahmenPos.Position.ToString());
					template = template.Replace("{{bezug}}", rahmen.Bezug);
					template = template.Replace("{{percentage}}", consumption.ToString());
					template = template.Replace("{{original}}", rahmenPos.OriginalAnzahl.ToString());
					template = template.Replace("{{ordred}}", rahmenPos.Geliefert.ToString());
					template = template.Replace("{{rest}}", rahmenPos.Anzahl.ToString());
					template = template.Replace("{{link}}", link);
				}

			}
			return template;
		}
	}
}
