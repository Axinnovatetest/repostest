using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl
{
	public class GetArticleListEngineeringCountHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public GetArticleListEngineeringCountHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<int> Handle()
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
						results = results.Where(x => GetArticleListEngineeringHandler.isArticleInLager(x, companyLagerEntities, articleExtensionEntities))?.ToList();
					}
				}
			}

			return ResponseModel<int>.SuccessResponse(results.Count);
			//return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.GetForEngineeringCount());
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
