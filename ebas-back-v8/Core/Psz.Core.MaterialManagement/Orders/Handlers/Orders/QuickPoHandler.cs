using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class QuickPoHandler: IHandle<QuickPORequestModel, ResponseModel<QuickPOResponseModel>>
	{

		private QuickPORequestModel data { get; set; }
		private UserModel user { get; set; }

		public QuickPoHandler(UserModel user, QuickPORequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<QuickPOResponseModel> Handle()
		{

			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<QuickPOResponseModel> Perform()
		{
			try
			{
				//Get Article default Supplier data
				var bestellenumeren = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(this.data.ArticleNr);
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this.data.ArticleNr);
				var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(bestellenumeren.Lieferanten_Nr.Value);
				var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(adress.Nr);
				var condition = Infrastructure.Data.Access.Tables.MTM.KonditionszuordnungstabelleAccess.Get((supplier.Konditionszuordnungs_Nr.HasValue) ? supplier.Konditionszuordnungs_Nr.Value : -1);
				var bestellungenMax = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetMax(Enums.OrderEnums.OrderTypes.Order.GetDescription());
				//Calculate CUPreis Field.
				var tbl_kupfer = Infrastructure.Data.Access.Tables.MTM.TBL_KUPFERAccess.GetLatestPrice();
				var Kupferpreis = tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.HasValue ? tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.Value : 0;
				var articleKupferzahl = article.Kupferzahl.HasValue ? article.Kupferzahl.Value : 0;
				var CUPreis = (Kupferpreis * articleKupferzahl / 1000) * data.Quantity;
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
						Bestellbestatigung_erbeten_bis = DateTime.Now.Date,
						Typ = "Bestellung",
						Rahmenbestellung = false,
						Bearbeiter = this.user.Number,
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
						Benutzer =$"{this.user.LegacyUserName} [{this.user.Username}] {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"
					};
					int orderId = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.InsertWithTransaction(orderData, botransaction.connection, botransaction.transaction);

					//Logging
					//var orderAfterInsert = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(orderId, botransaction.connection, botransaction.transaction);
					var _log = new LogHelper(
						orderId,
						bestellungenMax + 1,
						bestellungenMax + 1,
						orderData.Typ,
						LogHelper.LogType.CREATIONORDER_QUICKPO,
						"MTM",
						user)
							.LogMTM(orderId);
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

					// Add Position 
					var artickelEntity = this.data.GetEntity(CUPreis, orderId, article, bestellenumeren);
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(artickelEntity.Artikel_Nr ?? -1, botransaction.connection, botransaction.transaction);
					if(!string.IsNullOrWhiteSpace(articleEntity?.CocVersion))
					{
						var cocEntity = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersion(articleEntity.CocVersion, botransaction.connection, botransaction.transaction);
						artickelEntity.CocVersion = $"{(cocEntity?.Count > 0 ? cocEntity[0].Name : "")} {articleEntity?.CocVersion}".Trim();
					}
					var bestelleArtikelId = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.InsertWithTransaction(artickelEntity, botransaction.connection, botransaction.transaction);

					//Logging
					var _logA = new LogHelper(
						orderId,
						bestellungenMax + 1,
						bestellungenMax + 1,
						$"{orderData.Typ}",
						LogHelper.LogType.CREATIONPOS, "MTM", user)
						.LogMTM(bestelleArtikelId);
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_logA, botransaction.connection, botransaction.transaction);

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
					var availableRahmmenReposne = new Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails.GetAvailableRahmensHandler(user, availableRaRequest).Handle();
					if(availableRahmmenReposne.Success && availableRahmmenReposne.Body != null && availableRahmmenReposne.Body.Count > 0)
					{
						var rahmenToApply = availableRahmmenReposne.Body[0];
						artickelEntity.RA_Pos_zu_Bestellposition = rahmenToApply.PositionNr;
						artickelEntity.Bestellung_Nr = orderId;
						artickelEntity.Nr = bestelleArtikelId;
						var diff = artickelEntity.Anzahl ?? 0;
						var oldBAPostionEntity = new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity();
						// - 2025-08-27 - update RA qty on BE validate - Khelil
						var response = ResponseModel<QuickPOResponseModel>.SuccessResponse();// MaterialManagement.Helpers.SpecialHelper.UpdateRahmenBS<UpdateArticleInformationResponseModel>(artickelEntity, oldBAPostionEntity.RA_Pos_zu_Bestellposition ?? -1, diff, botransaction);
					}
					if(botransaction.commit())
					{
						return ResponseModel<QuickPOResponseModel>.SuccessResponse(new QuickPOResponseModel() { Id = orderId });

					}
					else
						return ResponseModel<QuickPOResponseModel>.FailureResponse("Transaction didn't commit.");


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

		public ResponseModel<QuickPOResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<QuickPOResponseModel>.AccessDeniedResponse();
			}
			if(user.Access?.MaterialManagement?.Purchasing?.OrderQuickPO != true)
			{
				return ResponseModel<QuickPOResponseModel>.FailureResponse("User does not have access");
			}
			if(user.Number is null || user.Number <=0)
			{
				return ResponseModel<QuickPOResponseModel>.FailureResponse($"User does not have a valid PO Creation Number [{user.Number}].");
			}
			if(this.data.Quantity <= 0)
			{
				return ResponseModel<QuickPOResponseModel>.FailureResponse($"Invalid quantity [{this.data.Quantity}]");
			}
			if(this.data.LagerortId <= 0)
			{
				return ResponseModel<QuickPOResponseModel>.FailureResponse($"Invalid Lager [{this.data.LagerortId}]");
			}

			//var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.Get(this.data.SupplierNr);
			//if(supplier is null)
			//	return ResponseModel<QuickPOResponseModel>.FailureResponse("Supplier doesn't exist");

			var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this.data.ArticleNr);
			if(artikel is null)
				return ResponseModel<QuickPOResponseModel>.FailureResponse("Artikel doesn't exist");

			// - 2023-05-16 -  KH - block deleted/archived/blocked supplier
			var bestellenumeren = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(this.data.ArticleNr);
			var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(bestellenumeren.Lieferanten_Nr.Value);
			var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(adress.Nr);
			var supplierExt = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(supplier.Nr);

			if(adress is null || supplier is null)
				return ResponseModel<QuickPOResponseModel>.FailureResponse("Supplier doesn't exist");
			if(adress.Lieferantennummer is null || adress.Lieferantennummer <= 0)
				return ResponseModel<QuickPOResponseModel>.FailureResponse("Supplier doesn't have a Nummer");
			if(adress.sperren == true)
				return ResponseModel<QuickPOResponseModel>.FailureResponse("Supplier is deleted");
			if(supplier.gesperrt_fur_weitere_Bestellungen == true)
				return ResponseModel<QuickPOResponseModel>.FailureResponse("Supplier is blocked for further orders");
			if(supplierExt is not null && supplierExt.IsArchived == true)
				return ResponseModel<QuickPOResponseModel>.FailureResponse("Supplier is archived");

			return ResponseModel<QuickPOResponseModel>.SuccessResponse();
		}
	}
}
