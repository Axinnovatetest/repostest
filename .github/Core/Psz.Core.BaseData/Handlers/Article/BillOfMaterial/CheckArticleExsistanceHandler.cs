using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class CheckArticleExsistanceHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public CheckArticleExsistanceHandler(UserModel user, string artnummer)
		{
			_user = user;
			_data = artnummer;
		}
		public ResponseModel<bool> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				bool _exsist = false;
				var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
				if(parentArticle != null)
				{
					_exsist = true;
				}
				return ResponseModel<bool>.SuccessResponse(_exsist);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<bool> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}
			if(this._data == null)
			{
				return ResponseModel<bool>.FailureResponse(key: "1", value: "Article number must not be empty");
			}
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
