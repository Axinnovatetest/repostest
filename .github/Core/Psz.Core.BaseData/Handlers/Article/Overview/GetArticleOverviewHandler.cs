using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class GetArticleOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.ArticleOverviewModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetArticleOverviewHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.ArticleOverviewModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleOverviewEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var articleExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data);
				var logEntities = Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.GetByObjetAndId(Enums.ObjectLogEnums.Objects.Article.GetDescription(), this._data, 5);
				var managersEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data);
				var artikelExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(articleOverviewEntity.ArtikelNr);
				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(articleOverviewEntity?.CustomerNumber??-1);
				var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adressenEntity?.Nr??-1);
				if(artikelExtensionEntity != null)
					articleOverviewEntity.ArticleImageId = artikelExtensionEntity.ImageId;

				var response = new Models.Article.ArticleOverviewModel(articleOverviewEntity, logEntities, managersEntity, adressenEntity, customerEntity);
				if(articleExtensionEntity != null)
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(articleExtensionEntity.CreatorID ?? -1);
					response.CreatedBy = (userEntity == null) ? "" : userEntity.Name;
					response.CreatedOn = articleExtensionEntity.DateCreation;
					response.OrderNumber = articleExtensionEntity.OrderNumber;
					response.Consumption12Months = articleExtensionEntity.Consumption12Months;
				}

				#region Tools
				if(articleOverviewEntity.ID_Klassifizierung == 10)
				{
					var tools = Infrastructure.Data.Access.Joins.BSD.TerminalToolsAccess.GetTerminlaTools(articleOverviewEntity.ArtikelNummer);
					if(tools != null && tools.Count > 0)
					{
						response.ToolsAL = tools.Where(t => t.Lager == (int)Enums.ArticleEnums.ProductionSites.AL).Select(t => t.Tool).OrderBy(x => x);
						response.ToolsTN = tools.Where(t => t.Lager == (int)Enums.ArticleEnums.ProductionSites.Tunesien).Select(t => t.Tool).OrderBy(x => x);
						response.ToolsBETN = tools.Where(t => t.Lager == (int)Enums.ArticleEnums.ProductionSites.BETN).Select(t => t.Tool).OrderBy(x => x);
						response.ToolsGZTN = tools.Where(t => t.Lager == (int)Enums.ArticleEnums.ProductionSites.GZ).Select(t => t.Tool).OrderBy(x => x);
						response.ToolsWSTN = tools.Where(t => t.Lager == (int)Enums.ArticleEnums.ProductionSites.WS).Select(t => t.Tool).OrderBy(x => x);
					}
				}
				#endregion
				return ResponseModel<Models.Article.ArticleOverviewModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.ArticleOverviewModel> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ArticleOverviewModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.ArticleOverviewModel>()
				{
					Errors = new List<ResponseModel<Models.Article.ArticleOverviewModel>.ResponseError>() {
						new ResponseModel<Models.Article.ArticleOverviewModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			//***
			return ResponseModel<Models.Article.ArticleOverviewModel>.SuccessResponse();
		}
	}
}