using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.BomPositionAltEdit _data { get; set; }
		public AddHandler(UserModel user, Models.Article.BillOfMaterial.BomPositionAltEdit position)
		{
			_user = user;
			_data = position;
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

				// 
				var originalPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data.OriginalPositionId);
				var maxPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetMaxPositionByOriginalBom(this._data.OriginalPositionId);
				var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(/*this._data.ParentArticleId*/(int)originalPosition.Artikel_Nr);
				// Supposed to come without article designation and number
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
				this._data.ArticleNumber = article.ArtikelNummer;
				this._data.ArticleDesignation = article.Bezeichnung1;
				this._data.Position = (maxPosition + 10).ToString("D3");
				this._data.ParentArticleId = parentArticle.ArtikelNr;

				// -- logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, originalPosition.Artikel_Nr ?? 0, "BOM Position ALT", "",
					$"{this._data.ArticleNumber}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Add));

				// -- other logs
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data.OriginalPositionId, 1, parentArticle.ArtikelNummer, null, this._data.Quantity.ToString(),
					null, this._data.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.Add);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Insert(this._data.ToEntity()));
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
			if(this._data.ArticleNumber == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Article number must not be empty");

			var parentPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data.OriginalPositionId);
			if(parentPosition == null)
				return ResponseModel<int>.FailureResponse(key: "2", value: "BOM original position not found");

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "3", value: $"Article [{this._data.ArticleNumber}] not found");

			if(parentPosition.Artikelnummer?.Trim().ToLower() == articleEntity.ArtikelNummer?.Trim().ToLower())
				return ResponseModel<int>.FailureResponse(key: "4", value: $"Article [{this._data.ArticleNumber}] is same as parent Position");

			if(parentPosition.Artikel_Nr == articleEntity.ArtikelNr)
				return ResponseModel<int>.FailureResponse(key: "5", value: $"Article [{this._data.ArticleNumber}] is the same as main Article");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
