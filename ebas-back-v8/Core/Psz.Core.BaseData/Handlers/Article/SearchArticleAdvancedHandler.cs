using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Infrastructure.Data.Access;
	public partial class ArticleService: IArticleService
	{
		public ResponseModel<Models.Article.ArticleSearchResponseModel> SearchArticleAdvanced(Identity.Models.UserModel user, Models.Article.ArticleSearchAdvancedModel data)
		{

			try
			{
				var validationResponse = this.ValidateSearchArticleAdvanced(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.ItemsPerPage > 0 ? (data.RequestedPage * data.ItemsPerPage) : 0,
					RequestRows = data.ItemsPerPage
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(data.SortFieldKey.ToLower())
					{
						default:
						case "artikelnr":
							sortFieldName = "[Artikel-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "[ArtikelNummer]";
							break;
						case "bezeichnung1":
							sortFieldName = "[Bezeichnung 1]";
							break;
						case "bezeichnung2":
							sortFieldName = "[Bezeichnung 2]";
							break;
						case "freigabestatus":
							sortFieldName = "[Freigabestatus]";
							break;
						case "index_kunde":
							sortFieldName = "[index_kunde]";
							break;
						case "index_kunde_datum":
							sortFieldName = "[index_kunde_datum]";
							break;
						case "artikelfamilie_kunde":
							sortFieldName = "[Artikelfamilie_Kunde]";
							break;
						case "customeritemnumber":
							sortFieldName = "[CustomerItemNumber]";
							break;
						case "edidefault":
							sortFieldName = "[EdiDefault]";
							break;
						case "aktiv":
							sortFieldName = "[Aktiv]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}

				#endregion

				var articles = new List<Models.Article.ArticleMinimalModel>();
				int allCount = 0;

				//List<string> articleNummers = null;
				//if(!string.IsNullOrWhiteSpace(this._data.CustomerNr) && !string.IsNullOrEmpty(this._data.CustomerNr))
				//{
				//	articleNummers = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetByKunden(this._data.CustomerNr, this._data.CustomerPrefix)?.Select(x => x.Artikelnummer).Distinct().ToList();
				//}
				string Condition = "";
				var includeExtension = false;


				for(int i = 0; i < data.ListeSearchArticleAdvanced.Count; i++)
				{

					switch(data.ListeSearchArticleAdvanced[i].typeColum)
					{
						case 1:
							Condition += $" and artikelnummer like '{data.ListeSearchArticleAdvanced[i].inputColum}%'";
							break;
						case 2:
							Condition += $" AND ([Bezeichnung 1] Like '%{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}%' OR  [Bezeichnung 2] Like '%{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}%' OR  [Bezeichnung 3] Like '%{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}%') ";
							break;
						case 3:
							List<string> articleNummers = null;
							if(!string.IsNullOrWhiteSpace(data.ListeSearchArticleAdvanced[i].inputColum) && !string.IsNullOrEmpty(data.ListeSearchArticleAdvanced[i].inputColum))
							{
								articleNummers = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetByKunden(data.ListeSearchArticleAdvanced[i].inputColum, "")?.Select(x => x.Artikelnummer).Distinct().ToList();
							}
							if(articleNummers != null)
							{
								Condition += $" AND [Artikelnummer] IN ('{string.Join("','", articleNummers.Select(x => x.SqlEscape()))}') ";
							}
							break;
						case 4:
							if(data.ListeSearchArticleAdvanced[i].inputColum != "" && data.ListeSearchArticleAdvanced[i].inputColum != null)
							{
								Condition += $" AND Aktiv = {(data.ListeSearchArticleAdvanced[i].inputColum.ToLower() == "true" ? 1 : 0)} ";
							}
							break;
						case 5:
							if(!string.IsNullOrEmpty(data.ListeSearchArticleAdvanced[i].inputColum) && !string.IsNullOrWhiteSpace(data.ListeSearchArticleAdvanced[i].inputColum))
							{
								Condition += $" AND [Warengruppe] = '{data.ListeSearchArticleAdvanced[i].inputColum}' ";
							}
							break;
						case 6:
							if(!string.IsNullOrWhiteSpace(data.ListeSearchArticleAdvanced[i].inputColum))
							{
								Condition += $" AND ([Artikelfamilie_Kunde] Like '%{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}%') ";
							}
							break;
						case 7:
							Condition += $" AND ([CustomerItemNumber] = '{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}' OR [ManufacturerNumber]='{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}')";
							break;
						case 8:
							if(!string.IsNullOrWhiteSpace(data.ListeSearchArticleAdvanced[i].inputColum))
							{
								includeExtension = true;
								Condition += $" AND (e.[OrderNumber] LIKE '{data.ListeSearchArticleAdvanced[i].inputColum.SqlEscape(true)}%') ";
							}
							break;
					}


				}
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SearchByNrNumberDesignationAdvanced(
					Condition, includeExtension,
					dataSorting,
					dataPaging);

				if(articleEntities != null && articleEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SearchByNrNumberDesignationAdvanced_CountAll(Condition, includeExtension);
					var articleExtEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNrs(articleEntities.Select(x => x.ArtikelNr).ToList());

					for(int i = 0; i < articleEntities.Count; i++)
					{
						var articleExtEntity = articleExtEntities.FirstOrDefault(x => x.ArtikelNr == articleEntities[i].ArtikelNr);
						articles.Add(new Models.Article.ArticleMinimalModel(articleEntities[i], articleExtEntity?.OrderNumber));
					}
				}

				return ResponseModel<Models.Article.ArticleSearchResponseModel>.SuccessResponse(
					new Models.Article.ArticleSearchResponseModel()
					{
						Articles = articles,
						RequestedPage = data.RequestedPage,
						ItemsPerPage = data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / data.ItemsPerPage)) : 0,
					});

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<Models.Article.ArticleSearchResponseModel> ValidateSearchArticleAdvanced(Identity.Models.UserModel user, Models.Article.ArticleSearchAdvancedModel data)
		{
			if(user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ArticleSearchResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.ArticleSearchResponseModel>.SuccessResponse();
		}
	}
}
