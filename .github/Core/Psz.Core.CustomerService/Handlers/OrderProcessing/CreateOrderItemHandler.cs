//using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class CreateOrderItemHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private CreateOrderItemModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public Infrastructure.Services.Utils.TransactionsManager _botransaction { get; set; }
		public CreateOrderItemHandler(CreateOrderItemModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			return this.Perform();
		}
		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var orderData = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data.OrderId, _botransaction.connection, _botransaction.transaction);
			if(orderData == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Order not found");

			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumberWithTransaction(_data.ItemNumber, null, _botransaction.connection, _botransaction.transaction);
			// - 2023-05-10 - Heidenreich - allow only ONCE to toggle Book for RG
			//var technicArticles = Module.BSD.TechnicArticleIds;
			var itemArticleIsTechnic = Helpers.HorizonsHelper.ArticleIsTechnic(itemDb.ArtikelNr);
			if(orderData.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE.Trim().ToLower())
			{
				if(orderData.Datum < DateTime.Today || orderData.Gebucht == true)
				{
					return ResponseModel<int>.FailureResponse("Invoice edit is not allowed");
				}
				var haveHorizon = (_user.Access.CustomerService.RGPosHorizon1 || _user.Access.CustomerService.RGPosHorizon2 || _user.Access.CustomerService.RGPosHorizon3);
				if(!haveHorizon && !itemArticleIsTechnic)
					return ResponseModel<int>.FailureResponse("You have no RG position horizon assigned.");
			}
			if(orderData.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION.Trim().ToLower())
			{
				var haveHorizon = (_user.Access.CustomerService.ABPosHorizon1 || _user.Access.CustomerService.ABPosHorizon2 || _user.Access.CustomerService.ABPosHorizon3);
				if(!haveHorizon && !itemArticleIsTechnic)
					return ResponseModel<int>.FailureResponse("You have no AB position horizon assigned.");
			}
			if(orderData.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY.Trim().ToLower())
			{
				var haveHorizon = (_user.Access.CustomerService.LSPosHorizon1 || _user.Access.CustomerService.LSPosHorizon2 || _user.Access.CustomerService.LSPosHorizon3);
				if(!haveHorizon && !itemArticleIsTechnic)
					return ResponseModel<int>.FailureResponse("You have no LS position horizon assigned.");
			}
			if(orderData.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST.Trim().ToLower())
			{
				var haveHorizon = (_user.Access.CustomerService.FRCPosHorizon1 || _user.Access.CustomerService.FRCPosHorizon2 || _user.Access.CustomerService.FRCPosHorizon3);
				if(!haveHorizon && !itemArticleIsTechnic)
					return ResponseModel<int>.FailureResponse("You have no Forcast position horizon assigned.");
			}

			if(itemDb == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Item not found");

			return ResponseModel<int>.SuccessResponse();
		}
		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex GetLastIndex(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
			Infrastructure.Services.Utils.TransactionsManager transactionManager = null)
		{
			var existingTransaction = true;
			if(transactionManager == null)
			{
				transactionManager = new Infrastructure.Services.Utils.TransactionsManager();
				transactionManager.beginTransaction();
				existingTransaction = false;
			}
			var _lastIndex = new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex
			{
				KundenIndex = null,
				KundenIndexDate = null
			};
			if(articleEntity != null)
			{
				if(Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetBOMVersionByArticle_Count(articleEntity.ArtikelNr, transactionManager.connection, transactionManager.transaction) > 0)
				{

					var kundenIndexes = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetKundenIndexSnapshotTimeByArticle(articleEntity.ArtikelNr, transactionManager.connection, transactionManager.transaction)
						?.DistinctBy(x => x.KundenIndex)?.ToList();
					var max1 = MoreLinq.MoreEnumerable.MaxBy(kundenIndexes, x => x.SnapshotTime).ToList();
					var max2 = MoreLinq.MoreEnumerable.MaxBy(max1, x => x.KundenIndexDate).ToList();
					var maxIndexKunden = MoreLinq.MoreEnumerable.MaxBy(max2, x => x.KundenIndex).ToList() ?? new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex>();
					if(maxIndexKunden != null && maxIndexKunden.Count > 0)
					{
						_lastIndex.KundenIndex = maxIndexKunden[0].KundenIndex;
						_lastIndex.KundenIndexDate = maxIndexKunden[0].KundenIndexDate;
					}
				}
				else
				{
					_lastIndex.KundenIndex = articleEntity.Index_Kunde;
					_lastIndex.KundenIndexDate = articleEntity.Index_Kunde_Datum;
				}
			}

			if(existingTransaction == false)
			{
				transactionManager.commit();
			}
			// - 
			return _lastIndex;
		}

		public ResponseModel<int> Perform(bool sharedTransaction = false)
		{
			if(sharedTransaction == false)
			{
				_botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				//opening sql transaction
				_botransaction.beginTransaction();
			}

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			lock(Locks.Locks.OrdersLock)
			{
				try
				{
					int newPositionNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetMaxPositionNumberByOrderId(_data.OrderId, _botransaction.connection, _botransaction.transaction) + 10;
					var orderData = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data.OrderId, _botransaction.connection, _botransaction.transaction);
					var itemDB = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumberWithTransaction(_data.ItemNumber, null, _botransaction.connection, _botransaction.transaction);
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)orderData.Kunden_Nr, _botransaction.connection, _botransaction.transaction);
					//calculating prices
					var discount = 0m;
					var unitPriceBasis = Convert.ToDecimal(itemDB.Preiseinheit ?? 0m);
					var fixedPrice = itemDB.VKFestpreis ?? false;
					var cuWeight = Convert.ToDecimal(itemDB.CuGewicht ?? 0);
					var del = (itemDB.DEL ?? 0);
					var freeText = "";

					var me1 = 0m;
					var me2 = 0m;
					var me3 = 0m;
					var me4 = 0m;
					var pm1 = 0m;
					var pm2 = 0m;
					var pm3 = 0m;
					var pm4 = 0m;
					var verkaufspreis = 0m;

					var itemPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDB.ArtikelNr, _botransaction.connection, _botransaction.transaction);
					if(itemPricingGroupsDb != null)
					{
						me1 = Convert.ToDecimal(itemPricingGroupsDb.ME1 ?? 0m);
						me2 = Convert.ToDecimal(itemPricingGroupsDb.ME2 ?? 0m);
						me3 = Convert.ToDecimal(itemPricingGroupsDb.ME3 ?? 0m);
						me4 = Convert.ToDecimal(itemPricingGroupsDb.ME4 ?? 0m);
						pm1 = Convert.ToDecimal(itemPricingGroupsDb.PM1 ?? 0m);
						pm2 = Convert.ToDecimal(itemPricingGroupsDb.PM2 ?? 0m);
						pm3 = Convert.ToDecimal(itemPricingGroupsDb.PM3 ?? 0m);
						pm4 = Convert.ToDecimal(itemPricingGroupsDb.PM4 ?? 0m);
						verkaufspreis = Convert.ToDecimal(itemPricingGroupsDb.Verkaufspreis ?? 0m);
					}
					// - 2023-09-29 - Reil price according to article type (Prototype, FirstSample, Serie, NullSerie)
					int ARTICLE_SERIE_ID = 4;
					if(_data.ItemTypeId != ARTICLE_SERIE_ID)
					{
						//var articleType = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndType(itemDB.ArtikelNr, (int)BaseData.Enums.ArticleEnums.GetItemType(((CustomerService.Enums.OrderEnums.ItemType)(_data.ItemTypeId ?? ARTICLE_SERIE_ID)).GetDescription()), _botransaction.connection, _botransaction.transaction);
						//verkaufspreis = articleType?.Verkaufspreis is null ? verkaufspreis : Convert.ToDecimal(articleType?.Verkaufspreis ?? 0m);
					}
					var singleCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedPrice,
							del,
							cuWeight);

					var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedPrice,
						(decimal)_data.Quantity,
						singleCopperSurcharge);

					var vkUnitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedPrice,
						verkaufspreis,
						(decimal)_data.Quantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(fixedPrice,
						unitPriceBasis,
						(decimal)_data.Quantity,
						vkUnitPrice,
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
						(decimal)_data.Quantity,
						discount);

					var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
						vkUnitPrice,
						(decimal)_data.Quantity);

					var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight((decimal)_data.Quantity,
						cuWeight);

					var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(itemDB.ArtikelNr, _botransaction.connection, _botransaction.transaction);
					var articleProductionPlace = articleProductionExtensionEntity?.ProductionPlace1_Id;
					var storageLocation = new KeyValuePair<int, string>();
					//if(articleProductionPlace != null)
					//{
					//	storageLocation = Enums.OrderEnums.GetArticleHauplager((Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)articleProductionPlace);
					//}
					if(articleProductionPlace != null)
					{
						storageLocation = Enums.OrderEnums.GetArticleHauplager((Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)articleProductionPlace);
						// - 2025-01-16 - Reil - use Hauptlager WS for articles with production place TN
						if(articleProductionPlace == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.TN)
						{
							storageLocation = Enums.OrderEnums.GetArticleHauplager(Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.WS);
						}
					}
					var _lastIndex = GetLastIndex(itemDB, _botransaction);


					var orderElementDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity()
					{
						// TODO Verif: Order Element Type :  Prototyp - Erstmuster - Nullserie - Serie + Bezug +Bemerkung [Type Position]
						Typ = _data.ItemTypeId, // (int)Enums.OrderEnums.Types.Confirmation, // <<<  
						AngebotNr = _data.OrderId,
						Position = _data.Position.HasValue ? _data.Position : newPositionNumber,
						//newPositionNumber,
						Wunschtermin = null,
						//orderData.Wunschtermin ?? DateTime.Now.AddDays(+30),
						Anzahl = _data.Quantity,
						Abladestelle = "",
						Bezeichnung2_Kunde = itemDB.Bezeichnung2,
						OriginalAnzahl = _data.Quantity,
						Freies_Format_EDI = "",

						EDI_PREIS_KUNDE = 0m,
						EDI_PREISEINHEIT = Convert.ToDecimal(itemDB.Preiseinheit ?? 0),
						EDI_Quantity_Ordered = _data.Quantity,
						EDI_Historie_Nr = null,

						LieferanweisungP_FTXDIN_TEXT = "",
						Bemerkungsfeld1 = "",
						Bemerkungsfeld2 = "",

						Bezeichnung1 = itemDB.Bezeichnung1,
						Bezeichnung2 = itemDB.Bezeichnung2,
						Bezeichnung3 = "-",
						Einheit = itemDB.Einheit,
						ArtikelNr = itemDB.ArtikelNr,

						Kupferbasis = 150,

						Preiseinheit = Convert.ToDecimal(itemDB.Preiseinheit ?? 1), // - 2022-05-30 Init to 1 as to respect DB Constraint
						DELFixiert = itemDB.DELFixiert ?? false,
						DEL = itemDB.DEL ?? 0,
						EinzelCuGewicht = Convert.ToDecimal(itemDB.CuGewicht ?? 0),
						VKFestpreis = fixedPrice,
						USt = customerDb.Umsatzsteuer_berechnen.HasValue && customerDb.Umsatzsteuer_berechnen.Value ? 0.19m : 0m,//Convert.ToDecimal(itemDB.Umsatzsteuer ?? 0),
						Einzelkupferzuschlag = singleCopperSurcharge,
						GesamtCuGewicht = totalCuWeight,
						Einzelpreis = unitPrice,
						VKEinzelpreis = vkUnitPrice,
						Gesamtpreis = totalPrice,
						Gesamtkupferzuschlag = totalCopperSurcharge,
						VKGesamtpreis = vKTotalPrice,
						erledigt_pos = false,
						// Fertigungsnummer = itemData.f // > ReferencedDocument, ignored for now
						POSTEXT = _data.Headline,
						Geliefert = 0, // << compatibility with psz soft
						Rabatt = 0, // << compatibility with psz soft
						Index_Kunde = _lastIndex.KundenIndex, //  itemDB.Index_Kunde, // kundenIndexes == null || kundenIndexes.Count <=0? "": maxIndexKunden.Value.Value,
						Index_Kunde_Datum = _lastIndex.KundenIndexDate, // itemDB.Index_Kunde_Datum, // kundenIndexes == null || kundenIndexes.Count <=0? null: maxIndexKunden.Value.Key,
						Fertigungsnummer = 0,
						Lagerort_id = storageLocation.Key,
						CSInterneBemerkung = _data.CSInterneBemerkung,
						ABPoszuRAPos = null,
					};
					var insertedId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(orderElementDb, _botransaction.connection, _botransaction.transaction);
					//order item extension
					Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity
					{
						Id = -1,

						OrderId = _data.OrderId,
						OrderItemId = insertedId,
						Status = (int)Enums.OrderEnums.OrderElementStatus.Original,
						OriginalQuantity = (decimal)_data.Quantity,
						OriginalGesamtpreis = 0m,
						OriginalVKGesamtpreis = -1,
						DesiredDate = orderData.Wunschtermin ?? DateTime.Now.AddDays(+30),
						CreationDate = DateTime.Now,
						CreationUserId = (_user?.Id ?? -1),

						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = (_user?.Id ?? -1),
						LastUpdateUsername = (_user?.Username ?? "-"),
						Version = 0,
					}, _botransaction.connection, _botransaction.transaction);

					//logging
					var InsertedItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(insertedId, _botransaction.connection, _botransaction.transaction);
					var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction((int)InsertedItem.AngebotNr, _botransaction.connection, _botransaction.transaction);
					var _log = new LogHelper(Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ, LogHelper.LogType.CREATIONPOS, "CTS", _user)
						.LogCTS(null, null, null, (int)InsertedItem.Position, InsertedItem.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, _botransaction.connection, _botransaction.transaction);

					if(sharedTransaction == true)
					{
						return ResponseModel<int>.SuccessResponse(insertedId);
					}
					else
					{
						if(_botransaction.commit())
						{
							return ResponseModel<int>.SuccessResponse(insertedId);
						}
						else
						{
							return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
						}
					}
				} catch(Exception e)
				{
					_botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
