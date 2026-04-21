using Geocoding;
using Infrastructure.Data.Entities.Tables.MTM;
using Infrastructure.Services.Utils;
using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class ValidateHandler: IHandle<ValidateRequestModel, ResponseModel<ValidateResponseModel>>
	{
		private ValidateRequestModel data { get; set; }
		private UserModel user { get; set; }

		public ValidateHandler(UserModel user, ValidateRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<ValidateResponseModel> Handle()
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

		private ResponseModel<ValidateResponseModel> Perform(UserModel user, ValidateRequestModel data)
		{
			TransactionsManager botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);

			try
			{
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderNumber);
				var orderItems = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr);

				botransaction.beginTransaction();

				var response = prepareData(bestellung, orderItems, botransaction);
				if(!response.Success)
					return response;

				// Logging Validaton
				var _log = new LogHelper(
				bestellung.Nr,
				bestellung.Bestellung_Nr ?? -1,
				int.TryParse(bestellung.Projekt_Nr, out var val) ? val : 0,
				bestellung.Typ,
				LogHelper.LogType.VALIDATEORDER,
				"MTM",
				user).LogMTM(bestellung.Nr);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					var consumptionMailList = new List<Tuple<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity, decimal>>();
					var raPosIds = orderItems?.Where(x => x.RA_Pos_zu_Bestellposition.HasValue)?.Select(x => x.RA_Pos_zu_Bestellposition.Value);
					if(raPosIds?.Count() > 0)
					{
						var rahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(raPosIds.Distinct().ToList())?.Where(x=> x.OriginalAnzahl.HasValue && x.Geliefert.HasValue);
						var Rahmens = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(rahmenPos?.Select(x=> x.AngebotNr??0)?.ToList());
						foreach(var item in rahmenPos)
						{
							var consumptionPercentage = (item.OriginalAnzahl is not null && item.OriginalAnzahl.Value > 0)
								   ? Math.Floor(((item.Geliefert / item.OriginalAnzahl) * 100) ?? 0)
								   : 0;
							if(consumptionPercentage >= Module.ModuleSettings.RahmenConsumptionNotificationThreshold)
							{
								var rahmen = Rahmens.FirstOrDefault(x => x.Nr == item.AngebotNr);
								consumptionMailList.Add(new Tuple<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity, decimal>(item, rahmen, consumptionPercentage));
							}
						}
					}
					//consumtion threshold mail
					if(consumptionMailList != null && consumptionMailList.Count > 0)
					{
						foreach(var item in consumptionMailList)
						{
							Infrastructure.Services.Email.Helpers.SendConsumptionEmail(item.Item1, item.Item2, item.Item3);
						}
					}
					return ResponseModel<ValidateResponseModel>.SuccessResponse();
				}
				else
					return ResponseModel<ValidateResponseModel>.FailureResponse("Transaction didn't commit.");

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<ValidateResponseModel> prepareData(
			Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity bestellung,
			List<Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity> orderItems,
			TransactionsManager botransaction)
		{
			lock(this) // Change this to proper lock
			{
				if(bestellung.Typ != "Bestellung" || (bestellung.Typ == "Bestellung" && bestellung.Rahmenbestellung.HasValue && bestellung.Rahmenbestellung.Value == false))
				{
					updateBestellung(bestellung, botransaction);
				}
				else if(bestellung.Typ == "Bestellung" && (!bestellung.Rahmenbestellung.HasValue))
				{
					////var logArticles = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					var raPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenPositionNr(orderItems?.Select(x => x.RA_Pos_zu_Bestellposition ?? -1)?.ToList());
					var raPositionToUpdate = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
					foreach(var orderItem in orderItems)
					{
						// - 2023-08-10 - new process RA in Angebote
						if(orderItem?.RA_Pos_zu_Bestellposition.HasValue == true)
						{
							var raPosition = raPositions?.FirstOrDefault(x => x.AngeboteArtikelNr == orderItem.RA_Pos_zu_Bestellposition);
							// - update date to BE date + 3 days - Brenner - 2023-08-10
							raPosition.AckDate = ((orderItem.Liefertermin ?? raPosition.GultigBis) ?? DateTime.MinValue).AddDays(3);
							raPositionToUpdate.Add(raPosition);
						}
					}

					// - 2023-08-10
					updateBestellung(bestellung, botransaction);
					if(raPositionToUpdate.Count > 0)
					{
						Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.UpdateAckDateWithTransaction(raPositionToUpdate, botransaction.connection, botransaction.transaction);
					}
				}
				else
				{
				}
			}
			return ResponseModel<ValidateResponseModel>.SuccessResponse();
		}

		private void updateBestellung(BestellungenEntity bestellung, TransactionsManager botransaction)
		{

			bestellung.Mandant = data.Client;

			if(bestellung.Typ == "Bestellung" && (!bestellung.gebucht.HasValue || bestellung.gebucht == false) && bestellung.Rahmenbestellung == false)
			{
				bestellung.gebucht = true;
				bestellung.erledigt = false;
				//bestellung.Bearbeiter = this.user.Number;
				//This is how to reverse the bestellung??
			}

			if(bestellung.Typ?.ToLower() == Enums.OrderEnums.OrderTypes.Kanaban.GetDescription().ToLower())
			{
				bestellung.gebucht = true;
				bestellung.erledigt = false;
			}

			//set standard supplier violation value for positions
			var positions = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr);
			var positionsToUpdate = new List<KeyValuePair<int, bool>>();
			if(positions != null && positions.Count > 0)
			{
				var standardLieferants = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(positions.Select(x=>x.Artikel_Nr ?? -1));
				foreach(var position in positions)
				{
					var standardLieferant = standardLieferants.FirstOrDefault(x => x.Artikel_Nr == position.Artikel_Nr);
					if(standardLieferant != null && standardLieferant.Lieferanten_Nr.HasValue)
					{
						if(standardLieferant.Lieferanten_Nr.HasValue)
						{
							if(standardLieferant.Lieferanten_Nr.Value != bestellung.Lieferanten_Nr)
							{
								position.StandardSupplierViolation = true;
							}
							else
							{
								position.StandardSupplierViolation = false;
							}
							positionsToUpdate.Add(new KeyValuePair<int, bool>(position.Nr, position.StandardSupplierViolation ?? false));
						}
					}
				}
				Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateStandardSupplierViolation(positionsToUpdate, botransaction.connection, botransaction.transaction);

				// - 2025-08-27 - update RA qty on BE validate - Khelil
				// - take needed qty from BE
				var orderItemsWRa = positions?.Where(x => x.RA_Pos_zu_Bestellposition.HasValue && x.RA_Pos_zu_Bestellposition.Value > 0);
				if(orderItemsWRa?.Count() > 0)
				{
					var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderItemsWRa.Select(x => x.RA_Pos_zu_Bestellposition.Value).ToList());
					var positionToUpdate = new List<KeyValuePair<int, decimal>>();
					foreach(var item in raPositions)
					{
						var totalNeededQty = orderItemsWRa.Where(x => x.RA_Pos_zu_Bestellposition == item.Nr)?.Sum(x => x.Anzahl ?? 0) ?? 0;
						positionToUpdate.Add(new KeyValuePair<int, decimal>(item.Nr, (item.Anzahl ?? 0) - totalNeededQty));
					}
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateQuantities(positionToUpdate, botransaction.connection, botransaction.transaction);
				}
			}
			Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.UpdateWithTransaction(bestellung, botransaction.connection, botransaction.transaction);
		}

		public ResponseModel<ValidateResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<ValidateResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<ValidateResponseModel>.FailureResponse("User need to have a User Number");

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderNumber);

			if(bestellung == null)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Order not found");

			if(bestellung.gebucht == true)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Order is already validated");

			var orderItems = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr);
			if(orderItems == null || orderItems.Count == 0)
			{
				return ResponseModel<ValidateResponseModel>.FailureResponse("No Position found");
			}
			foreach(var orderITem in orderItems)
			{
				if(orderITem.Bestatigter_Termin is null || orderITem.Liefertermin is null)
				{
					return ResponseModel<ValidateResponseModel>.FailureResponse("All Positions should have a Delivery Date and a Confirmed Delivery Date ");
				}
			}
			var Lieferanten = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(bestellung.Lieferanten_Nr.HasValue ? bestellung.Lieferanten_Nr.Value : 0);
			if(Lieferanten is null)
			{
				return ResponseModel<ValidateResponseModel>.FailureResponse("No Supplier found");
			}
			if(bestellung.Typ == "Rahmenbestellung" || (bestellung.Typ == "Bestellung" && bestellung.Rahmenbestellung == true))
			{
				return ResponseModel<ValidateResponseModel>.FailureResponse("Order of type Rahmenbestellung can't be validated");
			}
			//  - 2025-09-02 - make sure supplier is linked to article
			var suppliersLinks = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(orderItems.Select(x => new KeyValuePair<int, int>(bestellung.Lieferanten_Nr ?? 0, x.Artikel_Nr ?? 0)));
			var errors = new List<string>();
			foreach(var orderITem in orderItems)
			{
				var suppliersLink = suppliersLinks.FirstOrDefault(x => x.Artikel_Nr == orderITem.Artikel_Nr);
				if(suppliersLink is null)
				{
					errors.Add($"Position {orderITem.Position}: supplier [{bestellung.Vorname_NameFirma}] does not exist in purchase for article.");
				}
			}
			if(errors.Count>0)
			{
				return ResponseModel<ValidateResponseModel>.FailureResponse(errors);
			}

			// - 2023-05-16 -  KH - block deleted/archived/blocked supplier
			var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(Lieferanten.nummer ?? -1);
			var supplierExt = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Lieferanten.Nr);

			if(adress is null || Lieferanten is null)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Supplier doesn't exist");
			if(adress.Lieferantennummer is null || adress.Lieferantennummer <= 0)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Supplier doesn't have a Nummer");
			if(adress.sperren == true)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Supplier is deleted");
			if(Lieferanten.gesperrt_fur_weitere_Bestellungen == true)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Supplier is blocked for further orders");
			if(supplierExt is not null && supplierExt.IsArchived == true)
				return ResponseModel<ValidateResponseModel>.FailureResponse("Supplier is archived");

			// - 2025-08-27 - take BE quantity from RA at validate
			var orderItemsWRa = orderItems?.Where(x => x.RA_Pos_zu_Bestellposition.HasValue && x.RA_Pos_zu_Bestellposition.Value > 0);
			if(orderItemsWRa?.Count()>0)
			{
				var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderItemsWRa.Select(x => x.RA_Pos_zu_Bestellposition.Value).ToList());
				var overbookedMsg = new List<string>();
				foreach(var item in raPositions)
				{
					var overBooking = orderItemsWRa.Where(x => x.RA_Pos_zu_Bestellposition == item.Nr);
					var totalNeededQty = overBooking?.Sum(x => x.Anzahl ?? 0);
					if(totalNeededQty>item.Anzahl)
					{
						overBooking.ForEach(x => overbookedMsg.Add($"Position {x.Position ?? 0}: total requested quantity [{x.Anzahl ?? 0}] is higher than available in Rahmen [{item.Anzahl ?? 0}]."));
					}
				}
				if(overbookedMsg.Count>0)
				{
					return ResponseModel<ValidateResponseModel>.FailureResponse(overbookedMsg);
				}
			}

			return ResponseModel<ValidateResponseModel>.SuccessResponse();
		}
	}
}
