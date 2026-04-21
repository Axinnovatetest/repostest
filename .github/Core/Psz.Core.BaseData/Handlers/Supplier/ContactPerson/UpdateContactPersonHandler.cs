using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.ContactPerson
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
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.KundenId);
				var contactPersonEntity = this._data.ToDataEntity(lieferantenEntity.Nummer);

				var response = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Update(contactPersonEntity);
				return ResponseModel<int>.SuccessResponse(response);
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
			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.KundenId);
			if(lieferantenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Supplier not found"}
					}
				};
			}
			if(string.IsNullOrEmpty(this._data.PhoneNumber) || string.IsNullOrEmpty(this._data.ContactPerson) || string.IsNullOrEmpty(this._data.EmailAdress) || string.IsNullOrEmpty(this._data.Adress))
			{
				return ResponseModel<int>.FailureResponse("Please fill all the required fields");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
