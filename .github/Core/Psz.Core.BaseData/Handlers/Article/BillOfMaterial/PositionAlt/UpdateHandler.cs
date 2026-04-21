using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.BomPositionAltEdit _data { get; set; }
		public UpdateHandler(UserModel user, Models.Article.BillOfMaterial.BomPositionAltEdit positionAlt)
		{
			_user = user;
			_data = positionAlt;
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
				var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ParentArticleId);
				// Supposed to come without article designation and number
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
				this._data.ArticleId = article.ArtikelNr;
				this._data.ArticleNumber = article.ArtikelNummer;
				this._data.ArticleDesignation = article.Bezeichnung1;

				// -- logs
				var stucklistEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Get(this._data.Id);
				if(stucklistEntity.ArtikelNr != this._data.ArticleId || stucklistEntity.ArtikelNummer != this._data.ArticleNumber)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, originalPosition.Artikel_Nr ?? 0, "BOM Position ALT", $"{stucklistEntity.ArtikelNummer}",
						$"{this._data.ArticleNumber}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
				}
				if(stucklistEntity.Anzahl != this._data.Quantity)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, originalPosition.Artikel_Nr ?? 0, "BOM Position ALT Quantity", $"{stucklistEntity.Anzahl}",
						$"{this._data.Quantity}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
				}
				if(this._data.DocumentData != null && this._data.DocumentData.Length > 10) // Don't ask me why 10!
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, originalPosition.Artikel_Nr ?? 0, "BOM Position ALT Document", $"{stucklistEntity.DocumentId}",
						$"{this._data.DocumentId}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
				}
				// -- other logs
				if(stucklistEntity.ArtikelNr != this._data.ArticleId || stucklistEntity.ArtikelNummer != this._data.ArticleNumber)
				{
					var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data.Id, 1, parentArticle.ArtikelNummer, stucklistEntity.Anzahl.ToString(), null,
					stucklistEntity.ArtikelNummer, this._data.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.EditArt);
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);
				}
				if(stucklistEntity.Anzahl != this._data.Quantity)
				{
					var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data.Id, 1, parentArticle.ArtikelNummer, stucklistEntity.Anzahl.ToString(), this._data.Quantity.ToString(),
					null, stucklistEntity.ArtikelNummer, Enums.ObjectLogEnums.BOMLogType.EditQty);
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Update(this._data.ToEntity()));
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

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Get(this._data.Id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "BOM position not found");

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Article [{this._data.ArticleNumber}] not found");

			var parentPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data.OriginalPositionId);
			if(parentPosition.Artikelnummer?.Trim().ToLower() == articleEntity.ArtikelNummer?.Trim().ToLower())
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Article [{this._data.ArticleNumber}] is the same as parent Position");

			if(parentPosition.Artikel_Nr == articleEntity.ArtikelNr)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Article [{this._data.ArticleNumber}] is the same as main Article");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
