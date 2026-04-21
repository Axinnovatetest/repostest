using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetOrderTypesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CustomerService.OrderTypeModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetOrderTypesHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<Models.CustomerService.OrderTypeModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.CustomerService.OrderTypeModel>();

				var ordertypeEntities = Infrastructure.Data.Access.Tables.STG.OrderTypesAccess.Get();
				foreach(var ordertypeEntity in ordertypeEntities)
				{
					responseBody.Add(new Models.CustomerService.OrderTypeModel(ordertypeEntity));
				}

				return ResponseModel<List<Models.CustomerService.OrderTypeModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.CustomerService.OrderTypeModel>> Validate()
		{
			return ResponseModel<List<Models.CustomerService.OrderTypeModel>>.SuccessResponse();
		}
	}
}
