using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class UpdateHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private Models.Supplier.UpdateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateHandler(Models.Supplier.UpdateModel data,
			Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
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

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id, botransaction.connection, botransaction.transaction);
				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)lieferantenEntity.Nummer, botransaction.connection, botransaction.transaction);

				_data.AddressNr = adressenEntity.Nr;
				_data.AddressType = adressenEntity.Adresstyp ?? -1;
				var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateSupplier(_data.ToAdressenEntity(adressenEntity.Lieferantennummer), botransaction.connection, botransaction.transaction);

				var _addressenExtensionEntity = _data.ToAdressenExtensionEntity();
				var addressenExtensionEntity = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(adressenEntity.Nr, botransaction.connection, botransaction.transaction);
				if(addressenExtensionEntity == null)
				{
					Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.Insert(_addressenExtensionEntity, botransaction.connection, botransaction.transaction);
				}
				else
				{
					_addressenExtensionEntity.Id = addressenExtensionEntity.Id;
					Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.Update(_addressenExtensionEntity, botransaction.connection, botransaction.transaction);
				}

				var supplierEntity = new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity()
				{
					Nr = lieferantenEntity.Nr,
					Nummer = lieferantenEntity.Nummer,
					// ---
					Belegkreis = _data.SlipCircleId,
					Bestellbestatigung_anmahnen = _data.RemindOrderConfirmation,
					Bestellimit = _data.OrderLimit.HasValue ? (double)_data.OrderLimit.Value : (double?)null,
					Branche = _data.Industry,
					EG_Identifikationsnummer = _data.EgIdentificationNumber,
					Eilzuschlag = _data.ShippingExpressSurcharge.HasValue ? (double)_data.ShippingExpressSurcharge.Value : (double?)null,
					Frachtfreigrenze = _data.ShippingFreightAllowance.HasValue ? (double)_data.ShippingFreightAllowance.Value : (double?)null,
					Gesperrt_fur_weitere_Bestellungen = _data.BlockedForFurtherOrders,
					Grund_fur_Sperre = _data.ReasonForBlocking,
					Karenztage = _data.WaitingPeriod,
					Konditionszuordnungs_Nr = _data.ConditionAssignmentNumber,
					Kreditoren_Nr = string.Empty,
					Kundennummer_Lieferanten = string.Empty,
					Kundennummer_PSZ_AL_Lieferanten = string.Empty,
					Kundennummer_PSZ_CZ_Lieferanten = string.Empty,
					Kundennummer_PSZ_TN_Lieferanten = string.Empty,
					Kundennummer_SC_CZ_Lieferanten = string.Empty,
					Kundennummer_SC_Lieferanten = string.Empty,
					LH = null,
					LH_Datum = null,
					Lieferantengruppe = _data.SuppliersGroup,
					Mahnsperre = _data.Dunning,
					Mahnsperre_Lieferant = _data.DunningBlockSupplier,
					Mindestbestellwert = _data.MinimumValue.HasValue ? (double)_data.MinimumValue.Value : (double?)null,
					Rabattgruppe = _data.DiscountGroupId,
					Sprache = _data.LanguageId,
					Umsatzsteuer_berechnen = _data.CalculateSalesTax,
					Versandart = _data.ShippingMethod,
					Versandkosten = _data.ShippingCosts.HasValue ? (double)_data.ShippingCosts.Value : (double?)null,
					Wahrung = _data.CurrencyId,
					Wochentag_Anlieferung = _data.ShippingWeekDay,
					Zahlungsweise = _data.PaymentMethod,
					Zielaufschlag = _data.TargetImpact.HasValue ? (Single?)_data.TargetImpact.Value : (Single?)null,
					Zuschlag_Mindestbestellwert = _data.SurchargeMinimumOrderValue.HasValue ? (double)_data.SurchargeMinimumOrderValue.Value : (double?)null,
				};

				var insertedSupplierId = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Update(supplierEntity, botransaction.connection, botransaction.transaction);

				if(_data.ContactPersons != null && _data.ContactPersons.Count > 0)
				{
					var deleteContactPerson = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.DeleteBySupplierAddress((int)lieferantenEntity.Nummer, botransaction.connection, botransaction.transaction);
					foreach(var contactPerson in _data.ContactPersons)
					{
						Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.InsertWithTransaction(contactPerson.ToDataEntity(lieferantenEntity.Nummer), botransaction.connection, botransaction.transaction);
					}
				}
				// logging
				var log = ObjectLogHelper.getLog(this._user, adressenEntity.Nr, "Address", null, adressenEntity.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Edit);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedSupplierId);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
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

			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id);

			var errors = new List<ResponseModel<int>.ResponseError>();

			if(lieferantenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Supplier not found"}
					}
				};
			}

			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)lieferantenEntity.Nummer);
			if(adressenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Supplier address not found"}
					}
				};
			}

			if(string.IsNullOrEmpty(_data.AdressName1))
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 is empty" });
			}
			else
			{
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(_data.AdressName1.Trim(), _data.AddressType);
				if(addressEntity == null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 with the same Address Type not found" });
				}
				else
				{
					if(addressEntity.Nr != lieferantenEntity.Nummer)
					{
						errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 with the same Address Type is already used" });
					}
				}
			}

			if(_data.AddressEDIActive.HasValue && Convert.ToBoolean(_data.AddressEDIActive))
			{
				if(string.IsNullOrEmpty(_data.AdressDUNS) || string.IsNullOrWhiteSpace(_data.AdressDUNS))
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = $"Invalid value for Address DUNS [{_data.AdressDUNS}]" });
				}
				//else
				//{
				//    if(duns != adressenEntity.Duns)
				//    {
				//        errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Address DUNS do not match existing" });
				//    }
				//}
			}

			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
