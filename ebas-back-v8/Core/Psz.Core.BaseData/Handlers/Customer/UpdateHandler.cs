using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateHandler: IHandle<Models.Customer.UpdateModel, ResponseModel<int>>
	{
		private Models.Customer.UpdateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateHandler(Models.Customer.UpdateModel data,
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
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

					var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetWithTransaction(this._data.Id, botransaction.connection, botransaction.transaction);
					var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)kundenEntity.Nummer, botransaction.connection, botransaction.transaction);

					var addressEntity = this._data.ToAddressenEntity(adressenEntity.Nr, adressenEntity.Kundennummer);
					var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateCustomer(addressEntity, botransaction.connection, botransaction.transaction);

					var _addressenExtensionEntity = _data.ToAddressenExtensionEntity(adressenEntity.Nr);
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

					var customerEntity = this._data.ToKundenEntity(kundenEntity.Nr, kundenEntity.Nummer);
					var insertedSupplierId = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Update(customerEntity, botransaction.connection, botransaction.transaction);

					if(_data.ContactPersons != null && _data.ContactPersons.Count > 0)
					{
						var deleteContactPerson = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.DeleteBySupplierAddress((int)kundenEntity.Nummer, botransaction.connection, botransaction.transaction);
						foreach(var contactPerson in _data.ContactPersons)
						{
							Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.InsertWithTransaction(contactPerson.ToDataEntity(kundenEntity.Nummer), botransaction.connection, botransaction.transaction);
						}
					}
					//logging
					var log = ObjectLogHelper.getLog(this._user, insertedAddressId, "Address", null, addressEntity.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Edit);
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

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Id);

			var errors = new List<ResponseModel<int>.ResponseError>();

			if(kundenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Customer not found"}
					}
				};
			}

			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)kundenEntity.Nummer);
			if(adressenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Customer address not found"}
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
					if(addressEntity.Nr != kundenEntity.Nummer)
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
