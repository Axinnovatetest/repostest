using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using NLog.Fluent;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class QuickPoHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public QuickPoHandler(UserModel user, int data)
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

				return Perform();

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(article == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}
			var bestellenumeren = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(this._data);
			if(bestellenumeren == null)
			{
				return ResponseModel<int>.FailureResponse("Bestellnummern not found");
			}
			var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(bestellenumeren.Lieferanten_Nr ?? -1);
			if(adress == null)
			{
				return ResponseModel<int>.FailureResponse("Standardlieferant not found");
			}
			var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(adress.Nr);
			if(supplier == null)
			{
				return ResponseModel<int>.FailureResponse("Supplier not found");
			}
			return ResponseModel<int>.SuccessResponse();
		}
		private ResponseModel<int> Perform()
		{
			try
			{
				var LagerortId = 3;
				//Get Article default Supplier data
				var bestellenumeren = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(this._data);
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(bestellenumeren.Lieferanten_Nr.Value);
				var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(adress.Nr);
				var condition = Infrastructure.Data.Access.Tables.MTM.KonditionszuordnungstabelleAccess.Get((supplier.Konditionszuordnungs_Nr.HasValue) ? supplier.Konditionszuordnungs_Nr.Value : -1);
				var bestellungenMax = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetMax(Enums.ArticleEnums.OrderTypes.Order.GetDescription());
				//Calculate CUPreis Field.
				var tbl_kupfer = Infrastructure.Data.Access.Tables.MTM.TBL_KUPFERAccess.GetLatestPrice();
				var Kupferpreis = tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.HasValue ? tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.Value : 0;
				var articleKupferzahl = article.Kupferzahl.HasValue ? article.Kupferzahl.Value : 0;
				var minimumQuantity = (decimal?)bestellenumeren.Mindestbestellmenge ?? 1;
				var CUPreis = (Kupferpreis * articleKupferzahl / 1000) * minimumQuantity;
				//End Calculate CUPreis Field.

				var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
				try
				{
					botransaction.beginTransaction();

					// Add Order Entity
					var orderData = new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity()
					{
						Anrede = adress.Anrede,
						Vorname_NameFirma = adress.Name1,
						Name2 = adress.Name2,
						Name3 = adress.Name3,
						Abteilung = adress.Abteilung,
						Strasse_Postfach = adress.Strasse,
						Land_PLZ_Ort = String.Concat(adress.PLZ_Strasse, ' ', adress.Ort),
						Versandart = supplier.Versandart,
						Zahlungsweise = supplier.Zahlungsweise,
						Konditionen = condition?.Text,
						Unser_Zeichen = adress.Lieferantennummer.HasValue ? Convert.ToString(adress.Lieferantennummer.Value) : null,
						Ihr_Zeichen = supplier.Kundennummer__Lieferanten_,
						Wahrung = supplier.Wahrung,
						Frachtfreigrenze = supplier.Frachtfreigrenze,
						Mindestbestellwert = supplier.Mindestbestellwert,
						Briefanrede = adress.Briefanrede,
						Liefertermin = DateTime.Now.AddDays(bestellenumeren.Wiederbeschaffungszeitraum.Value).Date,
						Lieferanten_Nr = bestellenumeren.Lieferanten_Nr.Value,
						Bestellung_Nr = bestellungenMax + 1,
						Projekt_Nr = (bestellungenMax + 1).ToString(),
						Bestellbestatigung_erbeten_bis = DateTime.Now.AddDays(+3).Date,
						Typ = "Bestellung",
						Rahmenbestellung = false,
						Bearbeiter = this._user.Number,
						Datum = DateTime.Now.Date,
						Personal_Nr = 0,
						USt = 0,
						Rabatt = 0,
						Kundenbestellung = 0,
						best_id = 0,
						nr_anf = 999999,
						nr_RB = 0,
						nr_bes = 0,
						nr_war = 0,
						nr_gut = 0,
						nr_sto = 0,
						Belegkreis = 0,
						Neu = true,
						gebucht = false,
						gedruckt = false,
						erledigt = false,
						datueber = false,
						Mandant = "-",
						Loschen = false,
						In_Bearbeitung = false,
						Offnen = false,
						Kanban = false,
					};
					int orderId = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.InsertWithTransaction(orderData, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.UpdateBestId(orderId, botransaction.connection, botransaction.transaction);

					//Logging
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity
					{
						Id = orderId,
						BestellungenNr = bestellungenMax + 1,
						DateTime = DateTime.Now,
						LogObject = "Bestellung",
						LogText = "[Bestellung] Created (Quick PO) - Artikelstamm",
						LogType = "Order Creation (Quick PO)",
						Nr = orderId,
						Origin = "MTD",
						ProjektNr = bestellungenMax + 1,
						UserId = this._user.Id,
						Username = this._user.Username
					}, botransaction.connection, botransaction.transaction);

					// Add Position 
					var artickelEntity = new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity //= this.data.GetEntity(CUPreis, orderId, article, bestellenumeren);
					{
						Bestellung_Nr = orderId,
						Lagerort_id = LagerortId,
						AB_Nr_Lieferant = "",
						Kanban = false,
						CUPreis = CUPreis,
						Artikel_Nr = article.ArtikelNr,
						Bezeichnung_1 = article.Bezeichnung1,
						Bezeichnung_2 = article.Bezeichnung2,
						Einheit = article.Einheit,
						Umsatzsteuer = bestellenumeren.Umsatzsteuer.HasValue ? (float)bestellenumeren.Umsatzsteuer.Value : 0,
						Einzelpreis = bestellenumeren.Einkaufspreis ?? 0,
						Bestellnummer = bestellenumeren.Bestell_Nr,
						Rabatt = (float)bestellenumeren.Rabatt,
						Preiseinheit = bestellenumeren.Preiseinheit ?? 1,
						Liefertermin = DateTime.Now.AddDays(bestellenumeren.Wiederbeschaffungszeitraum ?? 30).Date,
						Anzahl = minimumQuantity,
						Gesamtpreis = (bestellenumeren.Einkaufspreis * minimumQuantity) ?? 0,
						Bestatigter_Termin = new DateTime(2999, 12, 31).Date,
						Position = 10,
						InfoRahmennummer = null,
						AnfangLagerBestand = 0,
						Start_Anzahl = minimumQuantity,
						Erhalten = 0,
						Aktuelle_Anzahl = 0,
						EndeLagerBestand = 0,
						Rabatt1 = 0,
						Produktionsort = 0,
						BP_zu_RBposition = 0,
						WE_Pos_zu_Bestellposition = 0,
						RB_OriginalAnzahl = 1,
						RB_Abgerufen = 1,
						RB_Offen = 1,
						erledigt_pos = false,
						Position_erledigt = false,
						Bemerkung_Pos = "Quick PO - Artikelstamm",
						Bemerkung_Pos_ID = false,
						In_Bearbeitung = false,
						Loschen = false,
					};
					//var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(artickelEntity.Artikel_Nr ?? -1, botransaction.connection, botransaction.transaction);
					if(!string.IsNullOrWhiteSpace(article?.CocVersion))
					{
						var cocEntity = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersion(article.CocVersion, botransaction.connection, botransaction.transaction);
						artickelEntity.CocVersion = $"{(cocEntity?.Count > 0 ? cocEntity[0].Name : "")} {article?.CocVersion}".Trim();
					}
					var bestelleArtikelId = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.InsertWithTransaction(artickelEntity, botransaction.connection, botransaction.transaction);

					//Logging
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity
						{
							BestellungenNr = bestellungenMax + 1,
							DateTime = DateTime.Now,
							LogObject = "Bestellung",
							LogText = "[Bestellung] New Position [10] Created",
							LogType = "Position Creation",
							Nr = orderId,
							Origin = "MTD",
							ProjektNr = bestellungenMax + 1,
							UserId = _user.Id,
							Username = _user.Username
						}, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
						ObjectLogHelper.getLog(this._user, _data,
					$"QuickPo",
					$"",
					$"Quantity {minimumQuantity} | Lager {LagerortId}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Add),
						botransaction.connection, botransaction.transaction);

					// - 
					// - add oldest available ra if any (Sani) 10/04/2025
					var availableRaRequest = new Psz.Core.MaterialManagement.Models.InfoRahmenRequestModel
					{
						ArtikelNr = artickelEntity.Artikel_Nr,
						PositionNr = bestelleArtikelId,
						Quantity = artickelEntity.Anzahl ?? 0,
						SupplierId = bestellenumeren.Lieferanten_Nr.Value,
						ConfirmationDate = artickelEntity?.Liefertermin ?? (DateTime.Now.AddDays(bestellenumeren is not null && bestellenumeren.Wiederbeschaffungszeitraum.HasValue ? bestellenumeren.Wiederbeschaffungszeitraum.Value : 0))
					};
					var availableRahmmenReposne = new Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails.GetAvailableRahmensHandler(_user, availableRaRequest).Handle();
					if(availableRahmmenReposne.Success && availableRahmmenReposne.Body != null && availableRahmmenReposne.Body.Count > 0)
					{
						var rahmenToApply = availableRahmmenReposne.Body[0];
						artickelEntity.RA_Pos_zu_Bestellposition = rahmenToApply.PositionNr;
						artickelEntity.Bestellung_Nr = orderId;
						artickelEntity.Nr = bestelleArtikelId;
						var diff = artickelEntity.Anzahl ?? 0;
						var oldBAPostionEntity = new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity();

						// - 2025-08-27 - update RA qty on BE validate - Khelil
						var response = ResponseModel<int>.SuccessResponse(); //MaterialManagement.Helpers.SpecialHelper.UpdateRahmenBS<Psz.Core.MaterialManagement.Orders.Models.OrderDetails.UpdateArticleInformationResponseModel>(artickelEntity, oldBAPostionEntity.RA_Pos_zu_Bestellposition ?? -1, diff, botransaction);
					}
					if(botransaction.commit())
					{
						return ResponseModel<int>.SuccessResponse(orderId);

					}
					else
						return ResponseModel<int>.FailureResponse("Transaction didn't commit.");


				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			} catch(Exception e)
			{
				throw;
			}
		}
	}
}
