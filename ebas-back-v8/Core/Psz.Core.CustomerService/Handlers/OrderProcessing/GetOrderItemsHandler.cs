using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetOrderItemsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<OrderItemModel>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetOrderItemsHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<OrderItemModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var response = new List<OrderItemModel>();
				var elementsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data, false);
				var elementsIds = elementsDb.Select(e => e.Nr).ToList();

				var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(elementsIds);
				var orderItemsIds = orderItemsDb?.Select(e => e.Nr)?.ToList();
				var orderItemsExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderItemsIds(orderItemsIds);

				var itemsIds = orderItemsDb?.Where(e => e.ArtikelNr.HasValue)?.Select(e => e.ArtikelNr.Value)?.ToList();
				var itemsDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemsIds);

				var storaLocationsIds = orderItemsDb?.Where(e => e.Lagerort_id.HasValue)?.Select(e => e.Lagerort_id.Value)?.ToList();
				var storageLocationsDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(storaLocationsIds);

				var ordersIds = orderItemsDb?.Where(e => e.AngebotNr.HasValue)?.Select(e => e.AngebotNr.Value)?.ToList();
				var ordersChangesDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.GetByOrdersIds(ordersIds);
				var ordersChangesIds = ordersChangesDb?.Select(e => e.Id)?.ToList();

				string IslSoRAB = string.Empty;
				var AngeboteNr = (orderItemsDb != null && orderItemsDb.Count > 0)
					? orderItemsDb[0].AngebotNr
					: null;
				var ABOrLS = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(AngeboteNr ?? -1);
				IslSoRAB = ABOrLS?.Typ;
				foreach(var orderItemDb in orderItemsDb)
				{
					var itemDb = orderItemDb.ArtikelNr.HasValue
						? itemsDb.Find(e => e.ArtikelNr == orderItemDb.ArtikelNr.Value)
						: null;

					Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity OroginalOrderOrLSItem = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity();
					var storageLocationDb = storageLocationsDb?.Find(e => e.LagerortId == orderItemDb.Lagerort_id);
					var orderItemsExtensionDb = orderItemsExtensionsDb?.Find(e => e.OrderItemId == orderItemDb.Nr);
					if(orderItemDb.LSPoszuABPos.HasValue && orderItemDb.LSPoszuABPos.Value != 0 && orderItemDb.LSPoszuABPos.Value != -1)
					{
						OroginalOrderOrLSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)orderItemDb.LSPoszuABPos);
					}
					var CalculatedValue = (orderItemDb.Preiseinheit.HasValue && orderItemDb.Preiseinheit.Value > 0) ?
								(orderItemDb.OriginalAnzahl ?? 0 / orderItemDb.Preiseinheit.Value) * orderItemDb.Einzelpreis ?? 0 * (1 - orderItemDb.Rabatt ?? 0)
								: 0;
					var order = new OrderItemModel()
					{
						Id = orderItemDb.Nr,
						OrderNumber = orderItemDb.AngebotNr?.ToString(),
						OrderId = orderItemDb.AngebotNr ?? -1,
						Done = (orderItemDb.erledigt_pos ?? false),
						// >>>>>>>>>>>>>>>>
						ItemId = orderItemDb.ArtikelNr ?? -1,
						ItemNumber = itemDb?.ArtikelNummer,
						RP = orderItemDb.RP ?? false,
						Position = orderItemDb.Position ?? 0,
						OpenQuantity_Quantity = Convert.ToDecimal(orderItemDb.Anzahl ?? 0),
						DesiredDate = orderItemDb.Wunschtermin,
						DeliveryDate = orderItemDb.Liefertermin,
						StorageLocationId = storageLocationDb != null ? storageLocationDb.LagerortId : -1,
						StorageLocationName = storageLocationDb?.Lagerort,
						FixedUnitPrice = orderItemDb.VKFestpreis ?? false, // <<<<<< !
						UnitPrice = Convert.ToDecimal(orderItemDb.VKEinzelpreis ?? 0),
						FixedTotalPrice = orderItemDb.VKFestpreis ?? false,
						TotalPrice = Convert.ToDecimal(orderItemDb.VKGesamtpreis ?? 0),
						UnitPriceBasis = Convert.ToDecimal(orderItemDb.Preiseinheit ?? 0),
						Discount = Convert.ToDecimal(orderItemDb.Rabatt ?? 0),
						VAT = Convert.ToDecimal(orderItemDb.USt ?? 0),
						// >>>>>>>>>>>>>>
						Designation1 = orderItemDb?.Bezeichnung1,
						Designation2 = orderItemDb?.Bezeichnung2,
						Designation3 = itemDb?.Bezeichnung3,
						MeasureUnitQualifier = itemDb.Einheit,
						DrawingIndex = itemDb.Zeichnungsnummer,
						CopperBase = orderItemDb.Kupferbasis ?? 0,
						DelFixed = orderItemDb.DELFixiert ?? false,
						DelNote = orderItemDb.DEL ?? 0,
						CopperWeight = Convert.ToDecimal(orderItemDb.EinzelCuGewicht ?? 0),
						CopperSurcharge = Convert.ToDecimal(orderItemDb.Einzelkupferzuschlag ?? 0),
						ProductionNumber = (orderItemDb.Fertigungsnummer ?? 0),
						// >>>>>>>>>>>>>>>>
						UnloadingPoint = orderItemDb.Abladestelle,
						OpenQuantity_UnitPrice = Convert.ToDecimal(orderItemDb.Einzelpreis ?? 0),
						OpenQuantity_TotalPrice = Convert.ToDecimal(orderItemDb.Gesamtpreis ?? 0),
						OpenQuantity_CopperWeight = Convert.ToDecimal(orderItemDb.GesamtCuGewicht ?? 0),
						OpenQuantity_CopperSurcharge = Convert.ToDecimal(orderItemDb.Gesamtkupferzuschlag ?? 0),
						OriginalOrderQuantity = Convert.ToDecimal(orderItemDb.OriginalAnzahl ?? 0),
						OriginalOrderAmount = (IslSoRAB == "Lieferschein") ? CalculatedValue : Convert.ToDecimal(orderItemDb.VKGesamtpreis ?? 0),
						DeliveredQuantity = (IslSoRAB == "Lieferschein") ? OroginalOrderOrLSItem?.Geliefert ?? 0
						: Convert.ToDecimal(orderItemDb.Geliefert ?? 0),
						// >>>>>>>>>>>>>>>>
						FreeText = orderItemDb.Freies_Format_EDI,
						Note1 = orderItemDb.Bemerkungsfeld1,
						Note2 = orderItemDb.Bemerkungsfeld2,
						Version = (orderItemsExtensionDb?.Version ?? 0),

						 
					};

					//if(_UpdateChoice)
					//{
					//	var StorageLocation = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data).Lagerort_id;
					//	var GetStorageLocationName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetLagerort(StorageLocation??0);
					//	order.StorageLocationName = GetStorageLocationName;
					//	order.DesiredDate = order.DeliveryDate;
					//}
					response.Add(order);
				}
				return ResponseModel<List<OrderItemModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<OrderItemModel>> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<List<OrderItemModel>>.AccessDeniedResponse();
			}
			var orderDB = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(orderDB == null)
				return ResponseModel<List<OrderItemModel>>.FailureResponse(key: "1", value: $"Order not found");

			return ResponseModel<List<OrderItemModel>>.SuccessResponse();
		}
	}
}
