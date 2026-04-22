using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer.ContactPerson
{
	public class GetCustomerContactPersonHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Supplier.ContactPersonModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetCustomerContactPersonHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Supplier.ContactPersonModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
				var contactPersonEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(kundenEntity.Nummer ?? -1);

				////Salutation
				//var salutationEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get();
				////Address
				//var AddressEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get();


				List<Models.Supplier.ContactPersonModel> response = new List<Models.Supplier.ContactPersonModel>();
				if(contactPersonEntity != null)
				{
					foreach(var item in contactPersonEntity)
					{
						response.Add(new Models.Supplier.ContactPersonModel(item));
					}
				}
				//Salutation
				//int actualSalutation;
				//string replacedSalutation;
				////Address
				//int actualAddress;
				//string replacedAddress;
				foreach(var item in response)
				{
					item.KundenId = kundenEntity.Nr;
					////Salutation
					//actualSalutation = int.TryParse(item.Salutation, out int i) ? i : 0;
					//replacedSalutation = salutationEntity.FirstOrDefault(x => x.ID == actualSalutation)?.Anrede;
					//item.Salutation = (item.Salutation != null && item.Salutation != "" && replacedSalutation != null) ? replacedSalutation : item.Salutation;
					////Address
					//actualAddress = int.TryParse(item.Adress, out int j) ? j : 0;
					//replacedAddress = AddressEntity.FirstOrDefault(x => x.ID == actualAddress)?.Anrede;
					//item.Adress = (item.Adress != null && item.Adress != "" && replacedAddress != null) ? replacedAddress : item.Adress;
				}
				return ResponseModel<List<Models.Supplier.ContactPersonModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Supplier.ContactPersonModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Supplier.ContactPersonModel>>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<List<Models.Supplier.ContactPersonModel>>()
				{
					Errors = new List<ResponseModel<List<Models.Supplier.ContactPersonModel>>.ResponseError>() {
						new ResponseModel<List<Models.Supplier.ContactPersonModel>>.ResponseError {Key = "1", Value = "Kunden not found"}
					}
				};
			}
			return ResponseModel<List<Models.Supplier.ContactPersonModel>>.SuccessResponse();
		}
	}
}
