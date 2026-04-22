using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class EntnahmeWertHandler: IHandle<Identity.Models.UserModel, ResponseModel<EntnahmeWertKompletModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime _DateD { get; set; }
		private DateTime _DateF { get; set; }
		private int _lager1 { get; set; }
		private int _lager2 { get; set; }
		private int _type { get; set; }




		public EntnahmeWertHandler(DateTime D1, DateTime D2, int L1, int Type, Identity.Models.UserModel user)
		{
			_user = user;
			_DateD = D1;
			_DateF = D2;
			_lager1 = L1;
			_type = Type;


		}
		public EntnahmeWertHandler()
		{

		}

		public ResponseModel<EntnahmeWertKompletModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager_Id == this._lager1).ToList();
				this._lager2 = 0;
				if(ModelLGT != null && ModelLGT.Count() > 0)
				{
					this._lager2 = ModelLGT[0].Lager_P_Id;
				}


				var response = new EntnahmeWertKompletModel();
				//----------------Emtnahme Wert with EK--------------------------
				var response0 = new List<EntnahmeWertTreeModel>();
				var entnahmeWertWithEKEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetEntnahmeWertWithEK(this._DateD, this._DateF, this._lager1, this._lager2, this._type, "");
				if(entnahmeWertWithEKEntities != null && entnahmeWertWithEKEntities.Count > 0)
				{
					var _groupping = entnahmeWertWithEKEntities.GroupBy(d => new { d.datum })
				   .Select(m => new Groupping
				   {
					   datum = m.Key.datum,
					   totalLigne = (from num in entnahmeWertWithEKEntities
									 where num.datum == m.Key.datum
									 select num.datum).Count(),
					   gesmtPreis = Math.Round((from num in entnahmeWertWithEKEntities
												where num.datum == m.Key.datum
												select num.kosten).Sum(), 2),
					   percentPreis = Math.Round((from num in entnahmeWertWithEKEntities
												  where num.datum == m.Key.datum
												  select num.kosten).Sum() /
									(from num in entnahmeWertWithEKEntities
									 select num.kosten).Sum() * 100, 2)
				   }).ToList();
					foreach(var item in _groupping)
					{
						response0.Add(new EntnahmeWertTreeModel
						{
							datum = item.datum,
							totalLigne = item.totalLigne,
							gesmtPreis = item.gesmtPreis,
							percentPreis = item.percentPreis,
							details = entnahmeWertWithEKEntities.Where(l => l.datum == item.datum)?
							.Select(d => new EntnahmeWertTreeDetailsModel
							{
								datum = d.datum,
								artikelNr = d.artikelNr,
								artikelnummer = d.artikelnummer,
								bezeichnung1 = d.bezeichnung1,
								anzahl = d.anzahl,
								zuFA = d.zuFA,
								grund = d.grund,
								kosten = Math.Round(d.kosten, 2)
							}).ToList()
						});
					}
				}


				response.entnahmeWetWithEK = response0;
				response.gesamtEntnahmeWetWithEK = Math.Round((from num in entnahmeWertWithEKEntities
															   select num.kosten).Sum(), 2);

				//----------------Emtnahme Wert without EK--------------------------
				var response1 = new List<EntnahmeWertTreeModel>();
				var entnahmeWertWithoutEKEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetEntnahmeWertWithoutEK(_DateD, _DateF, this._lager1, this._lager2, this._type, "");
				if(entnahmeWertWithoutEKEntities != null && entnahmeWertWithoutEKEntities.Count > 0)
				{
					var _groupping = entnahmeWertWithoutEKEntities.GroupBy(d => new { d.datum })
				   .Select(m => new Groupping
				   {
					   datum = m.Key.datum,
					   totalLigne = (from num in entnahmeWertWithoutEKEntities
									 where num.datum == m.Key.datum
									 select num.datum).Count(),
					   gesmtPreis = Math.Round((from num in entnahmeWertWithoutEKEntities
												where num.datum == m.Key.datum
												select num.kosten).Sum(), 2),
					   percentPreis = Math.Round((from num in entnahmeWertWithoutEKEntities
												  where num.datum == m.Key.datum
												  select num.kosten).Sum() /
									(from num in entnahmeWertWithoutEKEntities
									 select num.kosten).Sum() * 100, 2)
				   }).ToList();
					foreach(var item in _groupping)
					{
						response1.Add(new EntnahmeWertTreeModel
						{
							datum = item.datum,
							totalLigne = item.totalLigne,
							gesmtPreis = item.gesmtPreis,
							percentPreis = item.percentPreis,
							details = entnahmeWertWithoutEKEntities.Where(l => l.datum == item.datum)?
							.Select(d => new EntnahmeWertTreeDetailsModel
							{
								datum = d.datum,
								artikelNr = d.artikelNr,
								artikelnummer = d.artikelnummer,
								bezeichnung1 = d.bezeichnung1,
								anzahl = d.anzahl,
								zuFA = d.zuFA,
								grund = d.grund,
								kosten = Math.Round(d.kosten)
							}).ToList()
						});
					}
				}


				response.entnahmeWetWithoutEK = response1;
				response.gesamtEntnahmeWetWithoutEK = Math.Round((from num in entnahmeWertWithoutEKEntities
																  select num.kosten).Sum(), 2);




				return ResponseModel<EntnahmeWertKompletModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<EntnahmeWertKompletModel> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<EntnahmeWertKompletModel>.AccessDeniedResponse();
			}

			return ResponseModel<EntnahmeWertKompletModel>.SuccessResponse();
		}
		public class Groupping
		{
			public DateTime? datum { get; set; }
			public int totalLigne { get; set; }
			public decimal gesmtPreis { get; set; }
			public decimal percentPreis { get; set; }
		}
	}
}

