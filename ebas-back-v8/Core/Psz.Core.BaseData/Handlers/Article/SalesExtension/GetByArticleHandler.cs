using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByArticleHandler: IHandle<UserModel, ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>>
	{
		private UserModel _user
		{
			get;
			set;
		}
		public int _data
		{
			get;
			set;
		}
		public GetByArticleHandler(UserModel user, int articleNr)
		{
			this._user = user;
			this._data = articleNr;
		}
		public ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var packagingEntities = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get();

				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var preisGruppenEntites = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(this._data);
				var artikelKalkulEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(this._data);
				var salesItemSeries = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(this._data, (int)Common.Enums.ArticleEnums.SalesItemType.Serie);
				if(salesItemSeries == null && preisGruppenEntites != null)
				{
					var artikelExtension = new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity
					{
						ArticleNr = this._data,
						ArticleSalesType = Common.Enums.ArticleEnums.SalesItemType.Serie.GetDescription(),
						ArticleSalesTypeId = (int)Common.Enums.ArticleEnums.SalesItemType.Serie,
						Preisgruppe = 1
					};

					if(artikelEntity != null)
					{
						var vp = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.GetByNummer(artikelEntity.Verpackungsart);
						artikelExtension.Profuktionszeit = (decimal?)artikelEntity.Produktionszeit;
						artikelExtension.Stundensatz = artikelEntity.Stundensatz;
						artikelExtension.Verpackungsart = artikelEntity.Verpackungsart;
						artikelExtension.VerpackungsartId = vp?.Id;
						artikelExtension.Verpackungsmenge = artikelEntity.Verpackungsmenge;
						artikelExtension.Losgroesse = artikelEntity.Losgroesse;
					}

					if(preisGruppenEntites != null)
					{
						artikelExtension.Verkaufspreis = (decimal?)preisGruppenEntites.Verkaufspreis;
					}

					if(artikelKalkulEntities != null)
					{
						artikelExtension.Produktionskosten = artikelKalkulEntities.Betrag;
					}

					var id = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Insert(artikelExtension);

					salesItemSeries = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(id);
				}
				//
				var responseBody = new List<Models.Article.SalesExtension.SalesItemModel>();

				var salesItemSerie = new Models.Article.SalesExtension.SalesItemModel(salesItemSeries, artikelEntity, preisGruppenEntites, artikelKalkulEntities, salesItemSeries?.VerpackungsartId.HasValue == true ? packagingEntities?.FirstOrDefault(y => y.Artikelnummer?.ToLower()?.Trim() == salesItemSeries?.Verpackungsart?.ToLower()?.Trim()) : null);
				if(preisGruppenEntites != null)
					responseBody.Add(salesItemSerie);

				var salesItemEntites = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNr(this._data);
				if(salesItemEntites != null && salesItemEntites.Count > 0)
				{
					var otherSalesItems = salesItemEntites
					  .Where(x => x.ArticleSalesType.ToLower() != "serie").Select(x => new Models.Article.SalesExtension.SalesItemModel(x, x.VerpackungsartId.HasValue ? packagingEntities?.FirstOrDefault(y => y.Artikelnummer?.ToLower()?.Trim() == x.Verpackungsart?.ToLower()?.Trim()) : null))?.ToList();

					if(otherSalesItems != null && otherSalesItems.Count > 0)
						responseBody.AddRange(otherSalesItems);
				}

				// - 2025-11-28 - show DB/Marge - Khelil/Zipproth
				var margeEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetArticleMarge(artikelEntity.ArtikelNr);
				if(margeEntities?.Count()>0)
				{
					for(int i = 0; i < responseBody.Count; i++)
					{
						var m = margeEntities.FirstOrDefault(x => x.PriceType.Equals(responseBody[i].Type, StringComparison.OrdinalIgnoreCase) == true);
						if(m != null)
						{
							responseBody[i].DbWithCU = m.DBMitCu;
							responseBody[i].DbWithoutCU = m.DB;
							responseBody[i].MargeWithCU = m.MargeMitCu;
							responseBody[i].MargeWithoutCU = m.Marge;
						}
					}
				}

				return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>> Validate()
		{
			if(this._user == null
			/*
                            || this._user.Access.____*/
			)
			{
				return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.SuccessResponse();
		}
	}
}