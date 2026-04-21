using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetLieferPlannung_2Handler: IHandle<Identity.Models.UserModel, ResponseModel<List<LieferPlannung_2Model>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLieferPlannung_2Handler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<LieferPlannung_2Model>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<LieferPlannung_2Model>();
				var lieferPlannungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannung(_data);
				if(lieferPlannungEntity != null && lieferPlannungEntity.Count > 0)
				{
					var _groupping = lieferPlannungEntity.GroupBy(d => new { d.Vorname_NameFirma, d.Jahr, d.KW })
				   .Select(m => new Groupping
				   {
					   Vorname_NameFirma = m.Key.Vorname_NameFirma,
					   Jahr = m.Key.Jahr,
					   KW = m.Key.KW
				   }).ToList();
					foreach(var item in _groupping)
					{
						response.Add(new LieferPlannung_2Model
						{
							Vorname_NameFirma = item.Vorname_NameFirma,
							Jahr = item.Jahr,
							KW = item.KW,
							Details = lieferPlannungEntity.Where(l => l.Vorname_NameFirma == item.Vorname_NameFirma && l.Jahr == item.Jahr && l.KW == item.KW)?
							.Select(d => new LieferPlannung_2DetailsModel
							{
								Angebot_Nr = d.Angebot_Nr,
								Artikelnummer = d.Artikelnummer,
								Bestand = d.Bestand,
								Bezeichnung_1 = d.Bezeichnung_1,
								Gesamtpreis = d.Gesamtpreis,
								Liefertermin = d.Liefertermin,
								Menge_Offen = d.Menge_Offen,
								Wunschtermin = d.Wunschtermin,
								CSInterneBemerkung = d.CSInterneBemerkung,
								LName2 = d.LName2,
								LLand_PLZ_Ort = d.LLand_PLZ_Ort,

							}).ToList()
						});
					}
				}

				return ResponseModel<List<LieferPlannung_2Model>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<LieferPlannung_2Model>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<LieferPlannung_2Model>>.AccessDeniedResponse();
			}

			return ResponseModel<List<LieferPlannung_2Model>>.SuccessResponse();
		}

		public class Groupping
		{
			public string Vorname_NameFirma { get; set; }
			public int? Jahr { get; set; }
			public int? KW { get; set; }
		}
	}
}
