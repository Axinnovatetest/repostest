using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public partial class GetHallsHandler: IHandle<string, ResponseModel<List<Models.Article.HallResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHallsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Article.HallResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var results = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				// -
				return ResponseModel<List<Models.Article.HallResponseModel>>.SuccessResponse(
					results?.Select(x =>
						new Models.Article.HallResponseModel(x))
						?.Distinct()?.ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.HallResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.HallResponseModel>>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<List<Models.Article.HallResponseModel>>.SuccessResponse();
		}
	}
}
