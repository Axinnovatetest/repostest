using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class ToggleBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleOverviewModel.Blanket _data { get; set; }
		public ToggleBlanketHandler(Identity.Models.UserModel user, Models.Article.ArticleOverviewModel.Blanket data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				var articleData = this._data.ToBlanketEntity(this._data.OrderId == 1, articleEntity, this._user, Enums.ObjectLogEnums.Objects.Article, articleEntity.ArtikelNr, logs, Enums.ObjectLogEnums.LogType.Edit);
				var updatedId = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditBlanketChecks(articleData, this._data.OrderId == 1);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);

				// -
				return ResponseModel<int>.SuccessResponse(updatedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null /*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// -
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
