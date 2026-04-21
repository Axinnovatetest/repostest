using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public class UpdateFAUBGHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private ChangeFAUBGEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateFAUBGHandler(ChangeFAUBGEntryModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
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
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(_data.Fertigungsnummer);
				faEntity.UBG = _data.UBG;
				var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faEntity);
				//logging
				var _log = new Helpers.LogHelper(_data.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONFA, "CTS", _user)
					.LogCTS("UBG", (!faEntity.UBG).ToString(), faEntity.UBG.ToString(), 0);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(_data.Fertigungsnummer);
			if(faEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");

			// - 2022-09-06 - allow change in spite of quantity status
			//if (faEntity.Anzahl_erledigt > 0)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"FA has open quantity, UBG update impossible");
			//var VersandQty = Infrastructure.Data.Access.Joins.CTS.Divers.GetFAVersandQty(_data.Fertigungsnummer, GetVersandTable((int)faEntity.Lagerort_id));
			//if (VersandQty > 0)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"FA has printed lables, UBG update impossible");

			return ResponseModel<int>.SuccessResponse();
		}
		internal static string GetVersandTable(int lager)
		{
			string tbl = null;
			switch(lager)
			{
				case 26:
				case 156://temp al
					tbl = "[PSZAL_Lieferliste täglich]";
					break;
				case 7:
					tbl = "[PSZTN_Lieferliste täglich]";
					break;
				case 60:
					tbl = "[PSZBETN_Lieferliste täglich]";
					break;
				case 42:
					tbl = "[PSZKsarHelal_Lieferliste täglich]";
					break;
				case 6:
				case 21:
					tbl = "[PSZ_Lieferliste täglich]";
					break;
				case 15:
					tbl = "[PSZ_Einlagerung_täglich]";
					break;
			}
			return tbl;
		}
	}
}