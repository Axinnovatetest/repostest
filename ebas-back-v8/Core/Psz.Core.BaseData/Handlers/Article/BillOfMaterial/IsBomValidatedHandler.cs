using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class IsBomValidatedHandler: IHandle<UserModel, ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public IsBomValidatedHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}

		public ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var lastValidatedBom = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticle(articleEntity.ArtikelNr);
				var validatedBom = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndKundenIndex(articleEntity.ArtikelNr, articleEntity.Index_Kunde);
				var bomExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(articleEntity.ArtikelNr);

				// -
				return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.SuccessResponse(new Models.Article.BillOfMaterial.CanPartialValidationModel
				{
					LastBomIndexKunde = lastValidatedBom?.KundenIndex,
					LastBomVersion = lastValidatedBom?.BomVersion,
					Value = validatedBom?.Count > 0 || bomExtensionEntity?.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved
				});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.FailureResponse(key: "1", value: "Article not found");
			}

			var bomExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
			if(bomExtensionEntity == null
				&& Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data) == null)
				return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.FailureResponse(key: "1", value: "BOM item not found");

			return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.SuccessResponse();
		}

	}

}
