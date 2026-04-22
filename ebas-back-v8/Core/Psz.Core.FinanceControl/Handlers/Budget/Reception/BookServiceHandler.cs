using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class BookServiceHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Reception.BookModel _data { get; set; }

		public BookServiceHandler(Identity.Models.UserModel user, Models.Budget.Reception.BookModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.FNC);
			var botransactionDefault = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();
				botransactionDefault.beginTransaction();

				#region // -- transaction-based logic -- //

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
				bestellungEntity.best_id = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetByMaxBestellungNr(botransaction.connection, botransaction.transaction) + 1;
				var receptionId = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.InsertWithTransaction(bestellungEntity, botransaction.connection, botransaction.transaction);

				this._data.Articles = this._data.Articles ?? new List<Models.Budget.Reception.Article.UpdateModel>(); //

				// - 2022-07-18 - allow bokk w/o sending articles. Take all articles form original order
				if(this._data.Articles != null && this._data.Articles.Count <= 0)
				{
					this._data.Articles = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data.Nr, botransaction.connection, botransaction.transaction)
						?.Select(x => new Models.Budget.Reception.Article.UpdateModel(x))
						?.ToList();
				}

				var articleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetWithTransaction(this._data.Articles?.Select(x => x?.Nr ?? -1)
				?.ToList(), botransaction.connection, botransaction.transaction)?.Where(x => x.Anzahl > 0)?.ToList(); // - 2022-06-28 take only items not already booked
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

				var updatedArticles = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				for(int i = 0; i < originalArticles.Count; i++)
				{
					receivedItems.Add(new KeyValuePair<decimal, string>(originalArticles[i].Anzahl ?? 0, originalArticles[i].Bezeichnung_1));

					var art = originalArticles[i].SelfCopy();
					art.Bestellung_Nr = receptionId;
					art.Liefertermin = DateTime.Now;
					art.Lagerort_id = -1;
					art.Gesamtpreis = originalArticles[i].Gesamtpreis;
					art.erledigt_pos = true;
					updatedArticles.Add(art);

					// - originalArticles
					originalArticles[i].Erhalten = (articleEntities[i].Anzahl ?? 0) + (articleEntities[i].Erhalten ?? 0);
					originalArticles[i].Anzahl = 0;
					originalArticles[i].Gesamtpreis = 0;
					originalArticles[i].erledigt_pos = true;
					originalArticles[i].Bemerkung_Pos = $"{originalArticles[i].Bemerkung_Pos} Materialeingang: {originalArticles[i].Aktuelle_Anzahl} {originalArticles[i].Einheit} gebucht von: {this._user.Username} am: {DateTime.Now}"; // --
				}

				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.InsertWithTransaction(updatedArticles, botransaction.connection, botransaction.transaction);

				// Update Original Order
				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.UpdateWithTransaction(originalArticles, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.UpdateCompletionStatus(this._data.Nr, true, botransaction.connection, botransaction.transaction);



				// - Save Invoice
				#region >>>> Invoice >>>>
				if(_data != null && _data.Articles != null && _data.Articles.Count > 0)
				{
					for(int i = 0; i < _data.Articles.Count; i++)
					{
						_data.Articles[i].ReceivingQuantiy = _data.Articles[i].Anzahl;
					}
				}
				BookHandler.createInvoice(receptionId, articleEntities, _data.Articles, _user, botransaction);
				#endregion >>>> Invoice

				// - workflow history
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Nr, botransaction.connection, botransaction.transaction);
				Helpers.Processings.Budget.Order.SaveOrderHistory(botransaction, botransactionDefault, orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Book, this._user, $"SoftBooked {originalArticles.Count} positions");
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
							// Send email notification

							Module.EmailingService.SendEmailSendGridWithStaticTemplate(emailContent, emailTitle,
								new List<string> { originalOrderUser?.Email }, null, null,
							   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);


							/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { originalOrderUser?.Email }, null,
							saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);*/
						} catch(Exception ex)
						{
							Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", originalOrderUser?.Email)}]"));
							Infrastructure.Services.Logging.Logger.Log(ex);
						}
					}
					// - 
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
