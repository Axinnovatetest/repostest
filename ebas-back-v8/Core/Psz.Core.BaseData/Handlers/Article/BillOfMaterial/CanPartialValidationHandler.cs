using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class CanPartialValidationHandler: IHandle<UserModel, ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public CanPartialValidationHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}

		/// <summary>
		/// Can PartialValidate, if
		/// - has at least on snapshot BOM &&
		/// - last BOM has same kundenIndex as current BOM
		/// </summary>
		/// <returns></returns>
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
				var lastValidatedBom = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticle(this._data);

				Models.Article.BillOfMaterial.CanPartialValidationModel responseBody = null;
				if(lastValidatedBom == null)
				{
					responseBody = new Models.Article.BillOfMaterial.CanPartialValidationModel
					{
						CanPartialValidation = false
					};
				}
				else
				{
					// - no Partial Validation for First BOM
					var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);
					if(articleExtEntity.BomVersion <= 0)
					{
						responseBody = new Models.Article.BillOfMaterial.CanPartialValidationModel
						{
							CanPartialValidation = false
						};
					}

					if(lastValidatedBom.KundenIndex?.Trim()?.ToLower() == articleEntity.Index_Kunde?.Trim()?.ToLower())
					{
						responseBody = new Models.Article.BillOfMaterial.CanPartialValidationModel
						{
							CanPartialValidation = true,
							LastBomIndexKunde = lastValidatedBom.KundenIndex,
							LastBomVersion = lastValidatedBom.BomVersion
						};
					}
					else
					{
						responseBody = new Models.Article.BillOfMaterial.CanPartialValidationModel
						{
							CanPartialValidation = false,
						};
					}
				}


				// -
				return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.SuccessResponse(responseBody);
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

			if((Enums.ArticleEnums.BomStatus)(bomExtensionEntity?.BomStatusId ?? 0) != Enums.ArticleEnums.BomStatus.InPreparation)
			{
				return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.FailureResponse(key: "1", value: "cannot validate BOM not [In Preparation]");
			}

			return ResponseModel<Models.Article.BillOfMaterial.CanPartialValidationModel>.SuccessResponse();
		}

	}

}
