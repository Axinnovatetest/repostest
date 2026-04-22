using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetLagerStatusHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Logistics.LagerStatusModel>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetLagerStatusHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}
		public ResponseModel<List<Models.Article.Logistics.LagerStatusModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var bomItemAltEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { this._data }, true)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();

				return ResponseModel<List<Models.Article.Logistics.LagerStatusModel>>.SuccessResponse(bomItemAltEntities.Select(x => new Models.Article.Logistics.LagerStatusModel(x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Logistics.LagerStatusModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Logistics.LagerStatusModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.Logistics.LagerStatusModel>>.FailureResponse("Article not found");

			return ResponseModel<List<Models.Article.Logistics.LagerStatusModel>>.SuccessResponse();
		}

	}

}
