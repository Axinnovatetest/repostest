using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA
{
	public class LinkFaPositionToUBGHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private LinkFaPositionHBGRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public LinkFaPositionToUBGHandler(LinkFaPositionHBGRequestModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
				// - UBG
				var ubgFa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.UBGFaId);
				logs.Add(new Helpers.LogHelper(ubgFa.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONFA, "CTS", _user)
						.LogCTS("HBGFAPositionId", $"{ubgFa.HBGFAPositionId}", $"{this._data.HBGFaPositionId}", 0));
				ubgFa.HBGFAPositionId = this._data.HBGFaPositionId;
				Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(ubgFa, botransaction.connection, botransaction.transaction);

				// - HBG
				var hbgFaPosition = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.Get(this._data.HBGFaPositionId);
				var hbgFa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(hbgFaPosition.ID_Fertigung ?? -1);
				logs.Add(new Helpers.LogHelper(hbgFa.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONFA, "CTS", _user)
						.LogCTS("IsUBG", $"{hbgFaPosition.IsUBG}", $"{true}", 0));
				logs.Add(new Helpers.LogHelper(hbgFa.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONFA, "CTS", _user)
						.LogCTS("UBGFertigungsId", $"{hbgFaPosition.UBGFertigungsId}", $"{hbgFa.ID}", 0));
				logs.Add(new Helpers.LogHelper(hbgFa.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONFA, "CTS", _user)
						.LogCTS("UBGFertigungsnummer", $"{hbgFaPosition.UBGFertigungsnummer}", $"{hbgFa.Fertigungsnummer}", 0));
				hbgFaPosition.IsUBG = true;
				hbgFaPosition.UBGFertigungsId = hbgFa.ID;
				hbgFaPosition.UBGFertigungsnummer = hbgFa.Fertigungsnummer;
				Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.UpdateWithTransaction(hbgFaPosition, botransaction.connection, botransaction.transaction);

				//commiting
				if(botransaction.commit())
				{
					//logging
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(logs);
				}

				// -
				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				botransaction.rollback();
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

			var ubgFaEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.UBGFaId);
			if(ubgFaEntity == null || ubgFaEntity.Kennzeichen?.ToLower() == "STORNO")
			{
				return ResponseModel<int>.FailureResponse($"Cannot assign canceled UBG FA");
			}

			var hbgFaPosition = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.Get(this._data.HBGFaPositionId);
			if(hbgFaPosition == null)
			{
				return ResponseModel<int>.FailureResponse($"Position not found");
			}
			var hbgFa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(hbgFaPosition.ID_Fertigung ?? -1);
			if(hbgFa == null)
			{
				return ResponseModel<int>.FailureResponse($"HBG FA not found");
			}
			if(hbgFa.Kennzeichen?.ToLower() == "storno")
			{
				return ResponseModel<int>.FailureResponse($"HBG FA is canceled");
			}

			// 
			if(ubgFaEntity.Artikel_Nr != hbgFaPosition.Artikel_Nr)
			{
				return ResponseModel<int>.FailureResponse($"UBG Article is different from HBG FA Article");
			}
			if(ubgFaEntity.Anzahl != hbgFaPosition.Anzahl)
			{
				return ResponseModel<int>.FailureResponse($"UBG Quantity is different from HBG FA Quantity");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}