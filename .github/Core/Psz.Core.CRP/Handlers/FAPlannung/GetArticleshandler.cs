using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<ArticlesResponseModel> ValidateGetArticles(UserModel user)
		{
			if(user == null)
				return ResponseModel<ArticlesResponseModel>.AccessDeniedResponse();
			return ResponseModel<ArticlesResponseModel>.SuccessResponse();
		}
		public ResponseModel<ArticlesResponseModel> GetArticles(UserModel user, ArticlesRequestModel data)
		{
			try
			{
				var validationResponse = ValidateGetArticles(user);
				if(!validationResponse.Success)
					return validationResponse;
				if((data.Kundennumer is null || data.Kundennumer <= 0) && string.IsNullOrWhiteSpace(data.Kundenkreis))
					return ResponseModel<ArticlesResponseModel>.SuccessResponse(new ArticlesResponseModel { Items = null, PageRequested = data.RequestedPage, PageSize = data.PageSize, TotalCount = 0, TotalPageCount = 0 });


				var response = new ArticlesResponseModel();
				var articlesList = new List<ArticlesModel>();

				// -
				string expansion = "";
				if(!string.IsNullOrWhiteSpace(data.Kundenkreis) && data.Kundenkreis.Length > 2)
				{
					expansion = $"[Artikelnummer] LIKE '{data.Kundenkreis}%'";
				}
				else
				{
					if(data.Kundennumer > 0)
					{
						var artilcesNummerkreis = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerNumber(data.Kundennumer ?? 0);
						if(artilcesNummerkreis != null && artilcesNummerkreis.Count > 0)
						{
							expansion = "(";
							var nummerKreis = artilcesNummerkreis.Select(x => x.Nummerschlüssel).ToList();
							foreach(var item in nummerKreis)
							{
								if(nummerKreis.IndexOf(item) == 0)
									expansion += $"[Artikelnummer] LIKE '{item}-%'";
								else
									expansion += $"OR [Artikelnummer] LIKE '{item}-%'";
							}
							expansion += ")";
						}
						else
						{
							// - WRONG FILTER to return no article
							expansion = "[Artikelnummer] LIKE 'ZZZZZ'";
						}
					}
				}
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				var dataSorting = new Infrastructure.Data.Access.Settings.SortingModel();
				switch(data.SortField.Trim().ToLower())
				{
					default:
					case "artikelnummer":
						dataSorting.SortFieldName = "[Artikelnummer]";
						break;
					case "deficit":
						dataSorting.SortFieldName = "[Prio]";
						break;
				}
				dataSorting.SortDesc = data.SortDesc;
				#endregion
				var articles =
					Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticlesByNummerkreis_delayed(expansion, data.Artikelnummer, data.DeficitType, data.Unit, false, dataSorting, dataPaging);

				var articlesCount =
					Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticlesByNummerkreisCount_delayed(expansion, data.Artikelnummer, data.DeficitType, data.Unit, false);
				if(articles != null && articles.Count > 0)
				{
					articlesList = articles.Select(x => new ArticlesModel
					{
						ArticleNr = x.ArtikelNr ?? -1,
						Artikelnummer = x.Artikelnummer,
						Deficit = x.Prio.HasValue && x.Prio.Value >= -1,
						DeficitType = x.Prio ?? -2,
						UBG = x.UBG
					}).ToList();
				}
				response = new ArticlesResponseModel
				{
					Items = articlesList,
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = articlesCount > 0 ? articlesCount : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(articlesCount > 0 ? articlesCount : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<ArticlesResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}