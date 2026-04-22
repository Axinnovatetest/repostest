using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.BomPositionEdit _data { get; set; }
		public UpdateHandler(UserModel user, Models.Article.BillOfMaterial.BomPositionEdit articleChild)
		{
			_user = user;
			_data = articleChild;
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

				// -- logs
				var stucklistEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data.Id);
				if(stucklistEntity.Artikel_Nr_des_Bauteils != this._data.ArticleId || stucklistEntity.Artikelnummer != this._data.ArticleNumber)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleParentId, $"[{this._data.Id}] Article", $"{stucklistEntity.Artikelnummer}",
						$"{this._data.ArticleNumber}",
						$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
						Enums.ObjectLogEnums.LogType.Edit));
				}
				if(stucklistEntity.Anzahl != (double?)this._data.Quantity)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleParentId, $"[{this._data.Id}] BOM Position Quantity", $"{stucklistEntity.Anzahl}",
						$"{this._data.Quantity}",
						$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
						Enums.ObjectLogEnums.LogType.Edit));
				}
				if(this._data.DocumentData != null && this._data.DocumentData.Length > 10) // Don't ask me why 10!
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleParentId, $"[{this._data.Id}] BOM Position Document", $"{stucklistEntity.DocumentId}",
						$"{this._data.DocumentId}",
						$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
						Enums.ObjectLogEnums.LogType.Edit));
				}
				// -- other logs
				if(stucklistEntity.Artikel_Nr_des_Bauteils != this._data.ArticleId || stucklistEntity.Artikelnummer != this._data.ArticleNumber)
				{
					var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data.Id, 0, parentArticle.ArtikelNummer, stucklistEntity.Anzahl.ToString(), null,
					stucklistEntity.Artikelnummer, this._data.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.EditArt);
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);
				}
				if(stucklistEntity.Anzahl != (double?)this._data.Quantity)
				{
					var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data.Id, 0, parentArticle.ArtikelNummer, stucklistEntity.Anzahl.ToString(), this._data.Quantity.ToString(),
					null, stucklistEntity.Artikelnummer, Enums.ObjectLogEnums.BOMLogType.EditQty);
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Update(this._data.ToEntity()));
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

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data.Id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "BOM position not found");

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "2", value: $"Article [{this._data.ArticleNumber}] not found");

			if(articleEntity.ArtikelNr == this._data.ArticleParentId)
				return ResponseModel<int>.FailureResponse(key: "3", value: $"Article [{this._data.ArticleNumber}] is same as parent");

			var exsistant_children = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByParentAndChildID(this._data.ArticleParentId, articleEntity.ArtikelNr);
			if(exsistant_children != null && exsistant_children.Count > 0)
				exsistant_children = exsistant_children.Except(exsistant_children.Where(x => x.Nr == this._data.Id).ToList()).ToList();

			if(exsistant_children.Count > 0)
				return ResponseModel<int>.FailureResponse(key: "3", value: $"Article [{this._data.ArticleNumber}] already exsists in BOM");

			if(this._data.Quantity <= 0)
				return ResponseModel<int>.FailureResponse(key: "3", value: "Quanity cannot be null or Negative");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
