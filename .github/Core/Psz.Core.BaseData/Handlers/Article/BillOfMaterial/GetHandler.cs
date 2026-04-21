using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.BillOfMaterial.Bom>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int articleId)
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

				var bomEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				if(bomEntity == null)
				{
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
					{
						Id = -1,
						ArticleId = this._data,
						ArticleNumber = articleEntity.ArtikelNummer,
						ArticleDesignation = articleEntity.Bezeichnung1,
						BomVersion = 0,
						BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
						BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
						BomValidFrom = DateTime.Today,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = this._user.Id
					});
					bomEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
				}
				var bomItemEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
				var bomItemAltEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBoms(bomItemEntities?.Select(x => x.Nr)?.ToList());

				return ResponseModel<Models.Article.BillOfMaterial.Bom>.SuccessResponse(
					new Models.Article.BillOfMaterial.Bom(bomEntity, bomItemEntities, bomItemAltEntities, articleEntity));
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
