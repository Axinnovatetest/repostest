using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class BookHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Reception.BookModel _data { get; set; }

		public BookHandler(Identity.Models.UserModel user, Models.Budget.Reception.BookModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.FNC);
			var botransactionDefault = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			try
			{
				botransaction.beginTransaction();
				botransactionDefault.beginTransaction();

				#region // -- transaction-based logic -- //
				//TODO: - insert process here
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var receivedItems = new List<KeyValuePair<decimal, string>>();

				// Save Reception
				var bestellungEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetWithTransaction(this._data.Nr, botransaction.connection, botransaction.transaction);

				bestellungEntity.Nr = -1;
				bestellungEntity.Typ = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.RECEPTION_TYPE;
				bestellungEntity.Datum = DateTime.Now;
				bestellungEntity.Liefertermin = DateTime.Now;
				bestellungEntity.gebucht = true; // >>>>>>>> Confirm
				bestellungEntity.gedruckt = false;
				bestellungEntity.erledigt = true;
				bestellungEntity.datueber = true;
				bestellungEntity.Benutzer = $"{this._user.Username} {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
				bestellungEntity.Eingangslieferscheinnr = this._data.DeliveryNoteNumber;
				bestellungEntity.Eingangsrechnungsnr = this._data.InvoiceNumber;
				bestellungEntity.Freitext = $"CF: {this._data.CFNumber}";
				bestellungEntity.best_id = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetByMaxBestellungNr( botransaction.connection, botransaction.transaction) + 1;
				var receptionId = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.InsertWithTransaction(bestellungEntity, botransaction.connection, botransaction.transaction);

				this._data.Articles = this._data.Articles ?? new List<Models.Budget.Reception.Article.UpdateModel>(); //

				var articleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetWithTransaction(this._data.Articles?.Select(x => x?.Nr ?? -1)?.ToList(), botransaction.connection, botransaction.transaction);
				var originalArticles = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>(articleEntities.Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity
				{
					AB_Nr_Lieferant = x.AB_Nr_Lieferant,
					Aktuelle_Anzahl = x.Aktuelle_Anzahl,
					AnfangLagerBestand = x.AnfangLagerBestand,
					Anzahl = x.Anzahl,
					Artikel_Nr = x.Artikel_Nr,
					Bemerkung_Pos = x.Bemerkung_Pos,
					Bemerkung_Pos_ID = x.Bemerkung_Pos_ID,
					Bestatigter_Termin = x.Bestatigter_Termin,
					Bestellnummer = x.Bestellnummer,
					Bestellung_Nr = x.Bestellung_Nr,
					Bezeichnung_1 = x.Bezeichnung_1,
					Bezeichnung_2 = x.Bezeichnung_2,
					BP_zu_RBposition = x.BP_zu_RBposition,
					COC_bestatigung = x.COC_bestatigung,
					CUPreis = x.CUPreis,
					Einheit = x.Einheit,
					Einzelpreis = x.Einzelpreis,
					EMPB_Bestatigung = x.EMPB_Bestatigung,
					EndeLagerBestand = x.EndeLagerBestand,
					Erhalten = x.Erhalten,
					erledigt_pos = x.erledigt_pos,
					Gesamtpreis = x.Gesamtpreis,
					In_Bearbeitung = x.In_Bearbeitung,
					InfoRahmennummer = x.InfoRahmennummer,
					Kanban = x.Kanban,
					Lagerort_id = x.Lagerort_id,
					Liefertermin = x.Liefertermin,
					Loschen = x.Loschen,
					MhdDatumArtikel = x.MhdDatumArtikel,
					Nr = x.Nr,
					Position = x.Position,
					Position_erledigt = x.Position_erledigt,
					Preiseinheit = x.Preiseinheit,
					Preisgruppe = x.Preisgruppe,
					Produktionsort = x.Produktionsort,
					Rabatt = x.Rabatt,
					Rabatt1 = x.Rabatt1,
					RB_Abgerufen = x.RB_Abgerufen,
					RB_Offen = x.RB_Offen,
					RB_OriginalAnzahl = x.RB_OriginalAnzahl,
					schriftart = x.schriftart,
					sortierung = x.sortierung,
					Start_Anzahl = x.Start_Anzahl,
					Umsatzsteuer = x.Umsatzsteuer,
					WE_Pos_zu_Bestellposition = x.WE_Pos_zu_Bestellposition,
				})); // copy list
				var lagerEntities = Infrastructure.Data.Access.Tables.FNC.LagerAccess.GetWithTransaction(articleEntities.Select(x => x.Lagerort_id ?? -1)?.ToList(), botransaction.connection, botransaction.transaction) ?? new List<Infrastructure.Data.Entities.Tables.FNC.LagerEntity>();

				var updatedArticles = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				var updatedLager = new List<Infrastructure.Data.Entities.Tables.FNC.LagerEntity>();
				foreach(var item in this._data.Articles)
				{
					var idx = articleEntities.FindIndex(x => x.Nr == item.Nr);
					if(idx >= 0)
					{
						if(item.ReceivingQuantiy > 0)
						{
							receivedItems.Add(new KeyValuePair<decimal, string>(item.ReceivingQuantiy ?? 0, articleEntities[idx].Bezeichnung_1));
							var art = articleEntities[idx];
							art.Bestellung_Nr = receptionId;
							art.Liefertermin = DateTime.Now;
							art.Lagerort_id = item.Lagerort_id;
							art.Anzahl = item.ReceivingQuantiy;
							//art.Aktuelle_Anzahl = item.Aktuelle_Anzahl;
							art.Gesamtpreis = item.ReceivingQuantiy * item.Einzelpreis / item.Preiseinheit;
							art.erledigt_pos = true;
							updatedArticles.Add(art);

							//-
							var idy = lagerEntities.FindIndex(y => y.ID == item.Lagerort_id);
							if(idy >= 0)
							{
								var lag = lagerEntities[idy];
								lag.Bestand += item.ReceivingQuantiy;
								lag.letzte_Bewegung = DateTime.Now;
								updatedLager.Add(lag);
							}
							// - originalArticles & articleEntities have the same elements & order
							originalArticles[idx].Anzahl = originalArticles[idx].Anzahl - item.ReceivingQuantiy;
							originalArticles[idx].Erhalten = originalArticles[idx].Erhalten + item.ReceivingQuantiy;
							originalArticles[idx].Gesamtpreis = originalArticles[idx].Anzahl * originalArticles[idx].Einzelpreis / originalArticles[idx].Preiseinheit;
							originalArticles[idx].erledigt_pos = originalArticles[idx].Anzahl <= 0; // --
							originalArticles[idx].Bemerkung_Pos = $"{originalArticles[idx].Bemerkung_Pos} Materialeingang: {originalArticles[idx].Aktuelle_Anzahl} {originalArticles[idx].Einheit} gebucht von: {this._user.Username} am: {DateTime.Now}"; // --
						}
					}
				}

				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.InsertWithTransaction(updatedArticles, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.FNC.LagerAccess.UpdateWithTransaction(updatedLager, botransaction.connection, botransaction.transaction);

				// Update Original Order
				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.UpdateWithTransaction(originalArticles, botransaction.connection, botransaction.transaction);
				if(originalArticles.TrueForAll(x => x.erledigt_pos == true))
				{
					Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.UpdateCompletionStatus(this._data.Nr, true, botransaction.connection, botransaction.transaction);
				}


				// - Save Invoice
				createInvoice(receptionId, articleEntities, _data.Articles, _user, botransaction);

				// - workflow history
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Nr);
				Helpers.Processings.Budget.Order.SaveOrderHistory(botransaction, botransactionDefault, orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Book, this._user, $"Booked {this._data.Articles.Where(x => x.ReceivingQuantiy > 0)?.Count()} positions");

				#endregion // -- transaction-based logic -- //

				// - two transactions!, normally use MSDTC - but this is not critical
				if(botransaction.commit() && botransactionDefault.commit())
				{
					// -
					if(receivedItems != null && receivedItems.Count > 0)
					{
						var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Nr);
						var originalOrderUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity?.IssuerId ?? -1);
						string emailTitle = "[Budget] Order Incoming Inspection", emailContent;
						var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderExtensionEntity?.ProjectId ?? -1);
						emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
						emailContent += $"<span style='font-size:1.5em'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")} {originalOrderUser?.Username},</span><br/>";
						emailContent += $"<br/><span style='font-size:1.15em'><strong>{this._user.Name?.ToUpper()}</strong> has just received {receivedItems?.Count} item(s) of the order <strong>{orderExtensionEntity?.OrderNumber?.ToUpper()}</strong>";


						if(projectEntity != null)
							emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";

						emailContent += $" that you have placed.</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders'>here</a>";
						emailContent += "<br/><br/>Regards, <br/>IT Department</div>";

						try
						{
							Module.EmailingService.SendEmailSendGridWithStaticTemplate(
								emailContent,
								emailTitle,
								new List<string> { originalOrderUser?.Email }, null, null,
							   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);

							// Send email notification
							/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { originalOrderUser?.Email }, null,
							saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);*/
						} catch(Exception ex)
						{
							Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", originalOrderUser?.Email)}]", ex));
						}
					}

					return ResponseModel<int>.SuccessResponse(1);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				botransactionDefault.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data.Nr) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Nr);
			if(orderExtensionEntity == null || orderExtensionEntity.ApprovalUserId <= 0)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not valaidated");

			//var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Nr);
			//if (orderExtensionEntity == null || orderExtensionEntity.ApprovalUserId <= 0)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: "Order not valaidated");

			if(string.IsNullOrWhiteSpace(this._data.DeliveryNoteNumber)
				&& string.IsNullOrWhiteSpace(this._data.InvoiceNumber)
				&& string.IsNullOrWhiteSpace(this._data.CFNumber))
				return ResponseModel<int>.FailureResponse(key: "1", value: "Data invalid. At least one field must be filled");

			//if (string.IsNullOrEmpty(this._data.DeliveryNoteNumber) || string.IsNullOrWhiteSpace(this._data.DeliveryNoteNumber))
			//    return ResponseModel<int>.FailureResponse(key: "1", value: "Delivery Note Number invalid");

			var errors = new List<string>();
			var articleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(this._data.Articles?.Select(x => x?.Nr ?? -1)?.ToList());
			foreach(var item in this._data.Articles)
			{
				var idx = articleEntities.FindIndex(x => x.Nr == item.Nr);
				if(idx >= 0)
				{
					//if (item.ReceivingQuantiy > 0 && item.ReceivingQuantiy > articleEntities[idx].Anzahl)
					//{
					//    errors.Add($"Position {articleEntities[idx].Position}: receiving quantity {item.ReceivingQuantiy} invalid");
					//}

					if(item.ReceivingQuantiy > 0 && item.ReceivingQuantiy <= articleEntities[idx].Anzahl)
					{
						if(item.Lagerort_id == null || item.Lagerort_id.Value <= 0)
						{
							errors.Add($"Position {articleEntities[idx].Position}: storage location invalid");
						}
					}
				}
			}

			if(errors.Count > 0)
				return ResponseModel<int>.FailureResponse(errors);

			return ResponseModel<int>.SuccessResponse();
		}
		public static void createInvoice(int bookingId,
			List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> artikelEntities,
			List<Models.Budget.Reception.Article.UpdateModel> articles, Identity.Models.UserModel _user, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			try
			{
				// Save Invoice
				var bookingEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(bookingId, botransaction.connection, botransaction.transaction, Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types.Booking);
				if(bookingEntity == null)
					return;

				var bookingExtEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(bookingEntity.Bestellung_Nr ?? -1, botransaction.connection, botransaction.transaction); // - Booking does NOT have an extension
				bookingEntity.Typ = "Rechnung";
				bookingExtEntity.CreationDate = DateTime.Now;

				// - 
				var invoice = getInvoice(bookingEntity, bookingExtEntity);
				invoice.Number = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetMaxNumber(DateTime.Now.Year, botransaction.connection, botransaction.transaction) + 1;
				invoice.Reference = $"{invoice.CreationDate.Value.ToString("yyyy")}/{invoice.Number.ToString("D6")}";
				invoice.CompanyId = replaceInvoicingSite(invoice);
				var invoiceId = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.InsertWithTransaction(invoice, botransaction.connection, botransaction.transaction);

				// - 
				List<Tuple<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity, Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>> results;
				var artikelExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(bookingEntity.Bestellung_Nr ?? -1, botransaction.connection, botransaction.transaction);
				if(Models.Budget.Reception.BookModel.ToInvoiceItems(invoiceId, articles, artikelEntities, artikelExtensionEntities, out results))
				{
					var invoiceItems = new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
					foreach(var item in results)
					{
						invoiceItems.Add(getInvoiceItem(invoiceId, item.Item1, item.Item2));
					}
					// -
					Infrastructure.Data.Access.Tables.FNC.InvoiceItemAccess.InsertWithTransaction(invoiceItems, botransaction.connection, botransaction.transaction);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static int replaceInvoicingSite(Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity invoiceEntity)
		{
			if(invoiceEntity == null)
			{
				return -1;
			}
			// - PSP or GZ
			if(invoiceEntity.CompanyName?.ToLower()?.Trim() == "psp" /*|| invoiceEntity.CompanyName?.ToLower()?.Trim() == "ghezala"*/)
			{
				return Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get().FirstOrDefault(x => x.Name?.ToLower()?.Contains("psz tunisie") == true)?.Id ?? -1;
			}
			// - BETN bill to WS // - 2024-01-31 // BETN does not exists from 01.01.2024
			if(invoiceEntity.CompanyName?.ToLower()?.Trim().Contains("betn tunisie")==true && invoiceEntity.CreationDate?.Year > 2023)
			{
				return Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get().FirstOrDefault(x => x.Name?.ToLower()?.Contains("ws tunisie") == true)?.Id ?? -1;
			}

			return invoiceEntity.CompanyId ?? -1;
		}
		static Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity getInvoice(Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungenEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity bestellungenExtensionEntity)
		{
			if(bestellungenEntity == null)
				return null;

			var invoice = new Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity();

			// - Invoices are created from Bookings
			invoice.OrderId = bestellungenEntity.Bestellung_Nr;
			invoice.BookingId = bestellungenEntity.Nr;

			invoice.AB_Nr_Lieferant = bestellungenEntity.AB_Nr_Lieferant;
			invoice.Abteilung = bestellungenEntity.Abteilung;
			invoice.Anfrage_Lieferfrist = bestellungenEntity.Anfrage_Lieferfrist;
			invoice.Anrede = bestellungenEntity.Anrede;
			invoice.Ansprechpartner = bestellungenEntity.Ansprechpartner;
			invoice.Bearbeiter = bestellungenEntity.Bearbeiter;
			invoice.Belegkreis = bestellungenEntity.Belegkreis;
			invoice.Bemerkungen = bestellungenEntity.Bemerkungen;
			invoice.Benutzer = bestellungenEntity.Benutzer;
			invoice.best_id = bestellungenEntity.best_id;
			//invoice.Bestellbestatigung_erbeten_bis = (DateTime?)bestellungenEntity.Bestellbestatigung_erbeten_bis;
			invoice.Bezug = bestellungenEntity.Bezug;
			invoice.Briefanrede = bestellungenEntity.Briefanrede;
			invoice.datueber = bestellungenEntity.datueber;
			invoice.Datum = bestellungenEntity.Datum;
			invoice.Eingangslieferscheinnr = bestellungenEntity.Eingangslieferscheinnr;
			invoice.Eingangsrechnungsnr = bestellungenEntity.Eingangsrechnungsnr;
			invoice.erledigt = bestellungenEntity.erledigt;
			invoice.Frachtfreigrenze = (decimal?)bestellungenEntity.Frachtfreigrenze;
			invoice.Freitext = bestellungenEntity.Freitext;
			invoice.gebucht = bestellungenEntity.gebucht;
			invoice.gedruckt = bestellungenEntity.gedruckt;
			invoice.Ihr_Zeichen = bestellungenEntity.Ihr_Zeichen;
			invoice.In_Bearbeitung = bestellungenEntity.In_Bearbeitung;
			invoice.Kanban = bestellungenEntity.Kanban;
			invoice.Konditionen = bestellungenEntity.Konditionen;
			invoice.Kreditorennummer = bestellungenEntity.Kreditorennummer;
			invoice.Kundenbestellung = bestellungenEntity.Kundenbestellung;
			invoice.Land_PLZ_Ort = bestellungenEntity.Land_PLZ_Ort;
			invoice.Lieferanten_Nr = bestellungenEntity.Lieferanten_Nr;
			invoice.Liefertermin = bestellungenEntity.Liefertermin;
			invoice.Loschen = bestellungenEntity.Loschen;
			invoice.Mahnung = bestellungenEntity.Mahnung;
			invoice.Mandant = bestellungenEntity.Mandant;
			invoice.Mindestbestellwert = (decimal?)bestellungenEntity.Mindestbestellwert;
			invoice.Name2 = bestellungenEntity.Name2;
			invoice.Name3 = bestellungenEntity.Name3;
			invoice.Neu = bestellungenEntity.Neu;
			invoice.nr_anf = bestellungenEntity.nr_anf;
			invoice.nr_bes = bestellungenEntity.nr_bes;
			invoice.nr_gut = bestellungenEntity.nr_gut;
			invoice.nr_RB = bestellungenEntity.nr_RB;
			invoice.nr_sto = bestellungenEntity.nr_sto;
			invoice.nr_war = bestellungenEntity.nr_war;
			invoice.Offnen = bestellungenEntity.Offnen;
			invoice.Personal_Nr = bestellungenEntity.Personal_Nr;
			invoice.Projekt_Nr = bestellungenEntity.Projekt_Nr;
			invoice.Rabatt = bestellungenEntity.Rabatt;
			invoice.Rahmenbestellung = bestellungenEntity.Rahmenbestellung;
			invoice.Straße_Postfach = bestellungenEntity.Straße_Postfach;
			invoice.Typ = bestellungenEntity.Typ;
			invoice.Unser_Zeichen = bestellungenEntity.Unser_Zeichen;
			invoice.USt = (decimal?)bestellungenEntity.USt;
			invoice.Versandart = bestellungenEntity.Versandart;
			invoice.Vorname_NameFirma = bestellungenEntity.Vorname_NameFirma;
			invoice.Wahrung = bestellungenEntity.Wahrung;
			invoice.Zahlungsweise = bestellungenEntity.Zahlungsweise;
			invoice.Zahlungsziel = bestellungenEntity.Zahlungsziel;

			// -  Extension fields
			if(bestellungenEntity != null)
			{
				invoice.ApprovalTime = bestellungenExtensionEntity.ApprovalTime;
				invoice.ApprovalUserId = bestellungenExtensionEntity.ApprovalUserId;
				invoice.Archived = bestellungenExtensionEntity.Archived;
				invoice.ArchiveTime = bestellungenExtensionEntity.ArchiveTime;
				invoice.ArchiveUserId = bestellungenExtensionEntity.ArchiveUserId;
				invoice.BillingAddress = bestellungenExtensionEntity.BillingAddress;
				invoice.BillingCompanyId = bestellungenExtensionEntity.BillingCompanyId;
				invoice.BillingCompanyName = bestellungenExtensionEntity.BillingCompanyName;
				invoice.BillingContactName = bestellungenExtensionEntity.BillingContactName;
				invoice.BillingDepartmentId = bestellungenExtensionEntity.BillingDepartmentId;
				invoice.BillingDepartmentName = bestellungenExtensionEntity.BillingDepartmentName;
				invoice.BillingFax = bestellungenExtensionEntity.BillingFax;
				invoice.BillingTelephone = bestellungenExtensionEntity.BillingTelephone;
				invoice.BudgetYear = bestellungenExtensionEntity.BudgetYear;
				invoice.CompanyId = bestellungenExtensionEntity.CompanyId;
				invoice.CompanyName = bestellungenExtensionEntity.CompanyName;
				invoice.CreationDate = bestellungenExtensionEntity.CreationDate;
				invoice.CurrencyId = bestellungenExtensionEntity.CurrencyId;
				invoice.CurrencyName = bestellungenExtensionEntity.CurrencyName;
				invoice.DefaultCurrencyDecimals = bestellungenExtensionEntity.DefaultCurrencyDecimals;
				invoice.DefaultCurrencyId = bestellungenExtensionEntity.DefaultCurrencyId;
				invoice.DefaultCurrencyName = bestellungenExtensionEntity.DefaultCurrencyName;
				invoice.DefaultCurrencyRate = bestellungenExtensionEntity.DefaultCurrencyRate;
				invoice.Deleted = bestellungenExtensionEntity.Deleted;
				invoice.DeleteTime = bestellungenExtensionEntity.DeleteTime;
				invoice.DeleteUserId = bestellungenExtensionEntity.DeleteUserId;
				invoice.DeliveryAddress = bestellungenExtensionEntity.DeliveryAddress;
				invoice.DeliveryCompanyId = bestellungenExtensionEntity.DeliveryCompanyId;
				invoice.DeliveryCompanyName = bestellungenExtensionEntity.DeliveryCompanyName;
				invoice.DeliveryDepartmentId = bestellungenExtensionEntity.DeliveryDepartmentId;
				invoice.DeliveryDepartmentName = bestellungenExtensionEntity.DeliveryDepartmentName;
				invoice.DeliveryFax = bestellungenExtensionEntity.DeliveryFax;
				invoice.DeliveryTelephone = bestellungenExtensionEntity.DeliveryTelephone;
				invoice.DepartmentId = bestellungenExtensionEntity.DepartmentId;
				invoice.DepartmentName = bestellungenExtensionEntity.DepartmentName;
				invoice.Description = bestellungenExtensionEntity.Description;
				invoice.Discount = bestellungenExtensionEntity.Discount;
				invoice.InternalContact = bestellungenExtensionEntity.InternalContact;
				invoice.IssuerId = bestellungenExtensionEntity.IssuerId;
				invoice.IssuerName = bestellungenExtensionEntity.IssuerName;
				invoice.LastRejectionLevel = bestellungenExtensionEntity.LastRejectionLevel;
				invoice.LastRejectionTime = bestellungenExtensionEntity.LastRejectionTime;
				invoice.LastRejectionUserId = bestellungenExtensionEntity.LastRejectionUserId;
				invoice.LeasingMonthAmount = bestellungenExtensionEntity.LeasingMonthAmount;
				invoice.LeasingNbMonths = bestellungenExtensionEntity.LeasingNbMonths;
				invoice.LeasingStartMonth = bestellungenExtensionEntity.LeasingStartMonth;
				invoice.LeasingStartYear = bestellungenExtensionEntity.LeasingStartYear;
				invoice.LeasingTotalAmount = bestellungenExtensionEntity.LeasingTotalAmount;
				invoice.Level = bestellungenExtensionEntity.Level;
				invoice.LocationId = bestellungenExtensionEntity.LocationId;
				invoice.MandantId = bestellungenExtensionEntity.MandantId;
				invoice.OrderNumber = bestellungenExtensionEntity.OrderNumber;
				invoice.OrderPlacedEmailMessage = bestellungenExtensionEntity.OrderPlacedEmailMessage;
				invoice.OrderPlacedEmailTitle = bestellungenExtensionEntity.OrderPlacedEmailTitle;
				invoice.OrderPlacedReportFileId = bestellungenExtensionEntity.OrderPlacedReportFileId;
				invoice.OrderPlacedSendingEmail = bestellungenExtensionEntity.OrderPlacedSendingEmail;
				invoice.OrderPlacedSupplierEmail = bestellungenExtensionEntity.OrderPlacedSupplierEmail;
				invoice.OrderPlacedTime = bestellungenExtensionEntity.OrderPlacedTime;
				invoice.OrderPlacedUserEmail = bestellungenExtensionEntity.OrderPlacedUserEmail;
				invoice.OrderPlacedUserId = bestellungenExtensionEntity.OrderPlacedUserId;
				invoice.OrderPlacedUserName = bestellungenExtensionEntity.OrderPlacedUserName;
				invoice.OrderPlacementCCEmail = bestellungenExtensionEntity.OrderPlacementCCEmail;
				invoice.OrderType = bestellungenExtensionEntity.OrderType;
				invoice.PoPaymentType = bestellungenExtensionEntity.PoPaymentType;
				invoice.PoPaymentTypeName = bestellungenExtensionEntity.PoPaymentTypeName;
				invoice.ProjectId = bestellungenExtensionEntity.ProjectId;
				invoice.ProjectName = bestellungenExtensionEntity.ProjectName;
				invoice.Status = bestellungenExtensionEntity.Status;
				invoice.StorageLocationId = bestellungenExtensionEntity.StorageLocationId;
				invoice.StorageLocationName = bestellungenExtensionEntity.StorageLocationName;
				invoice.SupplierEmail = bestellungenExtensionEntity.SupplierEmail;
				invoice.SupplierFax = bestellungenExtensionEntity.SupplierFax;
				invoice.SupplierId = bestellungenExtensionEntity.SupplierId;
				invoice.SupplierName = bestellungenExtensionEntity.SupplierName;
				invoice.SupplierNumber = bestellungenExtensionEntity.SupplierNumber;
				invoice.SupplierNummer = bestellungenExtensionEntity.SupplierNummer;
				invoice.SupplierPaymentMethod = bestellungenExtensionEntity.SupplierPaymentMethod;
				invoice.SupplierPaymentTerm = bestellungenExtensionEntity.SupplierPaymentTerm;
				invoice.SupplierTelephone = bestellungenExtensionEntity.SupplierTelephone;
				invoice.SupplierTradingTerm = bestellungenExtensionEntity.SupplierTradingTerm;
				invoice.ValidationRequestTime = bestellungenExtensionEntity.ValidationRequestTime;
			}
			// -
			return invoice;
		}
		static Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity getInvoiceItem(
			int invoiceId,
			Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity extensionEntity)
		{
			if(artikelEntity == null)
				return null;

			var invoiceItem = new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity();
			invoiceItem.InvoiceId = invoiceId;

			invoiceItem.AB_Nr_Lieferant = artikelEntity.AB_Nr_Lieferant;
			invoiceItem.AnfangLagerBestand = artikelEntity.AnfangLagerBestand;
			invoiceItem.Anzahl = artikelEntity.Anzahl;
			invoiceItem.Artikel_Nr = artikelEntity.Artikel_Nr;
			invoiceItem.Bemerkung_Pos = artikelEntity.Bemerkung_Pos;
			invoiceItem.Bemerkung_Pos_ID = artikelEntity.Bemerkung_Pos_ID;
			invoiceItem.Bestatigter_Termin = artikelEntity.Bestatigter_Termin;
			invoiceItem.Bestellnummer = artikelEntity.Bestellnummer;
			invoiceItem.OrderId = artikelEntity.Bestellung_Nr ?? -1;
			invoiceItem.Bezeichnung_1 = artikelEntity.Bezeichnung_1;
			invoiceItem.Bezeichnung_2 = artikelEntity.Bezeichnung_2;
			invoiceItem.BP_zu_RBposition = artikelEntity.BP_zu_RBposition;
			invoiceItem.COC_bestätigung = artikelEntity.COC_bestatigung;
			invoiceItem.CUPreis = artikelEntity.CUPreis;
			invoiceItem.Einheit = artikelEntity.Einheit;
			invoiceItem.Einzelpreis = artikelEntity.Einzelpreis;
			invoiceItem.EMPB_Bestätigung = artikelEntity.EMPB_Bestatigung;
			invoiceItem.EndeLagerBestand = artikelEntity.EndeLagerBestand;
			invoiceItem.Erhalten = artikelEntity.Erhalten;
			invoiceItem.erledigt_pos = artikelEntity.erledigt_pos;
			invoiceItem.Gesamtpreis = artikelEntity.Gesamtpreis;
			invoiceItem.In_Bearbeitung = artikelEntity.In_Bearbeitung;
			invoiceItem.InfoRahmennummer = artikelEntity.InfoRahmennummer;
			invoiceItem.Kanban = artikelEntity.Kanban;
			invoiceItem.Lagerort_id = artikelEntity.Lagerort_id;
			invoiceItem.Liefertermin = artikelEntity.Liefertermin;
			invoiceItem.Löschen = artikelEntity.Loschen;
			invoiceItem.MhdDatumArtikel = artikelEntity.MhdDatumArtikel;
			invoiceItem.Position = artikelEntity.Position;
			invoiceItem.Position_erledigt = artikelEntity.Position_erledigt;
			invoiceItem.Preiseinheit = artikelEntity.Preiseinheit;
			invoiceItem.Preisgruppe = artikelEntity.Preisgruppe;
			invoiceItem.Produktionsort = artikelEntity.Produktionsort;
			invoiceItem.Rabatt = artikelEntity.Rabatt;
			invoiceItem.Rabatt1 = artikelEntity.Rabatt1;
			invoiceItem.RB_Abgerufen = artikelEntity.RB_Abgerufen;
			invoiceItem.RB_Offen = artikelEntity.RB_Offen;
			invoiceItem.RB_OriginalAnzahl = artikelEntity.RB_OriginalAnzahl;
			invoiceItem.schriftart = artikelEntity.schriftart;
			invoiceItem.sortierung = artikelEntity.sortierung;
			invoiceItem.Start_Anzahl = artikelEntity.Start_Anzahl;
			invoiceItem.Umsatzsteuer = (double?)artikelEntity.Umsatzsteuer;
			invoiceItem.WE_Pos_zu_Bestellposition = artikelEntity.WE_Pos_zu_Bestellposition;

			// - Extension fields
			if(extensionEntity != null)
			{
				invoiceItem.AccountId = extensionEntity.AccountId;
				invoiceItem.AccountName = extensionEntity.AccountName;
				invoiceItem.ArticleId = extensionEntity.ArticleId;
				invoiceItem.ConfirmationDate = extensionEntity.ConfirmationDate;
				invoiceItem.CurrencyId = extensionEntity.CurrencyId;
				invoiceItem.CurrencyName = extensionEntity.CurrencyName;
				invoiceItem.DefaultCurrencyDecimals = extensionEntity.DefaultCurrencyDecimals;
				invoiceItem.DefaultCurrencyId = extensionEntity.DefaultCurrencyId;
				invoiceItem.DefaultCurrencyName = extensionEntity.DefaultCurrencyName;
				invoiceItem.DefaultCurrencyRate = extensionEntity.DefaultCurrencyRate;
				invoiceItem.DeliveryDate = extensionEntity.DeliveryDate;
				invoiceItem.Description = extensionEntity.Description;
				invoiceItem.Discount = extensionEntity.Discount;
				invoiceItem.InternalContact = extensionEntity.InternalContact;
				invoiceItem.LocationId = extensionEntity.LocationId;
				invoiceItem.LocationName = extensionEntity.LocationName;
				//invoiceItem.OrderId = extensionEntity.OrderId;
				invoiceItem.Quantity = extensionEntity.Quantity;
				invoiceItem.SupplierDeliveryDate = extensionEntity.SupplierDeliveryDate;
				invoiceItem.SupplierOrderNumber = extensionEntity.SupplierOrderNumber;
				invoiceItem.TotalCost = extensionEntity.TotalCost;
				invoiceItem.TotalCostDefaultCurrency = extensionEntity.TotalCostDefaultCurrency;
				invoiceItem.UnitPrice = extensionEntity.UnitPrice;
				invoiceItem.UnitPriceDefaultCurrency = extensionEntity.UnitPriceDefaultCurrency;
				invoiceItem.VAT = extensionEntity.VAT;
			}

			// -
			return invoiceItem;
		}
	}
}
