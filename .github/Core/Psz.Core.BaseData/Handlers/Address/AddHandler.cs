using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class AddHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private Models.Address.AddAdresseRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public AddHandler(Identity.Models.UserModel user, Models.Address.AddAdresseRequestModel data)
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

				var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
						{
							Name1 = this._data.Name1,
							Name2 = this._data.Name2,
							Name3 = this._data.Name3,
							Adresstyp = (int)this._data.Type,
							StraBe = this._data.Street,
							Ort = this._data.City,
							Land = this._data.Country,
							PLZ_StraBe = this._data.StreetZipCode,
							Telefon = this._data.PhoneNumber,
							Fax = this._data.FaxNumber,
							EMail = this._data.EmailAdress,
							Anrede = this._data.PreName,
							Briefanrede = this._data.Salutation,
							WWW = this._data.Website,
							Bemerkungen = this._data.Notes,
							Bemerkung = this._data.Note,
							Abteilung = this._data.Department,
							Postfach = this._data.Mailbox,
							Sperren = false,
							Erfasst = DateTime.Now
						}, botransaction.connection, botransaction.transaction);
				//logging
				//Kundennummer = (int)this._data.Type == 3 ? null : Convert.ToInt32(this._data.CustomerNumber),
				//			Lieferantennummer = (int)this._data.Type == 3 ? null : Convert.ToInt32(this._data.SupplierNumber),
				var log = ObjectLogHelper.getLog(this._user, insertedAddressId, "Address", null, this._data.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
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
				this._data.Type < 0
				)
			{
				return ResponseModel<int>.FailureResponse("Please fill all required fields");
			}

			var addressTypes = Enum.GetValues(typeof(Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum)).Cast<Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum>().ToList();
			if(!addressTypes.Exists(x => (int)x == (int)this._data.Type))
			{
				return ResponseModel<int>.FailureResponse($"AddressType [{this._data.Type}] invalid.");
			}

			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(this._data.Name1.Trim(), (int)this._data.Type);
			if(addressEntity != null)
			{
				return ResponseModel<int>.FailureResponse($"Address [{this._data.Name1.Trim()}] with the same type and name is already used");
			}

			if(!String.IsNullOrEmpty(this._data.SupplierNumber))
			{
				if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByLieferantennummer(Convert.ToInt32(this._data.SupplierNumber), 1) == null)
				{
					return ResponseModel<int>.FailureResponse($"Lieferant [{this._data.SupplierNumber}] not found.");
				}
				var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByLieferantennummer(Convert.ToInt32(this._data.SupplierNumber), (int)this._data.Type);
				if(Lieferantennummer_exsist != null)
				{
					return ResponseModel<int>.FailureResponse($"Lieferant [{this._data.SupplierNumber}] already exists with address [{Lieferantennummer_exsist.Name1}].");
				}
			}
			if(!String.IsNullOrEmpty(this._data.CustomerNumber))
			{
				if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(Convert.ToInt32(this._data.CustomerNumber)) == null)
				{
					return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber}] not found.");
				}
				var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(Convert.ToInt32(this._data.CustomerNumber), (int)this._data.Type);
				if(Lieferantennummer_exsist != null)
				{
					return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber}] already exists with address [{Lieferantennummer_exsist.Name1}].");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
