using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetInitValuesHandler: IHandle<UserModel, ResponseModel<Models.Article.Purchase.InitValuesResponseModel>>
	{
		private UserModel _user { get; set; }
		public GetInitValuesHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<Models.Article.Purchase.InitValuesResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var purchaseEntity = (Infrastructure.Data.Access.Tables.BSD.Stammdaten_FirmaAccess.Get() ??
					new List<Infrastructure.Data.Entities.Tables.BSD.Stammdaten_FirmaEntity>()).FirstOrDefault();

				// -
				return ResponseModel<Models.Article.Purchase.InitValuesResponseModel>.SuccessResponse(
					new Models.Article.Purchase.InitValuesResponseModel
					{
						VAT = purchaseEntity?.Standard_USt ?? 0
					});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Purchase.InitValuesResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Purchase.InitValuesResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Purchase.InitValuesResponseModel>.SuccessResponse();
		}

	}

}
