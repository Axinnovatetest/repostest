using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Supplier.Adress
{
	public class UpdateSupplierAdressHandler: IHandle<Models.Supplier.SupplierAdressModel, ResponseModel<int>>
	{
		private Models.Supplier.SupplierAdressModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateSupplierAdressHandler(Identity.Models.UserModel user, Models.Supplier.SupplierAdressModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CostumerEditLock.GetOrAdd(this._data.AddressId, new object()))
			{
				try
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var adressEntity = this._data.ToEntity();
					var logs = LogChanges();
					// save update logs
					if(logs.Count > 0)
					{
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
					}
					var response = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateAdress(adressEntity);
					return ResponseModel<int>.SuccessResponse(1);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressId);

			if(adressenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Adress not found"}
					}
				};
			}
			if(string.IsNullOrEmpty(this._data.AdressStreet) || string.IsNullOrWhiteSpace(this._data.AdressStreet) ||
				string.IsNullOrEmpty(this._data.AdressCountry) || string.IsNullOrWhiteSpace(this._data.AdressCountry) ||
				 string.IsNullOrEmpty(this._data.AdressStreetZipCode) || string.IsNullOrWhiteSpace(this._data.AdressStreetZipCode) ||
				 string.IsNullOrEmpty(this._data.AdressCity) || string.IsNullOrWhiteSpace(this._data.AdressCity) ||
				 this._data.AddressType == null || !this._data.Number.HasValue || (this._data.Number.HasValue && this._data.Number.Value == 0) ||
				 string.IsNullOrEmpty(this._data.AdressName1) || string.IsNullOrWhiteSpace(this._data.AdressName1) ||
				 string.IsNullOrEmpty(this._data.PreName) || string.IsNullOrWhiteSpace(this._data.PreName)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Please fill all the required feilds"}
					}
				};
			}
			var allAdressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get();
			allAdressEntity = allAdressEntity.Except(allAdressEntity.Where(x => x.Lieferantennummer == adressenEntity.Lieferantennummer)).ToList();
			var _remaining_adress = allAdressEntity.Where(x => x.Lieferantennummer == this._data.AdressSupplierNumber).ToList();
			if(_remaining_adress != null && _remaining_adress.Count > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Lieferantennummer [{this._data.AdressSupplierNumber}] already exsists." }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)this._data.AddressId);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;
			//
			if(adressenEntity.Abteilung != this._data.AdressDepartment)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Abteilung", adressenEntity.Abteilung, this._data.AdressDepartment, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Adresstyp != this._data.AddressType)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Adresstyp", adressenEntity.Adresstyp.ToString(), this._data.AddressType.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Anrede != this._data.PreName)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Anrede", adressenEntity.Anrede, this._data.PreName, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Auswahl != this._data.AdressSelection)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Auswahl", adressenEntity.Auswahl.ToString(), this._data.AdressSelection.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Bemerkung != this._data.AdressNote)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Bemerkung", adressenEntity.Bemerkung, this._data.AdressNote, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Bemerkungen != this._data.AdressNotes)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Bemerkungen", adressenEntity.Bemerkungen, this._data.AdressNotes, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Briefanrede != this._data.AdressSalutation)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Briefanrede", adressenEntity.Briefanrede, this._data.AdressSalutation, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.EMail != this._data.AdressEmailAdress)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "EMail", adressenEntity.EMail, this._data.AdressEmailAdress, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Fax != this._data.AdressFaxNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Fax", adressenEntity.Fax, this._data.AdressFaxNumber, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Funktion != this._data.AdressFunction)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Funktion", adressenEntity.Funktion, this._data.AdressFunction, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Land != this._data.AdressCountry)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Land", adressenEntity.Land, this._data.AdressCountry, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Name1 != this._data.AdressName1)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Name1", adressenEntity.Name1, this._data.AdressName1, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Name2 != this._data.AdressName2)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Name2", adressenEntity.Name2, this._data.AdressName2, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Name3 != this._data.AdressName3)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Name3", adressenEntity.Name3, this._data.AdressName3, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Ort != this._data.AdressCity)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Ort", adressenEntity.Ort, this._data.AdressCity, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.PLZ_Postfach != this._data.AdressMailboxZipCode)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "PLZ_Postfach", adressenEntity.PLZ_Postfach, this._data.AdressMailboxZipCode, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.PLZ_StraBe != this._data.AdressStreetZipCode)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "PLZ_StraBe", adressenEntity.PLZ_StraBe, this._data.AdressStreetZipCode, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Postfach != this._data.AdressMailbox)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Postfach", adressenEntity.Postfach, this._data.AdressMailbox, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Postfach != this._data.AdressMailbox)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Postfach", adressenEntity.Postfach, this._data.AdressMailbox, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Postfach_bevorzugt != this._data.AdressMailboxIsPreferred)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Postfach_bevorzugt", adressenEntity.Postfach_bevorzugt.ToString(), this._data.AdressMailboxIsPreferred.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Sortierbegriff != this._data.AdressSortTerm)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Sortierbegriff", adressenEntity.Sortierbegriff, this._data.AdressSortTerm, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.StraBe != this._data.AdressStreet)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "StraBe", adressenEntity.StraBe, this._data.AdressStreet, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Stufe != this._data.AdressLevel)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Stufe", adressenEntity.Stufe, this._data.AdressLevel, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Telefon != this._data.AdressPhoneNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Telefon", adressenEntity.Telefon, this._data.AdressPhoneNumber, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Titel != this._data.AdressTitle)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Titel", adressenEntity.Titel, this._data.AdressTitle, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Von != this._data.AdressFrom)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Von", adressenEntity.Von, this._data.AdressFrom, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Vorname != this._data.AdressFirstName)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Vorname", adressenEntity.Vorname, this._data.AdressFirstName, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.WWW != this._data.AdressWebsite)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "WWW", adressenEntity.WWW, this._data.AdressWebsite, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Duns != this._data.AdressDUNS)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Duns", adressenEntity.Duns, this._data.AdressDUNS, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.EDI_Aktiv != this._data.AddressEDIActive)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "EDI_Aktiv", adressenEntity.EDI_Aktiv.ToString(), this._data.AddressEDIActive.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}
			if(adressenEntity.Erfasst != this._data.AdressRecordTime)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Erfasst", adressenEntity.Erfasst.ToString(), this._data.AdressRecordTime.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), logTypeEdit));
			}
			if(adressenEntity.Lieferantennummer != this._data.AdressSupplierNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Lieferantennummer", adressenEntity.Lieferantennummer.ToString(), this._data.AdressSupplierNumber.ToString(), Enums.ObjectLogEnums.Objects.Supplier_SupplierNumber.GetDescription(), logTypeEdit));
			}
			//
			return logs;
		}
	}
}
