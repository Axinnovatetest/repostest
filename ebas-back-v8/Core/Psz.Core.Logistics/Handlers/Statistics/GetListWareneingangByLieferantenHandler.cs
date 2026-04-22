using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetListWareneingangByLieferantenHandler: IHandle<Identity.Models.UserModel, ResponseModel<ListWareneingangLieferantHeadersModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime _DateD { get; set; }
		private DateTime _DateF { get; set; }
		private string _name1 { get; set; }





		public GetListWareneingangByLieferantenHandler(Identity.Models.UserModel user, DateTime D1, DateTime D2, string name1)
		{
			_user = user;
			_DateD = D1;
			_DateF = D2;
			_name1 = name1;


		}
		public GetListWareneingangByLieferantenHandler()
		{

		}

		public ResponseModel<ListWareneingangLieferantHeadersModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}




				var response = new ListWareneingangLieferantHeadersModel();
				//----------------Liste Wareneingang By Liferant--------------------------
				var response0 = new List<ListWareneingangDetailsByKundeUndDatumModel>();
				var wareneingangEntities = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetListeWareneingangByLieferant(this._DateD, this._DateF, this._name1);
				if(wareneingangEntities != null && wareneingangEntities.Count > 0)
				{
					var _groupping = wareneingangEntities.GroupBy(d => new { d.mois, d.annee, d.name1Lower })
				   .Select(m => new Groupping
				   {

					   mois = m.Key.mois,
					   annee = m.Key.annee,
					   name1Lower = m.Key.name1Lower,
					   moisEnLettre = m.Key.annee + " " + returnMois(m.Key.mois),

				   }).ToList();
					foreach(var item in _groupping)
					{
						response0.Add(new ListWareneingangDetailsByKundeUndDatumModel
						{
							name1 = item.name1Lower,
							mois = item.mois,
							annee = item.annee,
							moisEnLettre = item.moisEnLettre,
							details = wareneingangEntities.Where(l => l.name1Lower.ToLower() == item.name1Lower && l.mois == item.mois && l.annee == item.annee)?
							.Select(d => new WareneingangLieferantDetailsModel
							{
								liefertermin = d.liefertermin,
								name1 = d.name1,
								mois = d.mois,
								annee = d.annee,
								artikelnummer = d.artikelnummer,
								SummeVonAnzahl = d.SummeVonAnzahl,
								einheit = d.einheit,
								projektNr = d.projektNr,
								typ = d.typ,
							}).ToList()
						});
					}
				}


				response.details = response0;
				response.name1 = this._name1;






				return ResponseModel<ListWareneingangLieferantHeadersModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<ListWareneingangLieferantHeadersModel> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<ListWareneingangLieferantHeadersModel>.AccessDeniedResponse();
			}

			return ResponseModel<ListWareneingangLieferantHeadersModel>.SuccessResponse();
		}
		public class Groupping
		{
			public int mois { get; set; }
			public int annee { get; set; }
			public string moisEnLettre { get; set; }
			public string name1Lower { get; set; }

		}

		public string returnMois(int mois)
		{
			switch(mois)
			{
				case 1:
					return "Januar";
				case 2:
					return "Februar";
				case 3:
					return "März";
				case 4:
					return "April";
				case 5:
					return "Mai";
				case 6:
					return "Juni";
				case 7:
					return "Juli";
				case 8:
					return "August";
				case 9:
					return "September";
				case 10:
					return "Oktober";
				case 11:
					return "November";
				case 12:
					return "Dezember";
				default:
					return "";
			}
		}
	}
}
