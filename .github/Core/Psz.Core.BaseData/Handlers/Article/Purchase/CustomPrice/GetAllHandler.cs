using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Purchase.CustomPriceModel>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetAllHandler(UserModel user, int nummer)
		{
			this._user = user;
			this._data = nummer;
		}
		public ResponseModel<List<Models.Article.Purchase.CustomPriceModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var customPriceEntites = Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.GetByOrderNumber(this._data)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.Bestellnummern_StaffelpreiseEntity>();

				return ResponseModel<List<Models.Article.Purchase.CustomPriceModel>>.SuccessResponse(customPriceEntites.Select(x => new Models.Article.Purchase.CustomPriceModel(x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Purchase.CustomPriceModel>> Validate()
		{
			if(this._user == null/*
	            || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Purchase.CustomPriceModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Purchase.CustomPriceModel>>.SuccessResponse();
		}
	}
}
