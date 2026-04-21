using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class GetBOMStatusDetails: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetBOMStatusDetails(Identity.Models.UserModel user, int articleID)
		{
			_user = user;
			_data = articleID;
		}
		public ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var bomExtensionEntiy = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var listArticleCossArtikelEntity = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity>();
				if(articleEntity != null)
				{
					listArticleCossArtikelEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelOutilCossAccess.Get(this._data, articleEntity.ProductionSiteSequence.ToString());
				}

				bool? statusOutilCoss = null;

				if(listArticleCossArtikelEntity != null && listArticleCossArtikelEntity.Count() > 0)
				{
					statusOutilCoss = true;
					foreach(var item in listArticleCossArtikelEntity)
					{
						if(!item.Outil.ToUpper().Contains("TN AB"))
						{
							statusOutilCoss = false;
						}
					}
				}
				if(bomExtensionEntiy == null)
				{
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Insert(
						new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
						{
							ArticleDesignation = articleEntity.Bezeichnung1,
							ArticleId = this._data,
							ArticleNumber = articleEntity.ArtikelNummer,
							BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
							BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
							BomValidFrom = null,
							BomVersion = 0,
							Id = -1,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = this._user.Id,
							KontakteStatus = statusOutilCoss
						});
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditCPRequirement(this._data, true);
					bomExtensionEntiy = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
				}
				else
				{
					bomExtensionEntiy.KontakteStatus = statusOutilCoss;
				}

				return ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel>.SuccessResponse(
					new Models.Article.BillOfMaterial.BOMStatusDetailsModel(bomExtensionEntiy, articleEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel> Validate()
		{
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel>()
				{
					Errors = new List<ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel>.ResponseError>{
							new ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel>.ResponseError() { Key = "", Value = "Article not found" }
						}
				};
			}
			return ResponseModel<Models.Article.BillOfMaterial.BOMStatusDetailsModel>.SuccessResponse();
		}
	}
}
