using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public DeleteHandler(UserModel user, int id)
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

				var positionAlt = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Get(this._data);
				var originalPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(positionAlt.OriginalStucklistenNr);
				var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)originalPosition.Artikel_Nr);
				// -- logs
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, originalPosition.Artikel_Nr ?? 0, "Article",
						$"[{this._data}] {positionAlt.ArtikelNr}", "",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));
				}
				// -- other logs
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, this._data, 1, parentArticle.ArtikelNummer, null, positionAlt.Anzahl.ToString(),
					null, positionAlt.ArtikelNummer, Enums.ObjectLogEnums.BOMLogType.Delete);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Delete(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Get(this._data) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "BOM position not found");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
