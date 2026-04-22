using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListChoosePackingHandler: IHandle<Identity.Models.UserModel, ResponseModel<PackingGlobalChooseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public DateTime? datum { get; set; }
		public string kunde { get; set; }
		public string verpakungart { get; set; }

		public GetListChoosePackingHandler(DateTime? Datum, string Kunde, string Verpakungart, Identity.Models.UserModel user)
		{
			this._user = user;
			this.datum = Datum;
			this.kunde = Kunde;
			this.verpakungart = Verpakungart;
		}
		public ResponseModel<PackingGlobalChooseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new PackingGlobalChooseModel();
				var _ListeKunden = new List<KeyValuePair<string, string>>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeChoosePacking(this.datum, this.kunde, this.verpakungart);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
				{
					response.listeVerpackung = PackingListEntity.Select(k => new PackingChooseModel(k)).OrderBy(K => K.angeboteNr).ToList();
					//var x= (response.listeVerpackung).OrderBy(K => K.angeboteNr).ToList();
					foreach(var item in response.listeVerpackung)
					{

						decimal verpckungsGewich = 0;
						if(item.verpackungGewicht == null)
						{
							verpckungsGewich = 0;
						}
						else
						{
							verpckungsGewich = (decimal)item.verpackungGewicht;
						}
						int verpckungsMeng = 0;
						if(item.verpackungsmenge == null)
						{
							verpckungsMeng = 0;
						}
						else
						{
							verpckungsMeng = (int)item.verpackungsmenge;
						}
						item.gesammtGewicht = (item.verpackungsart == null) || item.verpackungsart.ToLower().Contains("gitterbox") || item.verpackungsart.ToLower().Contains("um-") ? item.gewichtArtikel * item.anzahl / 1000 : item.anzahl <= verpckungsMeng ? item.gewichtArtikel * item.anzahl / 1000 + verpckungsGewich / 1000 : ((decimal)item.gewichtArtikel * item.anzahl / 1000) + ((decimal)((decimal)verpckungsGewich / 1000 * (verpckungsMeng == 0 ? 0 : (decimal)item.anzahl / verpckungsMeng)));
						if(item.gesammtGewicht == 0 && item.verpackungsart == "")
						{
							item.gesammtGewicht = null;
						}
						if(item.verpackungGewicht == null && !((item.verpackungsart == null) || item.verpackungsart.ToLower().Contains("gitterbox") || item.verpackungsart.ToLower().Contains("um-")))
						{
							item.gesammtGewicht = null;
						}
					}
					response.listeDatum = PackingListEntity/*.Where(c=>c.versanddatum_Auswahl!=null)*/.Select(c => new { c.versanddatum_Auswahl }).Distinct().OrderBy(x => x.versanddatum_Auswahl).Select(x => x.versanddatum_Auswahl).ToList();
					//response.listeDatum = PackingListEntity.Select(t => t.versanddatum_Auswahl).Distinct().ToList();
					response.listeVersandenArten = PackingListEntity.Select(t => t.versandarten_Auswahl).Distinct().ToList();
					response.listeFirma = PackingListEntity.Select(c => new { c.lVornameNameFirma }).Distinct().OrderBy(x => x.lVornameNameFirma).Select(x => x.lVornameNameFirma).ToList();
					//response.listeFirma = PackingListEntity.Select(t => t.lVornameNameFirma).Distinct().ToList();
					var _lsiteNameFirma = PackingListEntity.Select(c => new { c.lVornameNameFirma, c.lName2, c.lStrassePostfach, c.lLandPLZORT }).Distinct().OrderBy(x => x.lVornameNameFirma).Select(x => new { x.lVornameNameFirma, x.lName2, x.lStrassePostfach, x.lLandPLZORT }).ToList();
					foreach(var item in _lsiteNameFirma)
					{
						_ListeKunden.Add(new KeyValuePair<string, string>(item.lVornameNameFirma, item.lVornameNameFirma + " || " + item.lName2 + " || " + item.lStrassePostfach + " || " + item.lLandPLZORT));
					}
					if(_lsiteNameFirma.Count() > 0)
					{
						response.lsiteNameFirma = _ListeKunden;
					}
					foreach(var item in PackingListEntity)
					{

						decimal verpckungsGewich = 0;
						if(item.verpackungGewicht == null)
						{
							verpckungsGewich = 0;
						}
						else
						{
							verpckungsGewich = (decimal)item.verpackungGewicht;
						}
						int verpckungsMeng = 0;
						if(item.verpackungsmenge == null)
						{
							verpckungsMeng = 0;
						}
						else
						{
							verpckungsMeng = (int)item.verpackungsmenge;
						}
						item.verpackungGewicht = (item.verpackungsart == null) || item.verpackungsart.ToLower().Contains("gitterbox") || item.verpackungsart.ToLower().Contains("um-") ? item.gewichtArtikel * item.anzahl / 1000 : item.anzahl <= verpckungsMeng ? item.gewichtArtikel * item.anzahl / 1000 + verpckungsGewich / 1000 : ((decimal)item.gewichtArtikel * item.anzahl / 1000) + ((decimal)((decimal)verpckungsGewich / 1000 * (verpckungsMeng == 0 ? 0 : (decimal)item.anzahl / verpckungsMeng)));
						//item.verpackungGewicht = item.verpackungsart == null || item.verpackungsart == "" || item.verpackungsart.ToLower().Contains("gitterbox") || item.verpackungsart.ToLower().Contains("um-") ? item.gewichtArtikel * item.anzahl / 1000 : item.anzahl <= item.verpackungsmenge ? item.gewichtArtikel * item.anzahl / 1000 + item.verpackungGewicht / 1000 : (decimal)item.gewichtArtikel * item.anzahl / 1000 + (decimal)(item.verpackungGewicht / 1000 * (item.anzahl / item.verpackungsmenge));
					}
				}


				return ResponseModel<PackingGlobalChooseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<PackingGlobalChooseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<PackingGlobalChooseModel>.AccessDeniedResponse();
			}

			return ResponseModel<PackingGlobalChooseModel>.SuccessResponse();
		}
	}
}