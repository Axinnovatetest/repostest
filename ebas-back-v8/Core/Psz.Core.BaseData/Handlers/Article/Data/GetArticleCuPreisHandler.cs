using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class GetArticleCuPreisHandler: IHandle<Identity.Models.UserModel, ResponseModel<decimal>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetArticleCuPreisHandler(Identity.Models.UserModel user, int ArticleNr)
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

				var lastCuPrice = Infrastructure.Data.Access.Tables.BSD.TBL_KupferAccess.GetLastRecord();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				return ResponseModel<decimal>.SuccessResponse(((lastCuPrice?.Aktueller_Kupfer_Preis_in_Gramm * articleEntity.Kupferzahl) ?? 0) / 1000);
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
