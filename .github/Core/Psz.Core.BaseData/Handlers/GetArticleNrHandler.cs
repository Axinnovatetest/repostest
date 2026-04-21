using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers
{
	public class GetArticleNrHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly string _data;

		public GetArticleNrHandler(Identity.Models.UserModel user,string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data);
				return ResponseModel<int>.SuccessResponse(article.ArtikelNr);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
	}
}