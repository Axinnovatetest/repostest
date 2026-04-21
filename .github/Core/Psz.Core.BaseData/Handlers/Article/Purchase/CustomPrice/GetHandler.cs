using System;

namespace Psz.Core.BaseData.Handlers.Article.Purchase.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Purchase.CustomPriceModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Purchase.CustomPriceModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Purchase.CustomPriceModel>.SuccessResponse(
						new Models.Article.Purchase.CustomPriceModel(
							Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Purchase.CustomPriceModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Purchase.CustomPriceModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Article.Purchase.CustomPriceModel>.FailureResponse("Custom price not found !");
			}

			return ResponseModel<Models.Article.Purchase.CustomPriceModel>.SuccessResponse();
		}
	}
}
