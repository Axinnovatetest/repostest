using Psz.Core.Common.Models;
using System;


namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class UpdateVersandStatusHandler
	{
		private long _idArtikelAngebote { get; set; }
		public bool _versandStatus { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateVersandStatusHandler(long idArtikelAngebote, bool versandStatus, Identity.Models.UserModel user)
		{
			this._user = user;
			this._idArtikelAngebote = idArtikelAngebote;
			this._versandStatus = versandStatus;
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



				var response = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.UpdateVersandStatut(this._idArtikelAngebote, this._versandStatus);
				//logging


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
			if(this._idArtikelAngebote == 0 || this._idArtikelAngebote == null)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Vepackung not found");
			}
			//var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);
			//if(faEntity == null)
			//	return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");
			return ResponseModel<int>.SuccessResponse();
		}

	}
}