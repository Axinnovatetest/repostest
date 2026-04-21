using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetArticlesUniqueHandler: IHandle<string, ResponseModel<List<string>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetArticlesUniqueHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<string>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<string>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetUniqueNumbers());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<string>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<string>>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<List<string>>.SuccessResponse();
		}
	}
}
