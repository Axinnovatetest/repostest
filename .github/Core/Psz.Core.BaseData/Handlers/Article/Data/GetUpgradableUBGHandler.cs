using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetUpgradableUBGHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetUpgradableUBGHandler(UserModel user, int data)
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

			return ResponseModel<List<Models.Article.Data.UpgradableHUBGItem>>.SuccessResponse();
		}
		public static List<Models.Article.Data.UpgradableHUBGItem> GetData(int articleId)
		{
			// - ubg articles in current Article's BOM
			var bomPosEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(articleId);
			if(bomPosEntities == null || bomPosEntities.Count <= 0)
			{
				return null;
			}

			var ubgEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomPosEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList())
				?.Where(x => x.UBG == true && x.Warengruppe?.ToLower()?.Trim() == "ef")?.ToList();
			if(ubgEntities == null || ubgEntities.Count <= 0)
			{
				return null;
			}

			var results = new List<Models.Article.Data.UpgradableHUBGItem>();
			foreach(var ubgEntity in ubgEntities)
			{
				var newerCustomerVersions = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(ubgEntity.CustomerNumber ?? -1, ubgEntity.ArtikelNummer?.Substring(0, ubgEntity.ArtikelNummer?.IndexOf('-') ?? 0), ubgEntity.CustomerItemNumber)
					?.Where(x => x.CustomerIndexSequence > ubgEntity.CustomerIndexSequence)?.ToList();
				if(newerCustomerVersions != null && newerCustomerVersions.Count > 0)
				{
					var maxIndexSeq = newerCustomerVersions.Max(x => x.CustomerIndexSequence);
					var lastIndex = newerCustomerVersions.Where(x => x.CustomerIndexSequence == maxIndexSeq)?.ToList();
					if(lastIndex != null)
					{
						if(lastIndex.Count == 1)
						{
							results.Add(new Models.Article.Data.UpgradableHUBGItem(ubgEntity, lastIndex[0]));
						}
						else
						{
							if(lastIndex.Count > 1)
							{
								var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleId);
								// - chose New Version in same ProdPlace
								var sameProdCountryArticle = lastIndex.Where(x => x.ProductionCountryCode == article.ProductionCountryCode)?.ToList();
								if(sameProdCountryArticle == null || sameProdCountryArticle.Count <= 0)
								{
									results.Add(new Models.Article.Data.UpgradableHUBGItem(ubgEntity, lastIndex[0]));
								}
								else
								{
									var sameProdSite = sameProdCountryArticle.Where(x => x.ProductionSiteCode == article.ProductionSiteCode)?.ToList();
									if(sameProdSite == null || sameProdSite.Count <= 0)
									{
										results.Add(new Models.Article.Data.UpgradableHUBGItem(ubgEntity, sameProdCountryArticle[0]));
									}
									else
									{
										results.Add(new Models.Article.Data.UpgradableHUBGItem(ubgEntity, sameProdSite[0]));
									}
								}
							}
						}
					}
				}
			}
			// - 
			return results;
		}
	}
}
