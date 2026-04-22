using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.CustomersGroup
{
	public class GetCustomerGroupsForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CustomersGroup.CustomerGroupModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomerGroupsForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.CustomersGroup.CustomerGroupModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var pszKundengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get();

				var response = new List<Models.CustomersGroup.CustomerGroupModel>();

				foreach(var pszKundengruppenEntity in pszKundengruppenEntities)
				{
					response.Add(new Models.CustomersGroup.CustomerGroupModel(pszKundengruppenEntity));
				}

				return ResponseModel<List<Models.CustomersGroup.CustomerGroupModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.CustomersGroup.CustomerGroupModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.CustomersGroup.CustomerGroupModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.CustomersGroup.CustomerGroupModel>>.SuccessResponse();
		}
	}
}
