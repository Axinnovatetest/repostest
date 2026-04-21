using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class CreateBlanketElementsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Models.Blanket.BlanketItem _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateBlanketElementsHandler(Models.Blanket.BlanketItem data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				int insertedId = -1;
				lock(Locks.Locks.OrdersLock)
				{
					int newPositionNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetMaxPositionNumberByOrderId(_data.AngebotNr ?? -1, botransaction.connection, botransaction.transaction) + 10;
					var orderData = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data.AngebotNr ?? -1, botransaction.connection, botransaction.transaction);
					var orderExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.AngebotNr ?? -1, botransaction.connection, botransaction.transaction);
					var itemDB = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(_data.MaterialNr ?? -1, botransaction.connection, botransaction.transaction);
					var wahrungenEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get(_data.WahrungId ?? -1, botransaction.connection, botransaction.transaction);
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetWithTransaction(orderExtension.CustomerId ?? -1, botransaction.connection, botransaction.transaction);
					var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(orderExtension.SupplierId ?? -1, botransaction.connection, botransaction.transaction);
					var bestellnummernEntity = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticleAndSupplier(itemDB.ArtikelNr, supplierEntity.Nummer ?? -1, botransaction.connection, botransaction.transaction);

					// - 
					var orderElementDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity()
					{
						AngebotNr = _data.AngebotNr,
						Position = newPositionNumber,
						Wunschtermin = orderData.Wunschtermin ?? DateTime.Now.AddDays(+30),
						Anzahl = _data.Zielmenge,
						AktuelleAnzahl = _data.Zielmenge,
						OriginalAnzahl = _data.Zielmenge,
						LieferanweisungP_FTXDIN_TEXT = "",
						Bemerkungsfeld1 = "",
						Bemerkungsfeld2 = "",
						Bezeichnung1 = _data.Bezeichnung,//itemDB.Bezeichnung1,
						Bezeichnung2 = _data.KundenMatNummer,//itemDB.Bezeichnung2,
						Bezeichnung3 = "-",
						Einheit = itemDB.Einheit,
						ArtikelNr = itemDB.ArtikelNr,
						Kupferbasis = 150,
						Preiseinheit = Convert.ToDecimal(itemDB.Preiseinheit ?? 1), // - 2022-05-30 Init to 1 as to respect DB Constraint
						DELFixiert = itemDB.DELFixiert ?? false,
						DEL = itemDB.DEL ?? 0,
						EinzelCuGewicht = Convert.ToDecimal(itemDB.CuGewicht ?? 0),
						Geliefert = 0, // << compatibility with psz soft
						Rabatt = 0, // << compatibility with psz soft
									//Einzelpreis = unitPrice * (wahrungenEntity.Entspricht_DM ?? 0),
									//Gesamtpreis = totalPrice * (wahrungenEntity.Entspricht_DM ?? 0),
						erledigt_pos = false,
						//
						USt = customerDb.Umsatzsteuer_berechnen.HasValue && customerDb.Umsatzsteuer_berechnen.Value ? 0.19m : 0m,
						//VKFestpreis = fixedTotalPrice,
						//GesamtCuGewicht = totalCuWeight,
						//Einzelkupferzuschlag = singleCopperSurcharge,
						//Gesamtkupferzuschlag = totalCopperSurcharge,
						//VKEinzelpreis = vkUnitPrice * (wahrungenEntity.Entspricht_DM ?? 0),
						//VKGesamtpreis = vKTotalPrice * (wahrungenEntity.Entspricht_DM ?? 0),
						Bezeichnung2_Kunde = null,
						EDI_Quantity_Ordered = 0m,
						EDI_PREIS_KUNDE = 0m,
						EDI_PREISEINHEIT = 0m,
						Index_Kunde = itemDB.Index_Kunde,
						Index_Kunde_Datum = itemDB.Index_Kunde_Datum,
						//EKPreise_Fix = fixedUnitPrice,
						Liefertermin = _data.DateOfExpiry,
						Bestellnummer = bestellnummernEntity?.Count > 0 ? bestellnummernEntity[0]?.Bestell_Nr : "",
					};
					// - order item extension
					var orderElementExtension = new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity
					{
						AngeboteArtikelNr = insertedId,
						BasePrice = _data.BasePrice,
						Bezeichnung = orderElementDb.Bezeichnung2,
						GultigAb = _data.ValidFrom,
						GultigBis = _data.DateOfExpiry,
						ExtensionDate = _data.DateOfExpiry, // - 2025-08-14 Hejdukova remove ExtDate ExtensionDate,
						KundenMatNummer = _data.KundenMatNummer,
						Material = _data.Material,
						MaterialNr = itemDB.ArtikelNr,
						ME = _data.ME,
						//Gesamtpreis = totalPrice * (wahrungenEntity.Entspricht_DM ?? 0),
						//Preis = unitPrice * (wahrungenEntity.Entspricht_DM ?? 0),
						WahrungName = wahrungenEntity?.Wahrung,
						WahrungSymbole = wahrungenEntity?.Symbol,
						WahrungId = wahrungenEntity?.Nr,
						Zielmenge = _data.Zielmenge,
						RahmenNr = (int)_data.AngebotNr,
						//
						//PreisDefault = unitPrice,
						//GesamtpreisDefault = totalPrice,
						//
						ReasonNewPosition = _data.Reason,
						Comment = _data.Comment,
						AB_nummer = _data.ABNummer,
					};
					// - price history
					var priceHistory = new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity()
					{
						RahmenNr = _data.AngebotNr ?? -1,
						PositionNr = insertedId,
						//Price = unitPrice * (wahrungenEntity.Entspricht_DM ?? 0),
						ValidFrom = _data.ValidFrom,
						DateUpdate = DateTime.Now,
						UserName = _user.Name,
						//PriceDefault = unitPrice,
						WarungSymbol = wahrungenEntity.Symbol,
					};

					// - Price computations
					ComputePositionPrice(orderElementDb, orderElementExtension, itemDB, wahrungenEntity, _data.Zielmenge ?? 0, _data.BasePrice);

					// - 
					insertedId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(orderElementDb, botransaction.connection, botransaction.transaction);
					orderElementExtension.AngeboteArtikelNr = insertedId;
					priceHistory.PositionNr = insertedId;
					Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.InsertWithTransaction(orderElementExtension, botransaction.connection, botransaction.transaction);

					//-
					priceHistory.BasePrice = _data.BasePrice;
					priceHistory.Price = orderElementDb.Einzelpreis;
					priceHistory.PriceDefault = orderElementExtension.PreisDefault;
					Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.InsertWithTransaction(priceHistory, botransaction.connection, botransaction.transaction);


					//updating price
					Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(_data.AngebotNr ?? -1);
					//logging
					var InsertedItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(insertedId, botransaction.connection, botransaction.transaction);
					var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction((int)InsertedItem.AngebotNr, botransaction.connection, botransaction.transaction);
					var _log = new LogHelper((int)Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ, LogHelper.LogType.CREATIONPOS, "CTS", _user)
						.LogCTS(null, null, null, (int)InsertedItem.Position, InsertedItem.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
				}
				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedId);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static void ComputePositionPrice(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity orderElementDb,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity orderElementDbExtension,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity itemDB, Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity wahrungenEntity,
			decimal quantity, decimal price)
		{
			#region price computations - 2022-09-06 - Reil
			//calculating prices
			var discount = 0m;
			var unitPriceBasis = Convert.ToDecimal(itemDB.Preiseinheit ?? 0m);
			var fixedTotalPrice = false; // -  cauz RA - itemDB.VKFestpreis ?? false;
			var fixedUnitPrice = true; // - cauz RA
			var cuWeight = Convert.ToDecimal(itemDB.CuGewicht ?? 0);
			var del = (itemDB.DEL ?? 0);

			var me1 = 0m;
			var me2 = 0m;
			var me3 = 0m;
			var me4 = 0m;
			var pm1 = 0m;
			var pm2 = 0m;
			var pm3 = 0m;
			var pm4 = 0m;
			var verkaufspreis = price; //  - 0m;

			var singleCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedTotalPrice,
					del,
					cuWeight);

			var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedTotalPrice,
				quantity,
				singleCopperSurcharge);

			var vkUnitPrice = fixedUnitPrice ? price : Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedTotalPrice,
				verkaufspreis,
				quantity,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(fixedUnitPrice, //fixedTotalPrice,
				unitPriceBasis,
				quantity,
				fixedUnitPrice ? vkUnitPrice + singleCopperSurcharge : vkUnitPrice,
				verkaufspreis,
				singleCopperSurcharge,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var totalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateTotalPrice(unitPriceBasis,
				unitPrice,
				quantity,
				discount);

			var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
				vkUnitPrice,
				quantity);

			var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight(quantity,
				cuWeight);
			#endregion price computations

			orderElementDb.Einzelpreis = unitPrice;
			orderElementDb.Gesamtpreis = totalPrice;
			orderElementDb.VKFestpreis = fixedTotalPrice;
			orderElementDb.GesamtCuGewicht = totalCuWeight;
			orderElementDb.Einzelkupferzuschlag = singleCopperSurcharge;
			orderElementDb.Gesamtkupferzuschlag = totalCopperSurcharge;
			orderElementDb.VKEinzelpreis = vkUnitPrice;
			orderElementDb.VKGesamtpreis = vKTotalPrice;
			orderElementDb.EKPreise_Fix = fixedUnitPrice;

			//order item extension
			orderElementDbExtension.Gesamtpreis = totalPrice;
			orderElementDbExtension.Preis = unitPrice;
			orderElementDbExtension.PreisDefault = unitPrice * (wahrungenEntity.Entspricht_DM ?? 0);
			orderElementDbExtension.GesamtpreisDefault = totalPrice * (wahrungenEntity.Entspricht_DM ?? 0);
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.AngebotNr ?? -1);
			if(rahmenEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Rahmen not found");
			var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.AngebotNr ?? -1);
			if(rahmenExtension != null && rahmenExtension.StatusId == (int)Enums.BlanketEnums.RAStatus.Validated)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Rahmen is validated cannot add position .");
			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data.Material);
			if(itemDb == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Position Item not found");


			if(_data.Zielmenge == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Zielmenge " + _data.Zielmenge + " is invalid");
			if(_data.BasePrice == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Preis " + _data.Preis + " is invalid");
			if(_data.WahrungId == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Währung " + _data.WahrungId + " is invalid");
			if(_data.ValidFrom == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Start Date {_data.ValidFrom} is invalid");
			if(_data.DateOfExpiry == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"End Date {_data.DateOfExpiry} is invalid");
			if(_data.ValidFrom > _data.DateOfExpiry && (rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale || (rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase && rahmenEntity.Personal_Nr != -1)))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Start Date {_data.ValidFrom.Value.ToString("dd/MM/yyyy")} is greater than End Date {_data.DateOfExpiry.Value.ToString("dd/MM/yyyy")}");
			// - 2025-08-14 Hejdukova remove ExtDate 
			//if(_data.ExtensionDate < _data.DateOfExpiry)
			//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Extension Date {_data.ExtensionDate.Value.ToString("dd/MM/yyyy")} should greater than {_data.DateOfExpiry.Value.ToString("dd/MM/yyyy")}");

			if(rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
			{
				DateTime _newDate, _oldDate;
				_newDate = _oldDate = _data.DateOfExpiry ?? new DateTime(1900, 1, 1);
				var horizonCheck = Helpers.HorizonsHelper.userHasRAPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
				if(!horizonCheck)
					return ResponseModel<int>.FailureResponse(messages);
			}
			else
			{
				// - RA-Purchase make sure price is smaller or equal to std
				var bestellnummernEntity = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(rahmenEntity.Kunden_Nr??0, itemDb.ArtikelNr);
				if((bestellnummernEntity?.Einkaufspreis ?? 0) < _data.BasePrice)
				{
					return ResponseModel<int>.FailureResponse( $"Price [{_data.BasePrice}] cannot be bigger than standard price [{bestellnummernEntity?.Einkaufspreis ?? 0}].");
				}
			}
				return ResponseModel<int>.SuccessResponse();
		}
	}
}
