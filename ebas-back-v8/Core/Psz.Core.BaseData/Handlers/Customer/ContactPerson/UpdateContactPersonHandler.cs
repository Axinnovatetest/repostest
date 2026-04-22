using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Customer.ContactPerson
{
	public class UpdateContactPersonHandler: IHandle<Models.Customer.OverviewModel, ResponseModel<int>>
	{
		private Models.Supplier.ContactPersonModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateContactPersonHandler(Identity.Models.UserModel user, Models.Supplier.ContactPersonModel data)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();
				#region // -- transaction-based logic -- //
				// -

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetWithTransaction(this._data.KundenId, botransaction.connection, botransaction.transaction);
				var contactPersonEntity = this._data.ToDataEntity(kundenEntity.Nummer);

				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.UpdateCustomerTechnician(contactPersonEntity.Nr, contactPersonEntity.Ansprechpartner, botransaction.connection, botransaction.transaction);
				var response = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.UpdateWithTransaction(contactPersonEntity, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
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
			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.KundenId);
			if(kundenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Customer not found"}
					}
				};
			}
			if(string.IsNullOrEmpty(this._data.PhoneNumber) || string.IsNullOrWhiteSpace(this._data.PhoneNumber) || string.IsNullOrEmpty(this._data.ContactPerson) || string.IsNullOrWhiteSpace(this._data.ContactPerson) || string.IsNullOrEmpty(this._data.EmailAdress) || string.IsNullOrWhiteSpace(this._data.EmailAdress) || string.IsNullOrEmpty(this._data.Adress) || string.IsNullOrWhiteSpace(this._data.Adress))
			{
				return ResponseModel<int>.FailureResponse("Please fill all the required fields");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
