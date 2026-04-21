using iText.StyledXmlParser.Jsoup.Helper;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class AddBestellungenFromBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private AddABFromRahmenModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AddBestellungenFromBlanketHandler(Identity.Models.UserModel user, AddABFromRahmenModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var rahmen = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RahmenId);
				var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.RahmenId);
				var supplier = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(rahmenExtension.SupplierId ?? -1);

				var supplierAdress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplier.Nummer ?? -1);

				var condition = Infrastructure.Data.Access.Tables.MTM.KonditionszuordnungstabelleAccess.Get((supplier.Konditionszuordnungs_Nr.HasValue) ? supplier.Konditionszuordnungs_Nr.Value : -1);
				var bestellungenMax = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetMax();
				//Calculate CUPreis Field.
				var tbl_kupfer = Infrastructure.Data.Access.Tables.MTM.TBL_KUPFERAccess.GetLatestPrice();
				var Kupferpreis = tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.HasValue ? tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.Value : 0;


				botransaction.beginTransaction();

				// Add Order Entity
				var orderData = new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity()
				{

					Anrede = supplierAdress?.Anrede,
					Vorname_NameFirma = supplierAdress?.Name1,
					Name2 = supplierAdress.Name2,
					Name3 = supplierAdress?.Name3,
					Abteilung = supplierAdress.Abteilung,
					Strasse_Postfach = supplierAdress.StraBe,
					Land_PLZ_Ort = String.Concat(supplierAdress?.PLZ_StraBe, ' ', supplierAdress?.Ort),
					Versandart = supplier?.Versandart,
					Zahlungsweise = supplier?.Zahlungsweise,
					Konditionen = condition?.Text,
					Unser_Zeichen = supplierAdress?.Lieferantennummer is not null ? Convert.ToString(supplierAdress.Lieferantennummer.Value) : null,
					Ihr_Zeichen = supplier?.Kundennummer_Lieferanten,
					Wahrung = supplier?.Wahrung,
					Frachtfreigrenze = decimal.TryParse (supplier?.Frachtfreigrenze?.ToString(), out var f)?f: 0,
					Mindestbestellwert = decimal.TryParse( supplier?.Mindestbestellwert?.ToString(), out var m)?m:0,
					Briefanrede = supplierAdress?.Briefanrede,
					Lieferanten_Nr = supplier?.Nummer,
					Bestellung_Nr = bestellungenMax + 1,
					Projekt_Nr = rahmen?.Projekt_Nr,
					Bestellbestatigung_erbeten_bis = DateTime.Now.Date,
					Typ = "Bestellung",
					Rahmenbestellung = false,
					Bearbeiter = _user.Number,
					Datum = DateTime.Now.Date,
					Personal_Nr = 0,
					USt = 0,
					Rabatt = 0,
					Kundenbestellung = 0,
					best_id = 0,
					nr_anf = 0,
					nr_RB = 0,
					nr_bes = 0,
					nr_war = 0,
					nr_gut = 0,
					nr_sto = 0,
					Belegkreis = 0,
					Neu = false,
				};
				int orderId = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.InsertWithTransaction(orderData, botransaction.connection, botransaction.transaction);
				var positionsToInsert = new List<Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity>();
				var logs = new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
				var positionsErrors = new List<string>();
				foreach(var item in _data.Positions)
				{
					var rahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(item.AngeboteneArtikelNr);
					if(rahmenPos.Anzahl < item.ABQuantity)
					{
						positionsErrors.Add($"RA Position {rahmenPos.Position}, added quantity in BE [{item.ABQuantity}] is bigger then the rahmen position rest quantity [{rahmenPos.Anzahl}]");
						continue;
					}
					var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.ArticleId);
					var bestellenumeren = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(item.ArticleId);
					var articleKupferzahl = article.Kupferzahl.HasValue ? article.Kupferzahl.Value : 0;
					var CUPreis = (Kupferpreis * articleKupferzahl / 1000) * item.ABQuantity;
					positionsToInsert.Add(Helpers.BlanketHelper.GetBestellArtikelEntity(CUPreis ?? 0m, orderId, article, bestellenumeren, item.ABQuantity ?? 0m, item.AngeboteneArtikelNr, rahmenPos.Einzelpreis ?? 0m, item.Lagerort ?? -1, item.ABWunstermin));


					// -2025 - 08 - 27 - update RA qty on BE validate - Khelil
					//rahmenPos.Anzahl = rahmenPos.Anzahl - item.ABQuantity;
					//rahmenPos.Geliefert = rahmenPos.OriginalAnzahl - rahmenPos.Anzahl;
					//Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(rahmenPos, botransaction.connection, botransaction.transaction);
					Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(rahmen.Nr, botransaction);
				}

				if(positionsErrors.Count > 0)
					return ResponseModel<int>.FailureResponse(positionsErrors);

				//logging
				positionsToInsert.ForEach(x =>
				{
					var bestelleArtikelId = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.InsertWithTransaction(positionsToInsert, botransaction.connection, botransaction.transaction);
					logs.Add(new BestellungenLogHelper(
					bestelleArtikelId,
					bestellungenMax + 1,
					bestellungenMax + 1,
					$"Article {x.Artikel_Nr}",
					BestellungenLogHelper.LogType.CREATIONPOS, "MTM", _user)
					.LogMTM(bestelleArtikelId));
				});
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(orderId);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction didn't commit.");
			} catch(Exception e)
			{
				botransaction.rollback();
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

			if(_data.Positions == null || _data.Positions.Count == 0)
				return ResponseModel<int>.FailureResponse("No positions to create.");
			foreach(var item in _data.Positions)
			{
				if(!item.ABQuantity.HasValue || item.ABQuantity <= 0)
					return ResponseModel<int>.FailureResponse("Position(s) quantity should not be empty,null or negative");
				if(item.ABQuantity > item.RARestQuantity)
					return ResponseModel<int>.FailureResponse("Order quantity cannot be bigger then RA rest quantity.");
				if(!item.Lagerort.HasValue)
					return ResponseModel<int>.FailureResponse("Lagerort required");
				if(!item.ABWunstermin.HasValue)
					return ResponseModel<int>.FailureResponse("BS Liefertermin required");
				foreach(var position in _data.Positions)
				{
					var positionExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(position.AngeboteneArtikelNr);
					// - 2025-08-14 Hejdukova remove ExtDate 
					if(position.ABWunstermin < positionExtension.GultigAb )
						return ResponseModel<int>.FailureResponse("BS Liefertermin should be in RA position date range");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
		public void SendConsumptionEmail(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity rahmenPos, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity rahmen, UserModel user, decimal percenatge)
		{
			var emailTitle = $"Rahmen position consumption notification";
			var emailBody = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
			+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";
			emailBody += $"<br/><span style='font-size:1.15em;'>The position [{rahmenPos.Position}] of the rahmen [{rahmen.Bezug}] has reached <strong style='color:red'>{percenatge}%</strong> consumption of its quantity</span>";
			emailBody += $"<br/><span style='font-size:1.15em;'>Original quantity: [{rahmenPos.OriginalAnzahl}]</span>";
			emailBody += $"<br/><span style='font-size:1.15em;'>Ordred quantity: [{rahmenPos.Geliefert}]</span>";
			emailBody += $"<br/><span style='font-size:1.15em;'>Rest quantity: [{rahmenPos.Anzahl}]</span>";
			emailBody += $"</span><br/><br/>Please, login to check it <a href='{Module.PurchaseEmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/rahmen/info/{rahmen.Nr}'>here</a>";
			emailBody += "<br/><br/>Regards, <br/>IT Department </div>";
			// Send email notification
			var adressesEntities = Infrastructure.Data.Access.Tables.PRS.RahmenConsumptionNotificationMailAdressesAccess.Get();
			var adresses = adressesEntities.Select(x => x.Mail).ToList();
			Module.EmailingService.SendEmailSendGridWithStaticTemplate(
				emailBody,
				emailTitle,
				  adresses, null, null,
			   true, user?.Email, user?.Username, senderId: user?.Id ?? -1, senderCC: false, attachmentIds: null);
		}

	}
}
