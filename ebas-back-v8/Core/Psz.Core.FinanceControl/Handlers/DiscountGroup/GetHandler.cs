using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.DiscountGroup
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.DiscountGroup.DiscountGroupModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.DiscountGroup.DiscountGroupModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var rabatthauptgruppenEntities = Infrastructure.Data.Access.Tables.FNC.RabatthauptgruppenAccess.Get();

				var response = new List<Models.DiscountGroup.DiscountGroupModel>();

				foreach(var rabatthauptgruppenEntity in rabatthauptgruppenEntities)
				{
					response.Add(new Models.DiscountGroup.DiscountGroupModel(rabatthauptgruppenEntity));
				}

				return ResponseModel<List<Models.DiscountGroup.DiscountGroupModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.DiscountGroup.DiscountGroupModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.DiscountGroup.DiscountGroupModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.DiscountGroup.DiscountGroupModel>>.SuccessResponse();
		}
	}
}
