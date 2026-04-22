using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class UmbuchungslisteHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UmbuchungslisteSearch _data { get; set; }
		public UmbuchungslisteHandler(Identity.Models.UserModel user, UmbuchungslisteSearch data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				//----------------------------Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse------------------------
				_data.Lieferant = (_data.Lieferant == null) ? "" : _data.Lieferant;
				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager == _data.Lager).ToList();
				var tbl_Planung_gestartet_Lagerort_ID = Module.LGT.tbl_Planung_gestartet_Lagerort_ID.ToList();
				var response = new List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>();
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity(
					_data.withFG, _data.withoutFG, _data.Lieferant, _data.bis, ModelLGT.Select(x => x.Lager).First(), ModelLGT.Select(x => x.Lager_Id).First(), ModelLGT.Select(x => x.Lager_P_Id).First()
					, ModelLGT.Select(x => x.Lagerorte_Lagerort_id).First(), ModelLGT.Select(x => x.bestellte_Artikel_Lagerort_id).First(), tbl_Planung_gestartet_Lagerort_ID
					, null, null);
				if(Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity != null && Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity.Count > 0)
					response = Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity.Select(k => new Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse(k)).ToList();


				return ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>>.SuccessResponse();
		}
	}
}
