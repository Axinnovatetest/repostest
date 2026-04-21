using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.BomPositionEdit _data { get; set; }
		public AddHandler(UserModel user, Models.Article.BillOfMaterial.BomPositionEdit item)
		{
			_user = user;
			_data = item;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// Supposed to come without article designation
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
				var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleParentId);
				this._data.ArticleNumber = article.ArtikelNummer;
				this._data.ArticleDesignation = article.Bezeichnung1;
				this._data.ArticleId = article.ArtikelNr;
				var response = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Insert(this._data.ToEntity());
				// -- logs
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleParentId, "BOM Position", "",
						$"{this._data.ArticleNumber}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));
				}
				// -- other logs
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data.Id, 0, parentArticle.ArtikelNummer, null, this._data.Quantity.ToString(),
					null, this._data.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.Add);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
			var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleParentId);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Article [{this._data.ArticleNumber}] not found");

			if(parentArticle == null)
				return ResponseModel<int>.FailureResponse(key: "2", value: $"Parent Article not found");

			if(articleEntity.ArtikelNr == this._data.ArticleParentId)
				return ResponseModel<int>.FailureResponse(key: "3", value: $"Article [{this._data.ArticleNumber}] is same as parent");

			//if (articleEntity.ArtikelNr == this._data.ArticleId)
			//    return ResponseModel<int>.FailureResponse(key: "4", value: $"Article [{this._data.ArticleNumber}] already exsist in BOM");

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByParentAndChildID(this._data.ArticleParentId, articleEntity.ArtikelNr).Count > 0)
				return ResponseModel<int>.FailureResponse(key: "3", value: $"Article [{this._data.ArticleNumber}] Already in BOM");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
