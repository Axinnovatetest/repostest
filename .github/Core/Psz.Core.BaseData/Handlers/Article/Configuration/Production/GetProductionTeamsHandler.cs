using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProductionTeamsHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Production.ArticleTeamsResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetProductionTeamsHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Production.ArticleTeamsResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				////for(int i = 0; i < 5; i++)
				////{
				////	Infrastructure.Data.Access.Tables.BSD.TeamsAccess.Insert(
				////		new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity
				////		{
				////			Description = "globales Team",
				////			Id = i,
				////			Name = $"TW{i + 1}",
				////			SiteId = 0,
				////			SitePrefix = "",
				////			TeamCategory = ' ',
				////			TeamIndex = 0
				////		});
				////}

				////var sites = new List<string> { "AL", "CZ", "TN", "BETN", "WS", "GZ" };
				////var siteIds = new List<int> { 26, 6, 7, 60, 42, 102 };
				////var d = new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
				////for(int k = 0; k < sites.Count; k++)
				////{
				////	for(int i = 5; i < 20; i++)
				////	{
				////		for(char j = 'A'; j < 'E'; j++)
				////		{

				////			d.Add(new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity
				////			{
				////				Description = "",
				////				Id = i,
				////				Name = $"{sites[k]}-TW{i + 1}-{j}".ToUpper(),
				////				SiteId = siteIds[k],
				////				SitePrefix = sites[k],
				////				TeamCategory = j,
				////				TeamIndex = i + 1
				////			});
				////		}
				////	}
				////}
				////Infrastructure.Data.Access.Tables.BSD.TeamsAccess.Insert(d);
				// -
				return ResponseModel<List<Models.Article.Production.ArticleTeamsResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.TeamsAccess.Get()?
						.Select(x => new Models.Article.Production.ArticleTeamsResponseModel(x)).Distinct().ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Production.ArticleTeamsResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Production.ArticleTeamsResponseModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Production.ArticleTeamsResponseModel>>.SuccessResponse();
		}
	}
}
