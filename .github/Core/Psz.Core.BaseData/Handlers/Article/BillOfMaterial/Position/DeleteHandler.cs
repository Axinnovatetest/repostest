using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				// TODO: Check if article in ongoing production
				var bom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data);
				var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)bom.Artikel_Nr);
				// -- logs
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, bom.Artikel_Nr ?? 0, $"[{this._data}] BOM Position",
						$"{bom.Artikel_Nr}", "",
						$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
						Enums.ObjectLogEnums.LogType.Delete));
				}
				// -- other logs
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data, 0, parentArticle.ArtikelNummer, null, bom.Anzahl.ToString(),
					null, bom.Artikelnummer, Enums.ObjectLogEnums.BOMLogType.Delete);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Delete(this._data));

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

			var bom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data);
			if(bom == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "BOM position not found");

			var articleEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(bom.Artikel_Nr ?? 0);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "BOM article not found");
			if(articleEntity.BomStatusId != (int)Enums.ArticleEnums.BomStatus.InPreparation)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Cannot update BOM while article status is '{articleEntity.BomStatus}'");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
