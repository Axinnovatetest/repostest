using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetUpgradableHBGHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetUpgradableHBGHandler(UserModel user, int data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 
				return ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>.SuccessResponse(GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
				return ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>.FailureResponse("Article not found");
			//if (articleEntity.UBG != true)
			//    return ResponseModel<List<Models.Article.Data.UpgradableHUBGModel>>.FailureResponse("Article is not UBG");

			return ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>.SuccessResponse();
		}
		public static List<Models.Article.Data.UpgradableHUBGItem> GetData(int articleId)
		{
			// - BOM where Article is used
			var bomPosEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticleUBG(articleId);
			if(bomPosEntities == null || bomPosEntities.Count <= 0)
			{
				return null;
			}

			var hbgEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomPosEntities.Select(x => x.Artikel_Nr ?? -1)?.ToList());
			if(hbgEntities == null || hbgEntities.Count <= 0)
			{
				return null;
			}

			var results = new List<Models.Article.Data.UpgradableHUBGItem>();
			foreach(var hbgEntity in hbgEntities)
			{
				results.Add(new Models.Article.Data.UpgradableHUBGItem(hbgEntity, null));
			}
			// - 
			return results;
		}
	}
}
