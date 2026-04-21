using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetByArticleHandler: IHandle<UserModel, ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetByArticleHandler(UserModel user, int articleNr)
		{
			this._user = user;
			this._data = articleNr;
		}
		public ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var staffelPreiEntities = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArticleNr(this._data, false)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
				var preisGruppenEntites = artikelEntity == null ? null : Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr((int)artikelEntity?.ArtikelNr);

				var response = new List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>();
				//if (staffelPreiEntities!=null&&staffelPreiEntities.Count>0)
				//{
				//    foreach (var cp in staffelPreiEntities)
				//    {
				//        response.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(cp, preisGruppenEntites));
				//    }
				//}

				// - Add prices based on Pricing group data, not Custom Prices
				if(preisGruppenEntites != null)
				{
					var articleExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(artikelEntity?.ArtikelNr ?? -1);
					var hourlyRate = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.GetByProdutionSiteId(articleExtension?.ProductionPlace1_Id ?? -1);
					// - S1
					if(preisGruppenEntites.Staffelpreis1.HasValue && preisGruppenEntites.Staffelpreis1.Value != 0)
					{
						var customPrice = staffelPreiEntities.Find(x => (x.Staffelpreis_Typ ?? "").Trim().Replace(" ", "").ToLower() == Enums.ArticleEnums.CustomPriceType.S1.GetDescription().Trim().Replace(" ", "").ToLower());
						if(customPrice == null)
						{
							Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Insert(
								new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = this._data,
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S1.GetDescription().Replace(" ", ""),
									Type = Enums.ArticleEnums.CustomPriceType.S1.GetDescription(),
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S1,
									Stundensatz = hourlyRate?.Count > 0 ? hourlyRate[0].HourlyRate : 0,
									Betrag=preisGruppenEntites.kalk_kosten ?? 0
								});
							customPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArtikelNrAndType(this._data, Enums.ArticleEnums.CustomPriceType.S1.GetDescription());
						}
						response.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(customPrice, preisGruppenEntites));
					}

					// - S2
					if(preisGruppenEntites.Staffelpreis2.HasValue && preisGruppenEntites.Staffelpreis2.Value > 0)
					{
						var customPrice = staffelPreiEntities.Find(x => (x.Staffelpreis_Typ ?? "").Trim().Replace(" ", "").ToLower() == Enums.ArticleEnums.CustomPriceType.S2.GetDescription().Trim().Replace(" ", "").ToLower());
						if(customPrice == null)
						{
							Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Insert(
								new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = this._data,
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S2.GetDescription().Replace(" ", ""),
									Type = Enums.ArticleEnums.CustomPriceType.S2.GetDescription(),
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S2,
									Stundensatz = hourlyRate?.Count > 0 ? hourlyRate[0].HourlyRate : 0
								});
							customPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArtikelNrAndType(this._data, Enums.ArticleEnums.CustomPriceType.S2.GetDescription());
						}
						response.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(customPrice, preisGruppenEntites));
					}

					// - S3
					if(preisGruppenEntites.Staffelpreis3.HasValue && preisGruppenEntites.Staffelpreis3.Value > 0)
					{
						var customPrice = staffelPreiEntities.Find(x => (x.Staffelpreis_Typ ?? "").Trim().Replace(" ", "").ToLower() == Enums.ArticleEnums.CustomPriceType.S3.GetDescription().Trim().Replace(" ", "").ToLower());
						if(customPrice == null)
						{
							Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Insert(
								new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = this._data,
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S3.GetDescription().Replace(" ", ""),
									Type = Enums.ArticleEnums.CustomPriceType.S3.GetDescription(),
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S3,
									Stundensatz = hourlyRate?.Count > 0 ? hourlyRate[0].HourlyRate : 0
								});
							customPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArtikelNrAndType(this._data, Enums.ArticleEnums.CustomPriceType.S3.GetDescription());
						}
						response.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(customPrice, preisGruppenEntites));
					}

					// - S4
					if(preisGruppenEntites.Staffelpreis4.HasValue && preisGruppenEntites.Staffelpreis4.Value > 0)
					{
						var customPrice = staffelPreiEntities.Find(x => (x.Staffelpreis_Typ ?? "").Trim().Replace(" ", "").ToLower() == Enums.ArticleEnums.CustomPriceType.S4.GetDescription().Trim().Replace(" ", "").ToLower());
						if(customPrice == null)
						{
							Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Insert(
								new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = this._data,
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S4.GetDescription().Replace(" ", ""),
									Type = Enums.ArticleEnums.CustomPriceType.S4.GetDescription(),
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S4,
									Stundensatz = hourlyRate?.Count > 0 ? hourlyRate[0].HourlyRate : 0
								});
							customPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArtikelNrAndType(this._data, Enums.ArticleEnums.CustomPriceType.S4.GetDescription());
						}
						response.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(customPrice, preisGruppenEntites));
					}
				}

				// -
				return ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
			{
				return new ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>
				{
					Success = false,
					Errors = new List<ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>.ResponseError>
					{
						new ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>.ResponseError
						{
							Key= "",
							Value = "Article not found"
						}
					}
				};
			}

			return ResponseModel<List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>.SuccessResponse();
		}
	}
}
