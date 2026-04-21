using Infrastructure.Services.Utils;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class SaveDelforHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private DeliveryForcastModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SaveDelforHandler(Identity.Models.UserModel user, DeliveryForcastModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				Helpers.DelforHelper.ValidateDelforData(_data.LineItems, out List<string> warnings, _user);
				if(warnings != null && warnings.Count > 0)
					return ResponseModel<int>.FailureResponse(warnings);
				var BOTransaction = new TransactionsManager();
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data.Header.CustomerId);
				var contact = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(_data.Header.CustomerId);
				var lastVersion = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetLastVersion(adress.Nr, _data.Header.DocumentNumber);
				var isDeliveryAddressSelected = !$"{this._data.Header.CustomerAdressStreet}{this._data.Header.CustomerAdressCity}{this._data.Header.CustomerAdressPostCode}".StringIsNullOrEmptyOrWhiteSpaces();

				var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adress.Nr, BOTransaction.connection, BOTransaction.transaction);
				var deliveryAddress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerEntity.LSADR ?? -1, BOTransaction.connection, BOTransaction.transaction)
					?? new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
					{
						Ort = "",
						Fax = "",
						Telefon = "",
						Land = "",
						Name1 = "",
						Postfach = "",
						StraBe = "",
					};
				var headerEntity = new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity
				{
					ReceivingDate = _data.Header.Date,
					DocumentNumber = _data.Header.DocumentNumber.Trim(),
					CreationTime = DateTime.Now,
					MessageType = "DELFOR",
					ValidFrom = _data.Header.ValidFrom,
					ValidTill = _data.Header.ValidTo,
					BuyerPartyName = _data.Header.CustomerName,
					BuyerContactName = _data.Header.CustomerContactName ?? "",
					BuyerContactTelephone = _data.Header.CustomerContactPhone,
					BuyerDUNS = adress.Duns,
					//SupplierDUNS=
					SupplierPartyName = "PSZ electronic GmbH",
					SupplierStreet = "Im Gstaudach 6",
					SupplierCity = "Vohenstrauß",
					SupplierPostCode = "92648",
					SupplierCountryName = "DE",
					SupplierContactTelephone = "09651924117147",
					SupplierContactFax = "09651924117214",
					ManualCreation = true,
					BuyerPurchasingDepartment = "",
					ConsigneeCity = isDeliveryAddressSelected ? _data.Header.CustomerAdressCity : deliveryAddress?.Ort ?? "", // - adress.Ort,
					ConsigneeContactFax = isDeliveryAddressSelected ? _data.Header.CustomerAdressFax : deliveryAddress?.Fax, // -adress.Fax,
					ConsigneeContactTelephone = isDeliveryAddressSelected ? _data.Header.CustomerAdressTelephone : deliveryAddress?.Telefon, // -adress.Telefon,
					ConsigneeCountryName = isDeliveryAddressSelected ? _data.Header.CustomerAdressCountry : deliveryAddress?.Land, // -adress.Land,
					ConsigneeDUNS = "",
					ConsigneePartyIdentification = _data.Header.CustomerAdressCustomerNumber ?? "", // -adress.Kundennummer.ToString(),
					ConsigneePartyName = isDeliveryAddressSelected ? _data.Header.CustomerAddressName : deliveryAddress?.Name1, // -adress.Name1,
					ConsigneePostCode = isDeliveryAddressSelected ? _data.Header.CustomerAdressPostCode : deliveryAddress?.PLZ_StraBe, // -adress.Postfach,
					ConsigneeStreet = isDeliveryAddressSelected ? _data.Header.CustomerAdressStreet : deliveryAddress?.StraBe, // - adress.StraBe,
					MessageReferenceNumber = "",
					PreviousReferenceVersionNumber = lastVersion != null ? lastVersion.ReferenceVersionNumber : null,
					RecipientId = "0",
					ReferenceNumber = "",
					ReferenceVersionNumber = lastVersion != null ? lastVersion.ReferenceVersionNumber + 1 : 0,
					SenderId = "",
					SupplierDUNS = "",
					SupplierPartyIdentification = "",
					PSZCustomernumber = _data.Header.CustomerId,
				};
				BOTransaction.beginTransaction();
				var headerID = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.InsertWithTransaction(headerEntity, BOTransaction.connection, BOTransaction.transaction);
				var lineItemPlansToInsert = new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
				var errors = new List<string>();
				foreach(var lineItem in _data.LineItems)
				{
					var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(lineItem.PSZ_Artikelnummer);
					var _lineItem = new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity
					{
						HeaderId = headerID,
						DocumentNumber = _data.Header.DocumentNumber,
						PositionNumber = int.TryParse(lineItem.Position, out var ps) ? ps : 0,
						CumulativeReceivedQuantity = lineItem.Gelieferte_Menge,
						CumulativeScheduledQuantity = lineItem.Eingeteilte_Menge,
						LastReceivedQuantity = lineItem.Letzter_Wareneing,
						LastASNNumber = lineItem.Lieferscheinnummer.HasValue ? lineItem.Lieferscheinnummer.ToString() : null,
						LastASNDeliveryDate = lineItem.Am,
						LastASNDate = lineItem.Am,
						CustomersItemMaterialNumber = article?.Bezeichnung1,
						SuppliersItemMaterialNumber = article?.ArtikelNummer,
						DrawingRevisionNumber = article.Index_Kunde,
						HeaderVersion = headerEntity.ReferenceVersionNumber,
						HeaderPreviousVersion = headerEntity.PreviousReferenceVersionNumber,
						ArticleId = article.ArtikelNr
					};
					var lineItemId = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.InsertWithTransaction(_lineItem, BOTransaction.connection, BOTransaction.transaction);
					var lineItemPlanPositionNumber = 10;
					foreach(var lineItemPlan in lineItem.LineItemPlans)
					{
						var dates = Helpers.DelforHelper.GetItemPlanDates(lineItemPlan.Period, lineItemPlan.Liefertermin);
						if((dates.Key.HasValue && dates.Key.Value < new DateTime(1900, 1, 1)) || (dates.Value.HasValue && dates.Value.Value < new DateTime(1900, 1, 1)))
						{
							errors.Add($"Pos {lineItem.Position} | {lineItem.PSZ_Artikelnummer} : Invalid date [{lineItemPlan.Period} | {lineItemPlan.Liefertermin}]");
							continue;
						}
						var _lineItemPlan = new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity
						{
							LineItemId = lineItemId,
							PositionNumber = lineItemPlanPositionNumber,
							PlanningQuantityCumulativeQuantity = lineItemPlan.Einteilungs_FZ,
							PlanningQuantityUnitOfMeasure = article.Einheit,
							PlanningQuantityFrequencyIdentifier = lineItemPlan.Period,
							PlanningQuantityQuantity = lineItemPlan.Menge,
							PlanningQuantityChange = lineItemPlan.Abw,
							PlanningQuantityRequestedShipmentDate = dates.Key,
							PlanningQuantityWeeklyPeriodEndDate = dates.Value
						};
						lineItemPlansToInsert.Add(_lineItemPlan);
						lineItemPlanPositionNumber += 10;
					}
				}

				if(errors.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(errors);
				}
				Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.InsertWithTransaction(lineItemPlansToInsert, BOTransaction.connection, BOTransaction.transaction);
				if(BOTransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
				{
					BOTransaction.rollback();
					return ResponseModel<int>.FailureResponse("Error in Transaction");
				}
			} catch(Exception e)
			{
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
			if(_data.LineItems == null || _data.LineItems.Count == 0)
				return ResponseModel<int>.FailureResponse("Delfor items empty.");
			var empty_plans = _data.LineItems.Where(x => x.LineItemPlans == null || x.LineItemPlans.Count == 0).ToList();
			if(empty_plans != null && empty_plans.Count > 0)
				return ResponseModel<int>.FailureResponse($"Delfor Position(s) [{string.Join(",", empty_plans.Select(x => x.Position).ToList())}] have no plans.");


			var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data.Header.CustomerId);
			var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adress.Nr);
			var deliveryAddress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerEntity.LSADR ?? -1);
			var isDeliveryAddressSelected = !$"{this._data.Header.CustomerAdressStreet}{this._data.Header.CustomerAdressCity}{this._data.Header.CustomerAdressPostCode}".StringIsNullOrEmptyOrWhiteSpaces();
			if(!isDeliveryAddressSelected && (customerEntity.LSADR == null || customerEntity.LSADR == 0 || deliveryAddress == null))
				return ResponseModel<int>.FailureResponse($"Current Lieferadresse (LSADR) is invalid.");


			return ResponseModel<int>.SuccessResponse();
		}

	}
}
