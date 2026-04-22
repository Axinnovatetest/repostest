using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class GetArticleBomHandler: IHandle<UserModel, ResponseModel<Models.Article.BillOfMaterial.Bom>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetArticleBomHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}
		public ResponseModel<Models.Article.BillOfMaterial.Bom> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articlesNrsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetNrsAndNummer();
				var bomItemEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);

				var joint_1 = (from bm in bomItemEntities
							   join art in articlesNrsEntity on bm.Artikel_Nr equals art.ArtikelNr
							   select bm).Distinct();

				var joint_2 = (from jn in joint_1
							   join art in articlesNrsEntity on jn.Artikel_Nr_des_Bauteils equals art.ArtikelNr
							   select jn).Distinct();

				var bomExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
				var bomItemAltEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBoms(joint_2?.Select(x => x.Nr)?.ToList());
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				if(bomExtensionEntity == null)
				{
					Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity Added_entity = new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity();
					Added_entity.ArticleDesignation = articleEntity.Bezeichnung2;
					Added_entity.ArticleNumber = articleEntity.ArtikelNummer;
					Added_entity.ArticleId = articleEntity.ArtikelNr;
					Added_entity.BomVersion = 0;
					Added_entity.BomStatus = Enums.ArticleEnums.BomStatus.Approved.GetDescription();
					Added_entity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.Approved;
					Added_entity.BomValidFrom = DateTime.Now;
					Added_entity.LastUpdateTime = DateTime.Now;
					Added_entity.LastUpdateUserId = this._user.Id;
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Insert(Added_entity);

				}
				var NewbomExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
				return ResponseModel<Models.Article.BillOfMaterial.Bom>.SuccessResponse(
					new Models.Article.BillOfMaterial.Bom(NewbomExtensionEntity, joint_2.ToList(), bomItemAltEntities, articleEntity));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<Models.Article.BillOfMaterial.Bom> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.BillOfMaterial.Bom>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data) == null
				&& Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data) == null)
				return ResponseModel<Models.Article.BillOfMaterial.Bom>.FailureResponse(key: "1", value: "BOM item not found");


			return ResponseModel<Models.Article.BillOfMaterial.Bom>.SuccessResponse();
		}
	}
}
