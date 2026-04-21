using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl
{
	public class GetArticleListEngineeringHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetArticleListEngineeringHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var results = Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.GetForEngineering();

			//-
			if(!this._user.IsGlobalDirector && !this._user.SuperAdministrator)
			{
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				if(userEntity != null && userEntity.CompanyId.HasValue && userEntity.CompanyId.Value > 0)
				{
					var companyLagerEntities = Infrastructure.Data.Access.Tables.CTS.lagerCompanyAccess.GetByCompanyId(userEntity.CompanyId.Value);
					if(companyLagerEntities != null && companyLagerEntities.Count > 0)
					{
						var articleExtensionEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(results.Select(x => x.ArticleNr).ToList());
						results = results.Where(x => isArticleInLager(x, companyLagerEntities, articleExtensionEntities))?.ToList();
					}
				}
			}

			return ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>.SuccessResponse(
						results?.Select(x => new Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel(x)
							).Distinct().ToList());
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>.SuccessResponse();
		}
		public static bool isArticleInLager(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity article,
			List<Infrastructure.Data.Entities.Tables.CTS.lagerCompanyEntity> lagerCompanyEntities,
			List<Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity> extensionEntities)
		{
			// - compare with Article's ending or the ProductionPlace1 from ArtikelProductionExtension
			var extArticle = extensionEntities.FirstOrDefault(x => x.ArticleId == article.ArticleNr);
			if(extArticle != null && extArticle.ProductionPlace1_Id.HasValue)
			{
				foreach(var lagerItem in lagerCompanyEntities)
				{
					if(lagerItem.Lagerort_id == extArticle.ProductionPlace1_Id)
					{
						return true;
					}
				}
			}
			else
			{
				return true;
			}

			// - 
			return false;
		}
	}
}
