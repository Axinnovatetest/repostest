using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class UpdateArticleInformationHandler: IHandle<UpdateArticleInformationRequestModel, ResponseModel<UpdateArticleInformationResponseModel>>
	{
		private UpdateArticleInformationRequestModel data { get; set; }
		private UserModel user { get; set; }
		public UpdateArticleInformationHandler(UserModel user, UpdateArticleInformationRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<UpdateArticleInformationResponseModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<UpdateArticleInformationResponseModel> Perform(UserModel user, UpdateArticleInformationRequestModel data)
		{
			var oldBAPostionEntity = new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity();
			if(data.Nr != -1 && data.Nr != 0)
				oldBAPostionEntity = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.Nr);

			// Calculate CUPreis Field.
			var tbl_kupfer = Infrastructure.Data.Access.Tables.MTM.TBL_KUPFERAccess.GetLatestPrice();
			var Kupferpreis = tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.HasValue ? tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.Value : 0;
			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.Artikel_Nr);

			var articleKupferzahl = article.Kupferzahl.HasValue ? article.Kupferzahl.Value : 0;
			data.CUPreis = (Kupferpreis * articleKupferzahl / 1000) * data.Quantity;
			//End Calculate CUPreis Field.



			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			var bestellArtikelId = oldBAPostionEntity != null ? oldBAPostionEntity.Nr : -1;

			if(oldBAPostionEntity != null && data.IsEdit)
			{
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(oldBAPostionEntity.Bestellung_Nr ?? -1);
				botransaction.beginTransaction();
				try
				{
					var updatedBAEntity = data.GetEntity(oldBAPostionEntity);
					updatedBAEntity.Nr = bestellArtikelId;

					var _log = new LogHelper(
							bestellung.Nr,
							oldBAPostionEntity.Bestellung_Nr ?? -1,
							int.TryParse($"{bestellung.Projekt_Nr}", out var p) ? p : 0,
							$"{bestellung.Typ}",
							LogHelper.LogType.MODIFICATIONPOS,
							"MTM",
							user);
					var logged = false;
					if(oldBAPostionEntity.Anzahl != data.Quantity)
					{
						logged = true;
						Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log.LogMTM(oldBAPostionEntity.Position ?? 10, $"[Menge] from {oldBAPostionEntity.Anzahl} to {data.Quantity}"), botransaction.connection, botransaction.transaction);
					}
					if(oldBAPostionEntity.Bestatigter_Termin != data.Bestatigter_Termin)
					{
						logged = true;
						Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log.LogMTM(oldBAPostionEntity.Position ?? 10, $"[Bestätigter Termin] from {oldBAPostionEntity?.Bestatigter_Termin.Value.ToString("dd/MM/yyyy")} to {data.Bestatigter_Termin.ToString("dd/MM/yyyy")}"), botransaction.connection, botransaction.transaction);
					}
					if(oldBAPostionEntity.Liefertermin != data.ReplinshementDate)
					{
						logged = true;
						Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log.LogMTM(oldBAPostionEntity.Position ?? 10, $"[Liefertermin] from {oldBAPostionEntity?.Liefertermin.Value.ToString("dd/MM/yyyy")} to {data.ReplinshementDate.ToString("dd/MM/yyyy")}"), botransaction.connection, botransaction.transaction);
					}
					if(!logged)
					{
						Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log.LogMTM(oldBAPostionEntity.Position ?? 10, $""), botransaction.connection, botransaction.transaction);
					}

					var diff = (updatedBAEntity.Anzahl ?? 0) - (oldBAPostionEntity.Anzahl ?? 0); // DON'T CHANGE ORDER 
					/// Diiff > 0 if new Anzahl > old Anzahl ==> this will remove the difference from the rahmen used quantity.
					/// Diff < 0 if  new Anzahl < old Anzahl ==> This will add back the quantity removed to the rahmen used quantity.
					/// - 2024-04-25 handle RA add/remove to position
					if(data.InfoRahmennummer != oldBAPostionEntity.RA_Pos_zu_Bestellposition)
					{
						// - adding RA to pos
						if(data.InfoRahmennummer > 0 && (!oldBAPostionEntity.RA_Pos_zu_Bestellposition.HasValue || oldBAPostionEntity.RA_Pos_zu_Bestellposition.Value <= 0))
						{
							diff = data.Quantity;
						}
						// - removing RA from pos
						if((!data.InfoRahmennummer.HasValue || data.InfoRahmennummer.Value <= 0) && oldBAPostionEntity?.RA_Pos_zu_Bestellposition > 0)
						{
							diff = -data.Quantity;
						}
						// - changing RA for another
						if(data.InfoRahmennummer > 0 && oldBAPostionEntity.RA_Pos_zu_Bestellposition > 0)
						{
							diff = -data.Quantity;
						}
					}

					// - 2025-08-27 - update RA qty on BE validate
					//var response = MaterialManagement.Helpers.SpecialHelper.UpdateRahmenBS<UpdateArticleInformationResponseModel>(updatedBAEntity, oldBAPostionEntity.RA_Pos_zu_Bestellposition ?? -1, diff, botransaction, bestellung.Lieferanten_Nr ?? -1);
					Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateWithTransaction(updatedBAEntity, botransaction.connection, botransaction.transaction);

					var response = ResponseModel<UpdateArticleInformationResponseModel>.SuccessResponse();

					// - 2025-08-26 set std supplier violation
					var standardLieferant = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(oldBAPostionEntity.Artikel_Nr ?? -1);
					Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateStandardSupplierViolation(oldBAPostionEntity.Nr, standardLieferant?.Lieferanten_Nr.HasValue == true && standardLieferant?.Lieferanten_Nr.Value != bestellung.Lieferanten_Nr, botransaction.connection, botransaction.transaction);

					if(!response.Success)
					{
						botransaction.rollback();
						return response;
					}
				} catch(Exception e)
				{
					botransaction.rollback();
					throw;
				}

			}
			else // New Article
			{
				botransaction.beginTransaction();
				try
				{
					// - 2023-03-30
					data.Bemerkung = data.Bemerkung ?? "-";
					data.CUPreis = 0;
					var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(data.Bestellung_Nr, botransaction.connection, botransaction.transaction);

					var updatedBAEntity = data.GetEntity();
					// - 2023-08-25 - CoC
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(updatedBAEntity.Artikel_Nr ?? -1, botransaction.connection, botransaction.transaction);
					if(!string.IsNullOrWhiteSpace(articleEntity?.CocVersion))
					{
						var cocEntity = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersion(articleEntity.CocVersion, botransaction.connection, botransaction.transaction);
						updatedBAEntity.CocVersion = $"{(cocEntity?.Count > 0 ? cocEntity[0].Name : "")} {articleEntity?.CocVersion}".Trim();
					}
					// - 2025-08-26 set std supplier violation
					var standardLieferant = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(updatedBAEntity.Artikel_Nr ?? -1);
					updatedBAEntity.StandardSupplierViolation = standardLieferant?.Lieferanten_Nr.HasValue == true && standardLieferant?.Lieferanten_Nr.Value != bestellung.Lieferanten_Nr;

					bestellArtikelId = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.InsertWithTransaction(updatedBAEntity, botransaction.connection, botransaction.transaction);
					//Logging
					var _log = new LogHelper(
						bestellung.Nr,
						data.Bestellung_Nr,
						0,
						$"{bestellung.Typ}",
						LogHelper.LogType.CREATIONPOS, "MTM", user)
						.LogMTM(data.Position);
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
					//var updatedBAEntity = data.GetEntity();
					updatedBAEntity.Nr = bestellArtikelId;
					var diff = updatedBAEntity.Anzahl ?? 0; // new BA Posittion no old anzahl to compare to.

					// - 2025-08-27 - update RA qty on BE validate - Khelil
					//var response = MaterialManagement.Helpers.SpecialHelper.UpdateRahmenBS<UpdateArticleInformationResponseModel>(updatedBAEntity, oldBAPostionEntity.RA_Pos_zu_Bestellposition ?? -1, diff, botransaction);
					Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateWithTransaction(updatedBAEntity, botransaction.connection, botransaction.transaction);
					var response = ResponseModel<UpdateArticleInformationResponseModel>.SuccessResponse();
					if(!response.Success)
					{
						botransaction.rollback();
						return response;
					}

				} catch(Exception)
				{
					botransaction.rollback();
					throw;
				}

			}


			if(botransaction.commit())
			{
				return ResponseModel<UpdateArticleInformationResponseModel>.SuccessResponse();
			}
			else
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Transaction didn't commit.");
		}
		public ResponseModel<UpdateArticleInformationResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<UpdateArticleInformationResponseModel>.AccessDeniedResponse();
			}

			if(user.Number == 0)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("User need to have a User Number");

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.Bestellung_Nr);

			if(bestellung is null)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Order not found");

			if(bestellung is not null && bestellung.gebucht == true)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Can't edit Validated Order");
			if(!IsValidDate(data.ReplinshementDate))
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse($"Wrong ReplinshementDate format [{data.ReplinshementDate}]");
			if(data is not null && data.ReplinshementDate > data.Bestatigter_Termin)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Confirmed date cannot be before Requested date");
			if(data.Price is null || data.Price <= 0)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Price cannot be empty or negative");
			if(string.IsNullOrEmpty(data.Bestell_Nr) || string.IsNullOrWhiteSpace(data.Bestell_Nr))
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Order Number cannot be empty");

			return ResponseModel<UpdateArticleInformationResponseModel>.SuccessResponse();
		}
		public static bool IsValidDate(DateTime date)
		{
			var year = date.Year;
			if(year < 1753 || year > 9999)
				return false;
			else
				return true;
		}
	}
}