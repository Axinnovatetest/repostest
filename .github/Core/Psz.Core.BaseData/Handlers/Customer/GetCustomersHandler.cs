using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	///
	public class GetCustomersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Customer.CustomerModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }


		public GetCustomersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Customer.CustomerModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetKundenAddresses();// .GetWhereKundennummerIsNotNull();
				var kundenEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get();


				var response = new List<Models.Customer.CustomerModel>();

				foreach(var kundenEntity in kundenEntities)
				{
					var adressEntity = adressenEntities.Find(e => e.Nr == kundenEntity.Nummer);
					var contactPersonsEntities = adressEntity != null
						? Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(adressEntity.Nr)
						: new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();

					var kundenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(kundenEntity.Nr);

					response.Add(new Models.Customer.CustomerModel(kundenEntity, adressEntity, contactPersonsEntities, kundenExtensionEntity));
				}

				return ResponseModel<List<Models.Customer.CustomerModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Customer.CustomerModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Customer.CustomerModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Customer.CustomerModel>>.SuccessResponse();
		}
	}
}
