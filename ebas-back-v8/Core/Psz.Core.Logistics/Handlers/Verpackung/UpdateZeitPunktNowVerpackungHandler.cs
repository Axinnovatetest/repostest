using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Logistics.Handlers.Verpackung
{

	public class UpdateZeitPunktNowVerpackungHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private long _idArtikelAngebote { get; set; }
		public DateTime? _zeitPunkt { get; set; }
		public bool _packstatus { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateZeitPunktNowVerpackungHandler(long idArtikelAngebote, bool packstatus, Identity.Models.UserModel user)
		{
			this._user = user;
			this._idArtikelAngebote = idArtikelAngebote;
			this._zeitPunkt = DateTime.Now;
			this._packstatus = packstatus;
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



				var response = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.UpdatePackstatusZeitPunktGedrucktVerpackung(this._idArtikelAngebote, this._zeitPunkt, this._packstatus);
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