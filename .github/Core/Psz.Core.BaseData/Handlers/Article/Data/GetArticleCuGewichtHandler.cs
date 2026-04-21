using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class GetArticleCuGewichtHandler: IHandle<Identity.Models.UserModel, ResponseModel<decimal>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetArticleCuGewichtHandler(Identity.Models.UserModel user, int ArticleNr)
		{
			this._user = user;
			this._data = ArticleNr;
		}
		public ResponseModel<decimal> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<decimal>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCuGewicht(this._data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<decimal> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<decimal>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
			{
				return ResponseModel<decimal>.FailureResponse("Article not found");
			}

			return ResponseModel<decimal>.SuccessResponse();
		}
	}
}
