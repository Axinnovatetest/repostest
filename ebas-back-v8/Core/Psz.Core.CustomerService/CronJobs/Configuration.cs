using Psz.Core.CustomerService.Handlers.E_Rechnung;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CronJobs
{
	public class Configuration
	{
		public static void NotifyExpiredNearlyRahmens()
		{
			var neralryExpiredpositions = GetRahmen_2MonthsNotice();
			if(neralryExpiredpositions != null && Module.EmailingService.EmailParamtersModel.RahmenExpiryEmailDestinations != null
				&& Module.EmailingService.EmailParamtersModel.RahmenExpiryEmailDestinations.Count > 0)
			{
				var rahmensNrs = neralryExpiredpositions.Select(x => x.RahmenNr).Distinct().ToList();
				var rahmenEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(rahmensNrs);
				var positionentities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(neralryExpiredpositions.Select(x => x.AngeboteArtikelNr).ToList());
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(positionentities?.Select(x => x.ArtikelNr ?? -1)?.ToList());
				var title = $"Rahmen expiration";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.15em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1em;'>The following Purchase Rahmen include positions that are set to expire in approximately <strong>two months or earlier</strong>."
				+ $"</span><br/><br/>"
				+ "</div>";
				content += $"<hr>";
				// Add table header
				content += "<table border='1' cellpadding='2' cellswpacing='0' style='border-collapse:collapse; font-family:Arial, sans-serif; font-size:14px; width:100%; max-width:100%;'>";
				content += "<thead style='background-color:#f2f2f2; font-weight:bold;'>";
				content += @"<tr><th style=""min-width:120px;"">RA-Nr</th>";
				content += @"<th style=""min-width:120px;"">Projekt-Nr</th>";
				content += @"<th style=""min-width:120px;"">Lieferant</th>";
				content += @"<th style=""min-width:120px;"">Position</th>";
				content += @"<th style=""min-width:120px;"">Artikel</th>";
				content += @"<th style=""min-width:120px;"">Bezeichnung</th>";
				content += @"<th style=""min-width:120px;"">Gesamtmenge</th>";
				content += @"<th style=""min-width:120px;"">Restmenge</th>";
				content += @"<th style=""min -width:120px;"">Gultig bis</th>";
				content += @"</tr></thead>";
				content += "<tbody>";
				foreach(var item in rahmensNrs)
				{
					var rahmenEntity = rahmenEntities.FirstOrDefault(x => x.Nr == item);
					var rahmenPositions = neralryExpiredpositions.Where(x => x.RahmenNr == item).ToList();
					var raLink = $"{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/order-responses/{rahmenEntity.Nr}";
					foreach(var pos in rahmenPositions.OrderBy(x => x.AngeboteArtikelNr))
					{
						content += "<tr>";
						content += @$"<td style=""min-width:120px;text-align:center;""><a href='{raLink}'>{rahmenEntity.Angebot_Nr}</a></td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{rahmenEntity.Projekt_Nr}</td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{rahmenEntity.Vorname_NameFirma}</td>";
						var positionentity = positionentities.FirstOrDefault(x => x.Nr == pos.AngeboteArtikelNr);
						var articleEntity = articleEntities.FirstOrDefault(x => x.ArtikelNr == positionentity.ArtikelNr);
						content += @$"<td style=""min-width:120px;text-align:center;"">{positionentity?.Position}</td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{articleEntity?.ArtikelNummer}</td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{positionentity?.Bezeichnung1}</td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{positionentity.OriginalAnzahl}</td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{positionentity.Anzahl}</td>";
						content += @$"<td style=""min-width:120px;text-align:center;"">{pos?.GultigBis?.ToString("dd/MM/yyyy")}</td>";
						content += "</tr>";
					}
				}
				content += "<tbody></table>";
				content += "<br/><br/>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";

				try
				{
					Module.EmailingService.SendEmailAsync(title, content, Module.EmailingService.EmailParamtersModel.RahmenExpiryEmailDestinations, null);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email NotifyExpiredNearlyRahmens"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}

			static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity> GetRahmen_2MonthsNotice()
			{
				var _2monthsExpiryRahmensPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Get2MonthsPriorExpiry();

				return _2monthsExpiryRahmensPositions;
			}
		}
		public static async Task<InvoicesCreationSummaryModel> CreateInvoices(List<string> errors, UserModel _user)
		{
			var createdInvoices = new List<CreatedInvoiceModels.CreatedInvoiceModel>();
			var creationSummary = new InvoicesCreationSummaryModel();

			try
			{
				errors = errors ?? new List<string>();
				var einzelKunden = Infrastructure.Data.Access.Joins.CTS.Divers.GetKundenListForERechnung((int)Enums.E_RechnungEnums.RechnungTyp.Einzelrechnung);
				var sammelKunden = Infrastructure.Data.Access.Joins.CTS.Divers.GetKundenListForERechnung((int)Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung);
				var SonstigesKunden = Infrastructure.Data.Access.Joins.CTS.Divers.GetKundenListForERechnung((int)Enums.E_RechnungEnums.RechnungTyp.Sonderregelung);

				var resulteinzelKunden = await HandleInvoiceCreation(einzelKunden, Enums.E_RechnungEnums.RechnungTyp.Einzelrechnung, errors, _user);
				var resultsammelKunden = await HandleInvoiceCreation(sammelKunden, Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung, errors, _user, IsSammel: true);
				var resultSonstigesKunden = await HandleInvoiceCreation(SonstigesKunden, Enums.E_RechnungEnums.RechnungTyp.Sonderregelung, errors, _user);

				if(resulteinzelKunden is not null && resulteinzelKunden.Item2 is not null)
				{
					creationSummary.LseinzelTotalCount = resulteinzelKunden.Item2.LsTotalCount;
					creationSummary.CreatedeinzelInvoicesCount = resulteinzelKunden.Item2.CreatedInvoicesCount;
				}
				if(resultsammelKunden is not null && resultsammelKunden.Item2 is not null)
				{
					creationSummary.LssammelTotalCount = resultsammelKunden.Item2.LsTotalCount;
					creationSummary.CreatedsammelInvoicesCount = resultsammelKunden.Item2.CreatedInvoicesCount;
				}
				if(resultSonstigesKunden is not null && resultSonstigesKunden.Item2 is not null)
				{
					creationSummary.LsSonstigesTotalCount = resultSonstigesKunden.Item2.LsTotalCount;
					creationSummary.CreatedSonstigesInvoicesCount = resultSonstigesKunden.Item2.CreatedInvoicesCount;
				}
				if(resulteinzelKunden.Item1 is not null && resulteinzelKunden.Item1.Count > 0)
					createdInvoices = createdInvoices.Concat(resulteinzelKunden.Item1).ToList();
				if(resultsammelKunden.Item1 is not null && resultsammelKunden.Item1.Count > 0)
					createdInvoices = createdInvoices.Concat(resultsammelKunden.Item1).ToList();
				if(resultSonstigesKunden.Item1 is not null && resultSonstigesKunden.Item1.Count > 0)
					createdInvoices = createdInvoices.Concat(resultSonstigesKunden.Item1).ToList();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			} finally
			{

				var emails = new List<string>(Module.CTS.InvoiceAdminEmails)
						{
							_user.Email
						}.Distinct().ToList();

				if(createdInvoices.Count > 0 || errors.Count > 0)
				{
					var createdFormatted = new GetERechnungCreatedByIdsHandler(new Core.Identity.Models.UserModel { Id = -1, Username = "background-system" }, createdInvoices.Select(x => x.RgNr)?.ToList()).Handle();
					if(createdFormatted is not null && createdFormatted.Success && createdFormatted.Body is not null && createdFormatted.Body.Created?.Count > 0)
					{
						for(int i = 0; i < createdInvoices.Count; i++)
						{
							var t = createdFormatted.Body.Created.FirstOrDefault(x => x.ForfallNr == createdInvoices[i].RgNumber);
							if(t is not null)
							{
								createdInvoices[i].TypeInvoice = t.Type;
								createdInvoices[i].Amount = t.Betreg ?? 0;
								createdInvoices[i].SammelItems = t.SammelList?.Select(x => new CreatedInvoiceModels.CreatedInvoiceModel.SammelItem { LSForfallNr = x.LSForfallNr, LSNr = x.LSNr, RechnungProjectNr = x.RechnungProjectNr })?.ToList();
							}
						}
					}
					var attachementXLS = Infrastructure.Services.Email.Helpers.GetExcelFile(createdInvoices);
					if(emails?.Count > 0)
					{
						await Module.EmailingService.SendEmailAsync
										(
										"Created Invoices",
										Infrastructure.Services.Email.Helpers.GetInvoiceCreationSummaryContent(createdInvoices, errors),
										emails,
										attachments: new List<KeyValuePair<string, System.IO.Stream>> { new KeyValuePair<string, System.IO.Stream>("data.xlsx", new MemoryStream(attachementXLS)) },
										toAddressesCC: null,
										saveHistory: true, senderEmail: Module.EmailingService.GetMainAdress(),
										senderName: "",
										senderId: -2, senderCC: false,
										attachmentIds: null,
										IsTable: true
										);
					}
				}
				else
				{
					var emailContent = $"This is an info email of invoice processing for {DateTime.Today.ToString("dd.MM.yyyy")}.<br/>There has been no invoice created today!<br/>";
					await Module.EmailingService.SendEmailAsync
										(
										$"[Invoices Info] Processing of {DateTime.Today.ToString("dd.MM.yyyy")}",
										emailContent,
										emails,
										attachments: null,
										toAddressesCC: null,
										saveHistory: true, senderEmail: Module.EmailingService.GetMainAdress(),
										senderName: "",
										senderId: -2, senderCC: false,
										attachmentIds: null,
										IsTable: true
										);
				}
				// - 2023-10-31
				afterMathChecks(emails);
			}

			// -
			return creationSummary;
		}
		public static async Task<Tuple<List<CreatedInvoiceModels.CreatedInvoiceModel>, InvoicesCreationModel>> HandleInvoiceCreation(List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity> customersList, Enums.E_RechnungEnums.RechnungTyp type, List<string> errors, UserModel _user, bool IsSammel = false)
		{
			var createdInvoices = new List<CreatedInvoiceModels.CreatedInvoiceModel>();
			var unitSummary = new InvoicesCreationModel();
			int count = 0;
			errors = errors ?? new List<string>();
			if(customersList != null && customersList.Count > 0)
			{
				var created = new List<E_RechnungAutoCreationModel>();
				var genericUser = new Core.Identity.Models.UserModel { Id = -1, Username = "background-system" };
				customersList.ForEach(x =>
				{
					var errorsSo = new List<string>();
					created.AddRange(Helpers.E_RechnungHelper.CreateManualRechnung(type, x.Nr, genericUser, out errorsSo, out var _count, true) ?? new List<E_RechnungAutoCreationModel> { });

					errors.AddRange(errorsSo ?? new List<string> { });
					count = count + _count;
				});
				if(created != null && created.Count > 0)
				{
					var angebotNrs = created.Select(x => x.ForfallNr).ToList();
					Helpers.E_RechnungHelper.archiveRechnung(angebotNrs);
					created.ForEach(x =>
					{
						Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity
						{
							CreationTime = DateTime.Now,
							CustomerNr = x.CustomerNr,
							CustomerName = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(x.CustomerNr)?.Name1,
							CustomerRechnungType = type.GetDescription(),
							LsAngebotNr = x.LsAngebotNr,
							LSNr = x.LSNr,
							RechnungForfallNr = x.ForfallNr,
							RechnungNr = x.Nr,
							RechnungProjectNr = x.ProjectNr,
						});
					});

					if(IsSammel)
					{
						created = created.GroupBy(item => item.ForfallNr).Select(group => group.First()).ToList();
					}

					foreach(var item in created)
					{
						try
						{
							var result = await new SendRechnungEmailSubHandler(_user, item.Nr).HandleAsync();
							createdInvoices.Add(new CreatedInvoiceModels.CreatedInvoiceModel() { Sent = result.Body.Sent, ForfallNr = result.Body.Subject, Customer = item.Customer, CustomerNumber = item.CustomerNumber, Amount = 0, LsNumber = item.LsAngebotNr, RgNr = item.Nr, RgNumber = item.ForfallNr });
						} catch(Exception e)
						{
							Infrastructure.Services.Logging.Logger.Log(e);
						}
					}
				}

				unitSummary.LsTotalCount = count;

				if(created is not null && created.Count > 0)
					unitSummary.CreatedInvoicesCount = created.Count;
				else
					unitSummary.CreatedInvoicesCount = 0;

				if(errors != null && errors.Count > 0)
				{
					errors.ForEach(x =>
					{
						Infrastructure.Data.Access.Tables.CTS.__E_rechnung_ErrorsAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_ErrorsEntity
						{
							Error = x,
							ErrorTime = DateTime.Now,
						});
					});
				}
			}
			return new Tuple<List<CreatedInvoiceModels.CreatedInvoiceModel>, InvoicesCreationModel>(createdInvoices, unitSummary);

		}
		internal static async void afterMathChecks(List<string> emails)
		{
			var missedLS = false;
			var missedRG = false;
			var emailContent = "";

			// - check if there are LS eligible for RG and w/o RG
			var lsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetOpenDeliveriesForRechnung();
			if(lsEntities?.Count > 0)
			{
				var lsWoRgEnities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveriesWoRechnung(lsEntities?.Select(x => x.Nr));
				if(lsWoRgEnities?.Count > 0)
				{
					missedLS = true;
					emailContent += $@"<br/><span style='font-size:1.15em;'>LS without corresponding invoices (count {lsWoRgEnities.Count}).</span>
										<table style='width: 100%; border-collapse: collapse;'>
										<tr>
											<th style='border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;'>Customer</th>
											<th style='border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;'>LS-Nummer</th>
										</tr>";
					lsWoRgEnities.ForEach(x => { emailContent += $"<tr><td style='border: 1px solid #000; padding: 1px;'>{x.Vorname_NameFirma}</td><td style='border: 1px solid #000; padding: 1px;'>{x.Angebot_Nr}</td></tr>"; });
					emailContent += $"</table><br />";
				}
			}

			// - check if there RG created but not sent
			var rgTodayEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetInvoices(DateTime.Today, DateTime.Today, true, true);
			if(rgTodayEntities?.Count > 0)
			{
				var rgTodayNotSentEntities = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetNotSent(rgTodayEntities.Select(x => x.Nr));
				if(rgTodayNotSentEntities?.Count > 0)
				{
					missedRG = true;
					emailContent += $@"<br/><span style='font-size:1.15em;'>Not sent invoices (count {rgTodayNotSentEntities.Count}).</span>
										<table style='width: 100%; border-collapse: collapse;'>
										<tr>
											<th style='border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;'>Customer</th>
											<th style='border: 1px solid #000; padding: 1px; text-align: left; background-color: #f2f2f2;'>RG-Nummer</th>
										</tr>";
					rgTodayNotSentEntities.ForEach(x => { emailContent += $"<tr><td style='border: 1px solid #000; padding: 1px;'>{x.CustomerName}</td><td style='border: 1px solid #000; padding: 1px;'>{x.RechnungForfallNr}</td></tr>"; });
					emailContent += $"</table><br />";
				}
			}

			// - 
			if(missedLS || missedRG)
			{
				// - send alert
				emailContent = $"This is an alert email of invoice processing for {DateTime.Today.ToString("dd.MM.yyyy")}.<br/>The following failures are found, please consider checking the system to eventually fix them.<br/>"
					+ emailContent;

				if(emails?.Count > 0)
				{
					await Module.EmailingService.SendEmailAsync
										(
										$"[Invoices Alert] Processing of {DateTime.Today.ToString("dd.MM.yyyy")}",
										emailContent,
										emails,
										attachments: null,
										toAddressesCC: null,
										saveHistory: true, senderEmail: Module.EmailingService.GetMainAdress(),
										senderName: "",
										senderId: -2, senderCC: false,
										attachmentIds: null,
										IsTable: true
										);
				}
				System.IO.File.WriteAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"Logs/{DateTime.Now.ToString("yyyyMMddTHHmmfff")}-ERechnungEmail.log"), emailContent);
			}
		}
	}
}