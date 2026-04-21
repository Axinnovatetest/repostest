using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class UpdateOrderItemHandler: IHandle<Identity.Models.UserModel, ResponseModel<object>>
	{
		private UpdateOrderItemModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateOrderItemHandler(UpdateOrderItemModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<object> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				lock(Locks.Locks.OrdersLock)
				{
					var warnings = new List<string>();
					var orderItemDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Id);
					var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data.ItemNumber);
					var storageLocationDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_data.StorageLocationId);

					if(orderItemDb.Anzahl != (decimal?)_data.OrderedQuantity && _data.OrderedQuantity == 0)
					{
						warnings.Add("Ordered Quantity is 0!");
						warnings.Add("The position will be send as order annulation.");
					}
					//prices calculation
					var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDb.ArtikelNr);
					var discount = _data.Discount;
					var fixedPrice = _data.FixedTotalPrice;
					var unitPriceBasis = _data.UnitPriceBasis;
					var cuWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0);
					var del = (_data.DelNote ?? 0); //  (itemDb.DEL ?? 0);

					var me1 = 0m;
					var me2 = 0m;
					var me3 = 0m;
					var me4 = 0m;
					var pm1 = 0m;
					var pm2 = 0m;
					var pm3 = 0m;
					var pm4 = 0m;
					var verkaufspreis = 0m;
					var kupferbasis = _data.CopperBase;

					if(itemPricingGroupDb != null)
					{
						me1 = Convert.ToDecimal(itemPricingGroupDb.ME1 ?? 0m);
						me2 = Convert.ToDecimal(itemPricingGroupDb.ME2 ?? 0m);
						me3 = Convert.ToDecimal(itemPricingGroupDb.ME3 ?? 0m);
						me4 = Convert.ToDecimal(itemPricingGroupDb.ME4 ?? 0m);
						pm1 = Convert.ToDecimal(itemPricingGroupDb.PM1 ?? 0m);
						pm2 = Convert.ToDecimal(itemPricingGroupDb.PM2 ?? 0m);
						pm3 = Convert.ToDecimal(itemPricingGroupDb.PM3 ?? 0m);
						pm4 = Convert.ToDecimal(itemPricingGroupDb.PM4 ?? 0m);
						verkaufspreis = Convert.ToDecimal(itemPricingGroupDb.Verkaufspreis ?? 0m);
					}
					// - 2023-09-29 - Reil price according to article type (Prototype, FirstSample, Serie, NullSerie)
					int ARTICLE_SERIE_ID = 4;
					if(_data.ItemTypeId != ARTICLE_SERIE_ID)
					{
						//var articletype = infrastructure.data.access.tables.bsd.artikelsalesextensionaccess.getbyarticlenrandtype(itemdb.artikelnr, (int)basedata.enums.articleenums.getitemtype(((customerservice.enums.orderenums.itemtype)(_data.itemtypeid ?? article_serie_id)).getdescription()));
						//verkaufspreis = articletype?.verkaufspreis is null ? verkaufspreis : convert.todecimal(articletype?.verkaufspreis ?? 0m);
					}
					// - Einzelkupferzuschlag
					var singleCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedPrice,
						del,
						cuWeight,
						kupferbasis);

					// - Gesamtkupferzuschlag
					var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedPrice,
						_data.OrderedQuantity,
						singleCopperSurcharge);

					// - VKEinzelpreis
					var vkUnitPrice = _data.FixedPrice ? _data.UnitPrice : Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedPrice,
						verkaufspreis,
						_data.OrderedQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					// - Einzelpreis
					var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(_data.FixedPrice, //fixedPrice,
						unitPriceBasis,
						_data.OrderedQuantity,
						_data.FixedPrice ? vkUnitPrice + singleCopperSurcharge : vkUnitPrice,
						verkaufspreis,
						singleCopperSurcharge,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					// - Gesamtpreis
					var totalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateTotalPrice(unitPriceBasis,
						unitPrice,
						_data.OrderedQuantity,
						discount);

					// - VKGesamtpreis
					var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
						vkUnitPrice,
						_data.OrderedQuantity);

					// - GesamtCuGewicht
					var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight(_data.OrderedQuantity,
						cuWeight);

					orderItemDb.OriginalAnzahl = (decimal)_data.OrderedQuantity;
					orderItemDb.Typ = _data.ItemTypeId;
					orderItemDb.Position = _data.PositionNumber;
					orderItemDb.Wunschtermin = _data.DesiredDate;
					orderItemDb.Anzahl = (decimal)_data.OrderedQuantity;
					orderItemDb.AktuelleAnzahl = (decimal)_data.OrderedQuantity;
					orderItemDb.Abladestelle = _data.UnloadingPoint;
					//orderItemDb.Bezeichnung2_Kunde = itemDb.Bezeichnung2;
					orderItemDb.Freies_Format_EDI = _data.FreeText;
					orderItemDb.Bemerkungsfeld1 = _data.Note1;
					orderItemDb.Bemerkungsfeld2 = _data.Note2;
					orderItemDb.Einheit = _data.MeasureUnitQualifier;
					orderItemDb.ArtikelNr = itemDb.ArtikelNr;
					orderItemDb.Kupferbasis = kupferbasis;// 150;
					orderItemDb.Preiseinheit = unitPriceBasis == 0 ? 1 : unitPriceBasis; // - 2022-05-30 - init to 1 to respect DB Constraint
					orderItemDb.DELFixiert = _data.DelFixed; // itemDb.DELFixiert;
					orderItemDb.DEL = _data.DelNote; //_data.DelFixed.HasValue && !data.DelFixed.Value ?_data.DelNote: itemDb.DEL; // itemDb.DEL;
					orderItemDb.EinzelCuGewicht = itemDb.CuGewicht;
					orderItemDb.VKFestpreis = fixedPrice;
					orderItemDb.USt = (decimal)_data.VAT; //itemDb.Umsatzsteuer;
					orderItemDb.Einzelkupferzuschlag = (decimal)singleCopperSurcharge;
					orderItemDb.GesamtCuGewicht = (decimal)totalCuWeight;
					orderItemDb.Einzelpreis = (decimal)unitPrice;
					orderItemDb.VKEinzelpreis = (decimal)vkUnitPrice;
					orderItemDb.Gesamtpreis = (decimal)totalPrice;
					orderItemDb.Gesamtkupferzuschlag = (decimal)totalCopperSurcharge;
					orderItemDb.VKGesamtpreis = (decimal)vKTotalPrice;
					orderItemDb.Lagerort_id = storageLocationDb != null
						? storageLocationDb.LagerortId
						: (int?)null;
					orderItemDb.Liefertermin = _data.DeliveryDate;
					orderItemDb.RP = _data.RP;
					orderItemDb.EKPreise_Fix = _data.FixedPrice;
					orderItemDb.POSTEXT = _data.Postext;
					orderItemDb.Bezeichnung1 = _data.Designation1;
					orderItemDb.Bezeichnung2 = _data.Designation2;
					orderItemDb.Index_Kunde = _data.Index_Kunde;
					orderItemDb.Index_Kunde_Datum = _data.Index_Kunde_Datum;
					orderItemDb.CSInterneBemerkung = _data.CSInterneBemerkung;

					var _oldPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Id);
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(orderItemDb);
					Helpers.ItemElementHelper.SetStatus(orderItemDb.Nr);

					//logging
					var _Logs = GetLog(orderItemDb, _oldPosition, _user);
					if(_Logs != null && _Logs.Count > 0)
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_Logs);

					return new ResponseModel<object>
					{
						Success = true,
						Errors = null,
						Warnings = warnings,
						Infos = null,
						Body = null
					};
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<object> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<object>.AccessDeniedResponse();
			}
			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data.ItemNumber);
			if(itemDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Item not found");
			if(itemDb.Freigabestatus.ToUpper() == "O")
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Item is 'Obsolete'");
			var itemWsamePositionNr = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrAndPosition(_data.Id, _data.OrderId, _data.PositionNumber);
			if(itemWsamePositionNr != null && itemWsamePositionNr.Count > 0)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Position [{_data.PositionNumber}] already exists in Order");
			var ItemOrder = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.OrderId);
			if(ItemOrder == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Order not found.");
			else
			{
				if(ItemOrder.Gebucht.HasValue && ItemOrder.Gebucht.Value)
					return ResponseModel<object>.FailureResponse(key: "1", value: $"Order is Booked, modifications impossible.");
				if(ItemOrder.Erledigt.HasValue && ItemOrder.Erledigt.Value)
					return ResponseModel<object>.FailureResponse(key: "1", value: $"Order is Done, modifications impossible.");
			}
			var orderItemDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Id);
			if(orderItemDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Order Element not found");
			if(orderItemDb.AngebotNr != ItemOrder.Nr)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Element is not Element of Order");
			var customerDb = ItemOrder.Kunden_Nr.HasValue
				? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(ItemOrder.Kunden_Nr.Value)
				: null;
			if(customerDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Customer not found");
			var storageLocationDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_data.StorageLocationId);
			if(storageLocationDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Storage Location not found");
			if(_data.UnitPriceBasis <= 0)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"UnitPriceBasis " + _data.UnitPriceBasis + " is invalid");
			if(_data.OrderedQuantity < 0)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Ordered Quantity " + _data.OrderedQuantity + " is invalid");
			var originalPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)orderItemDb.PositionZUEDI);
			if(originalPosition == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Original Position " + orderItemDb.PositionZUEDI + " is not found");
			else if(originalPosition.ArtikelNr != orderItemDb.ArtikelNr)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Split Position should have same Article as Original.");

			return ResponseModel<object>.SuccessResponse();
		}
		internal static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLog(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _new,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _old, Core.Identity.Models.UserModel user)
		{
			var _object = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_old.AngebotNr ?? -1);
			var _newArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_new.ArtikelNr ?? -1);
			var _oldArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_old.ArtikelNr ?? -1);
			var _oldStorageLocation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_old.Lagerort_id ?? -1);
			var _newStorageLocation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_new.Lagerort_id ?? -1);
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var _toLog = new Helpers.LogHelper(_object.Nr, (int)_object.Angebot_Nr, int.TryParse(_object.Projekt_Nr, out var val) ? val : 0, _object.Typ, Helpers.LogHelper.LogType.MODIFICATIONPOS, "CTS", user);
			if(_old.Kupferbasis != _new.Kupferbasis)
			{
				_logs.Add(_toLog.LogCTS("Kupferbasis", _old.Kupferbasis.ToString(), _new.Kupferbasis.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.DEL != _new.DEL)
			{
				_logs.Add(_toLog.LogCTS("DEL", _old.DEL.ToString(), _new.DEL.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Liefertermin != _new.Liefertermin)
			{
				_logs.Add(_toLog.LogCTS("Liefertermin", _old.Liefertermin.ToString(), _new.Liefertermin.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Bezeichnung1 != _new.Bezeichnung1)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung1", _old.Bezeichnung1.ToString(), _new.Bezeichnung1.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Bezeichnung2 != _new.Bezeichnung2)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung2", _old.Bezeichnung2.ToString(), _new.Bezeichnung2.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Wunschtermin != _new.Wunschtermin)
			{
				_logs.Add(_toLog.LogCTS("Wunschtermin", _old.Wunschtermin.ToString(), _new.Wunschtermin.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.EKPreise_Fix.HasValue && _old.EKPreise_Fix != _new.EKPreise_Fix)
			{
				_logs.Add(_toLog.LogCTS("EKPreise_Fix", _old.EKPreise_Fix.ToString(), _new.EKPreise_Fix.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.VKFestpreis != _new.VKFestpreis)
			{
				_logs.Add(_toLog.LogCTS("VKFestpreis", _old.VKFestpreis.ToString(), _new.VKFestpreis.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Freies_Format_EDI != _new.Freies_Format_EDI)
			{
				_logs.Add(_toLog.LogCTS("Freies_Format_EDI", _old.Freies_Format_EDI.ToString(), _new.Freies_Format_EDI.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.ArtikelNr != _newArticle.ArtikelNr)
			{
				_logs.Add(_toLog.LogCTS("Artikel", _oldArticle.ArtikelNummer, _newArticle.ArtikelNummer.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Bemerkungsfeld1 != _new.Bemerkungsfeld1)
			{
				_logs.Add(_toLog.LogCTS("Bemerkungsfeld1", _old.Bemerkungsfeld1.ToString(), _new.Bemerkungsfeld1.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Bemerkungsfeld2 != _new.Bemerkungsfeld2)
			{
				_logs.Add(_toLog.LogCTS("Bemerkungsfeld2", _old.Bemerkungsfeld2.ToString(), _new.Bemerkungsfeld2.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Position != _new.Position)
			{
				_logs.Add(_toLog.LogCTS("Position", _old.Position.ToString(), _new.Position.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Lagerort_id != _new.Lagerort_id)
			{
				_logs.Add(_toLog.LogCTS("Lagerort", _oldStorageLocation?.Lagerort, _newStorageLocation?.Lagerort, (int)_old.Position, _old.Nr));
			}
			if(_old.VKEinzelpreis != _new.VKEinzelpreis)
			{
				_logs.Add(_toLog.LogCTS("VKEinzelpreis", _old.VKEinzelpreis.ToString(), _new.VKEinzelpreis.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Preiseinheit != _new.Preiseinheit)
			{
				_logs.Add(_toLog.LogCTS("Preiseinheit", _old.Preiseinheit.ToString(), _new.Preiseinheit.ToString(), (int)_old.Position, _old.Nr));
			}
			if(_old.Abladestelle != _new.Abladestelle)
			{
				_logs.Add(_toLog.LogCTS("Abladestelle", _old.Abladestelle, _new.Abladestelle, (int)_old.Position, _old.Nr));
			}
			if(_old.USt != _new.USt)
			{
				_logs.Add(_toLog.LogCTS("USt", _old.USt.ToString(), _new.USt.ToString(), (int)_old.Position, _old.Nr));
			}
			return _logs;
		}
	}
}
