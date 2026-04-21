using Psz.Core.Common.Models;
using Psz.Core.Logistics.Helpers;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class UpdateGedrucktVDA: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private ListeUpdatePacking _listeUpdatePacking { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateGedrucktVDA(ListeUpdatePacking listeUpdatePacking, Identity.Models.UserModel user)
		{
			this._user = user;
			this._listeUpdatePacking = listeUpdatePacking;
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

				List<long> nrAngeboteArtikel = this._listeUpdatePacking.listeLieferscheine;

				var response = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.UpdateGedrucktVDA(nrAngeboteArtikel);
				//logging
				string customer = "Nothink";
				var listeVDA = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetVDAByAngeboteArtikelNr(nrAngeboteArtikel[0]);
				if(listeVDA != null && listeVDA.Count() > 0)
				{
					customer = listeVDA[0].lVornameNameFirma;
				}

				string ch = "";
				int indice = 1;
				foreach(var item in nrAngeboteArtikel)
				{
					if(indice == 1)
					{
						ch += item;
					}
					else
					{
						ch += "," + item;
					}
					indice++;
				}
				var _log = new LogHelper(0,
					 0, "VDA", LogHelper.LogType.CREATEVDA, "LGT", _user)
					.LogLGT(null, customer, null, 0);
				Infrastructure.Data.Access.Tables.Logistics.Logistics_LogAccess.Insert(_log);

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
			//var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);
			//if(faEntity == null)
			//	return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
