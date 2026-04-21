using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Supplier
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
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var lieferantenEntity = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(this._data.Id);
				var adressenEntity = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get((int)lieferantenEntity.Nummer);

				var addressEntity = new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity()
				{
					Nr = adressenEntity.Nr,
					Lieferantennummer = adressenEntity.Lieferantennummer,
					Adresstyp = (int)Infrastructure.Data.Access.Tables.FNC.AdressenAccess.AddressType.Supplier, // - adressenEntity.Adresstyp,
																												// --- 
					Anrede = _data.AdressPreName,
					Auswahl = _data.AdressSelection,
					//Bemerkung = _data.AdressNote,
					Bemerkungen = _data.AdressNotes,
					Briefanrede = _data.AdressSalutation,
					//Dienstag_Anliefertag = _data.ShippingTuesdayIsDeliveryDay,
					//Donnerstag_Anliefertag = _data.ShippingThursdayIsDeliveryDay,
					//Duns = _data.AdressDUNS,
					eMail = _data.AdressEmailAdress,
					erfasst = _data.AdressRecordTime,
					Fax = _data.AdressFaxNumber,
					//Freitag_Anliefertag = _data.ShippingFridayIsDeliveryDay,
					Funktion = _data.AdressFunction,
					Land = _data.AdressCountry,
					//Mittwoch_Anliefertag = _data.ShippingWednesdayIsDeliveryDay,
					//Montag_Anliefertag = _data.ShippingMondayIsDeliveryDay,
					Name1 = _data.AdressName1,
					Name2 = _data.AdressName2,
					Name3 = _data.AdressName3,
					Ort = _data.AdressCity,
					PLZ_Postfach = _data.AdressMailboxZipCode,
					PLZ_StraBe = _data.AdressStreetZipCode,
					Postfach = _data.AdressMailbox,
					Postfach_bevorzugt = _data.AdressMailboxIsPreferred,
					Sortierbegriff = _data.AdressSortTerm,
					StraBe = _data.AdressStreet,
					stufe = _data.AdressLevel,
					Telefon = _data.AdressPhoneNumber,
					Titel = _data.AdressTitle,
					von = _data.AdressFrom,
					Vorname = _data.AdressFirstName,
					WWW = _data.AdressWebsite,
					//EDI_Aktiv = _data.AddressEDIActive,

					sperren = false,
					Abteilung = string.Empty,
					//Kundennummer = null,
					//Personalnummer = null,
				};
				var insertedAddressId = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Update(addressEntity);

				var supplierEntity = new Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity()
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
					LH = false,
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
					Zielaufschlag = _data.TargetImpact.HasValue ? (double)_data.TargetImpact.Value : (double?)null,
					Zuschlag_Mindestbestellwert = _data.SurchargeMinimumOrderValue.HasValue ? (double)_data.SurchargeMinimumOrderValue.Value : (double?)null,
				};

				var insertedSupplierId = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Update(supplierEntity);

				// - 
				CreateHandler.addDiverseArticle(adressenEntity.Lieferantennummer ?? -1);

				if(_data.ContactPersons != null && _data.ContactPersons.Count > 0)
				{
					var deleteContactPerson = Infrastructure.Data.Access.Tables.FNC.AnsprechpartnerAccess.DeleteBySupplierAddress((int)lieferantenEntity.Nummer);
					foreach(var contactPerson in _data.ContactPersons)
					{
						Infrastructure.Data.Access.Tables.FNC.AnsprechpartnerAccess.Insert(contactPerson.ToDataEntity(lieferantenEntity.Nummer));
					}
				}

				return ResponseModel<int>.SuccessResponse(insertedSupplierId);
			} catch(Exception e)
			{
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

			var lieferantenEntity = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(this._data.Id);

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

			var adressenEntity = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get((int)lieferantenEntity.Nummer);
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
				var addressEntity = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.GetByName1(_data.AdressName1.Trim(), _data.AddressType);
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
				if(!_data.AdressDUNS.HasValue || !int.TryParse(_data.AdressDUNS.ToString(), out int duns))
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Invalid value for Address DUNS" });
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
