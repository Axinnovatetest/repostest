using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class UmbuchungslisteDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UmbuchungslisteSearch _data { get; set; }
		public UmbuchungslisteDetailsHandler(Identity.Models.UserModel user, UmbuchungslisteSearch data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				//----------------------------------------------------
				_data.Lieferant = (_data.Lieferant == null) ? "" : _data.Lieferant;
				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager == _data.Lager).ToList();
				var tbl_Planung_gestartet_Lagerort_ID = Module.LGT.tbl_Planung_gestartet_Lagerort_ID.ToList();
				var GenericResult = new PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult();
				var response_Details_II = new List<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Model>();
				var response_Details_III = new List<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Model>();
				var response_Details_IV = new List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV>();

				//------------Details II--------------
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_II_Entity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPsz_Disposition_UmbuchungList_Details_II(
					_data.withFG, _data.withoutFG, _data.Lieferant, _data.bis, ModelLGT.Select(x => x.Lager).First(), ModelLGT.Select(x => x.Lager_Id).First(), ModelLGT.Select(x => x.Lager_P_Id).First()
					, ModelLGT.Select(x => x.Lagerorte_Lagerort_id).First(), ModelLGT.Select(x => x.bestellte_Artikel_Lagerort_id).First(), tbl_Planung_gestartet_Lagerort_ID
					, _data.Stucklisten_Artikelnummer, null, null);
				if(Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_II_Entity != null && Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_II_Entity.Count > 0)
					response_Details_II = Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_II_Entity.Select(k => new PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Model(k)).ToList();
				//------------Details III--------------
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_III_Entity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPsz_Disposition_UmbuchungList_Details_III(

					_data.withFG, _data.withoutFG, _data.Lieferant, _data.bis, ModelLGT.Select(x => x.Lager).First(), ModelLGT.Select(x => x.Lager_Id).First(), ModelLGT.Select(x => x.Lager_P_Id).First()
					, ModelLGT.Select(x => x.Lagerorte_Lagerort_id).First(), ModelLGT.Select(x => x.bestellte_Artikel_Lagerort_id).First(), tbl_Planung_gestartet_Lagerort_ID
					, _data.Stucklisten_Artikelnummer, null, null);
				if(Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_III_Entity != null && Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_III_Entity.Count > 0)
					response_Details_III = Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_III_Entity.Select(k => new PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Model(k)).ToList();


				//--------Details IV
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV_Entity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPsz_Disposition_UmbuchungList_Details_IV(

	_data.withFG, _data.withoutFG, _data.Lieferant, _data.bis, ModelLGT.Select(x => x.Lager).First(), ModelLGT.Select(x => x.Lager_Id).First(), ModelLGT.Select(x => x.Lager_P_Id).First()
	, ModelLGT.Select(x => x.Lagerorte_Lagerort_id).First(), ModelLGT.Select(x => x.bestellte_Artikel_Lagerort_id).First(), tbl_Planung_gestartet_Lagerort_ID
	, _data.Stucklisten_Artikelnummer, null, null);
				if(Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV_Entity != null && Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV_Entity.Count > 0)
					response_Details_IV = Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV_Entity.Select(k => new Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV(k)).ToList();

				//-------Result-------------
				GenericResult.List_Details_II = response_Details_II;
				GenericResult.List_Details_III = response_Details_III;
				GenericResult.List_Details_IV = response_Details_IV;
				GenericResult.Lieferent_Details_III = response_Details_III.Select(x => x.Name1).First();
				GenericResult.EK = Convert.ToDecimal(String.Format("{0:0.000}", response_Details_III.Select(x => x.Einkaufspreis).First()));
				GenericResult.MOQ = response_Details_III.Select(x => x.Mindestbestellmenge).First();
				GenericResult.std_Lief_Zeit = response_Details_III.Select(x => x.Wiederbeschaffungszeitraum).First();
				GenericResult.Telefon = response_Details_III.Select(x => x.Telefon).First();
				GenericResult.Fax = response_Details_III.Select(x => x.Fax).First();
				GenericResult.Bestell_Nr = response_Details_III.Select(x => x.Bestell_Nr).First();
				GenericResult.Bestant_List_Details_IV = response_Details_IV.Select(x => x.Bestand).First();
				return ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult>.SuccessResponse(GenericResult);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult>.AccessDeniedResponse();
			}

			return ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult>.SuccessResponse();
		}
	}
}
