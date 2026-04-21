using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class EditHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private Models.Address.AddressModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Address.AddressModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			botransaction.beginTransaction();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				#region // -- transaction-based logic -- //
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.Id, botransaction.connection, botransaction.transaction);
				addressEntity.Name1 = this._data.Name1;
				addressEntity.Name2 = this._data.Name2;
				addressEntity.Name3 = this._data.Name3;
				addressEntity.Adresstyp = this._data.AddressType;
				addressEntity.StraBe = this._data.Street;
				addressEntity.Ort = this._data.City;
				addressEntity.Land = this._data.Country;
				addressEntity.PLZ_StraBe = this._data.StreetZipCode;
				addressEntity.Telefon = this._data.PhoneNumber;
				addressEntity.Fax = this._data.FaxNumber;
				addressEntity.EMail = this._data.EmailAdress;
				addressEntity.Anrede = this._data.PreName;
				addressEntity.Briefanrede = this._data.Salutation;
				addressEntity.WWW = this._data.Website;
				addressEntity.Bemerkungen = this._data.Notes;
				addressEntity.Bemerkung = this._data.Note;
				addressEntity.Abteilung = this._data.Department;
				addressEntity.PLZ_Postfach = this._data.MailboxZipCode;
				addressEntity.Postfach = this._data.Mailbox;
				// -
				if(this._data.AddressType != 3)
				{
					addressEntity.Kundennummer = this._data.CustomerNumber;
					addressEntity.Lieferantennummer = this._data.SupplierNumber;
				}

				var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateWithTransaction(addressEntity, botransaction.connection, botransaction.transaction);
				//logging
				var log = ObjectLogHelper.getLog(this._user, insertedAddressId, "Address", null, this._data.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Edit);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedAddressId);
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

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(
				string.IsNullOrEmpty(this._data.Name1) || string.IsNullOrWhiteSpace(this._data.Name1) ||
				string.IsNullOrEmpty(this._data.City) || string.IsNullOrWhiteSpace(this._data.City) ||
				string.IsNullOrEmpty(this._data.Street) || string.IsNullOrWhiteSpace(this._data.Street) ||
				string.IsNullOrEmpty(this._data.StreetZipCode) || string.IsNullOrWhiteSpace(this._data.StreetZipCode) ||
				string.IsNullOrEmpty(this._data.Country) || string.IsNullOrWhiteSpace(this._data.Country) ||
				this._data.AddressType < 0
				)
			{
				return ResponseModel<int>.FailureResponse("Please fill all required fields");
			}

			var kundenByNummer = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber ?? -1, this._data.AddressType);

			if(kundenByNummer == null)
			{
				return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber}] not found.");
			}

			if(kundenByNummer.Adresstyp == (int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard)
			{
				return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber}] can have only one standard address");
			}


			var addressTypes = Enum.GetValues(typeof(Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum)).Cast<Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum>().ToList();
			if(!addressTypes.Exists(x => (int)x == this._data.AddressType))
			{
				return ResponseModel<int>.FailureResponse($"AddressType [{this._data.AddressType}] invalid.");
			}

			var oaddressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.Id);
			if(oaddressEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Address not found");
			}

			if(oaddressEntity.Adresstyp != this._data.AddressType)
			{
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(this._data.Name1.Trim(), this._data.AddressType);
				if(addressEntity != null)
				{
					return ResponseModel<int>.FailureResponse($"Address [{this._data.Name1.Trim()}] with the same type already exists");
				}
			}


			if(this._data.SupplierNumber.HasValue == true && this._data.SupplierNumber.Value > 0 && this._data.SupplierNumber.Value != oaddressEntity.Lieferantennummer)
			{
				if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByLieferantennummer(this._data.SupplierNumber.Value, 1) == null)
				{
					return ResponseModel<int>.FailureResponse($"Lieferant [{this._data.SupplierNumber.Value}] not found.");
				}
				var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByLieferantennummer(this._data.SupplierNumber.Value, this._data.AddressType);
				if(Lieferantennummer_exsist != null)
				{
					return ResponseModel<int>.FailureResponse($"Lieferant [{this._data.SupplierNumber.Value}] already exists with another address [{Lieferantennummer_exsist.Name1}].");
				}
			}
			if(this._data.CustomerNumber.HasValue == true && this._data.CustomerNumber.Value > 0 && this._data.CustomerNumber.Value == oaddressEntity.Kundennummer)
			{

				var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber ?? -1, this._data.AddressType, this._data.Id);
				if(Lieferantennummer_exsist != null)
				{
					return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber.Value}] already exists with address [{Lieferantennummer_exsist.Name1}].");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
