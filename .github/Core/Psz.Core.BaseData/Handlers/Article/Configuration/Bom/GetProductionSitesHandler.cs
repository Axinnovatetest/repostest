using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Bom
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProductionSitesHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>>>
	{
		private UserModel _user { get; set; }
		public GetProductionSitesHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var productionPlaceEntites = Enum.GetValues(typeof(Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)).Cast<Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace>().ToList();
				if(productionPlaceEntites != null && productionPlaceEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>>.SuccessResponse(productionPlaceEntites
							.Select(x => new Models.Article.Configuration.Bom.ProductionSitesModel { Id = (int)x, Name = $"{x.GetDescription()}".Trim() }).Distinct().ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.Bom.ProductionSitesModel>>.SuccessResponse();
		}
	}
}
