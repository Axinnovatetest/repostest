using System;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetArticleMaterialCostDBwoCUHandler: IHandle<UserModel, ResponseModel<decimal>>
	{
		private UserModel _user
		{
			get;
			set;
		}
		public int _data
		{
			get;
			set;
		}
		public GetArticleMaterialCostDBwoCUHandler(UserModel user, int articleNr)
		{
			this._user = user;
			this._data = articleNr;
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

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				return ResponseModel<decimal>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetMaterialCostDBwoCU(articleEntity.ArtikelNummer));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<decimal> Validate()
		{
			if(this._user == null /* || this._user.Access.____*/)
			{
				return ResponseModel<decimal>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<decimal>.FailureResponse("Article not found.");

			return ResponseModel<decimal>.SuccessResponse();
		}
	}
}