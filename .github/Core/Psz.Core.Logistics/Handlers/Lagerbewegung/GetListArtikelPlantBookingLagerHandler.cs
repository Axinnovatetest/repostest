using Infrastructure.Data.Access.Tables;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetListArtikelPlantBookingLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<ListLagerArtikelPlantBooking>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _lagerID { get; set; }

		public GetListArtikelPlantBookingLagerHandler(Identity.Models.UserModel user, int lagerID)
		{
			this._user = user;
			this._lagerID = lagerID;
		}
		public ResponseModel<ListLagerArtikelPlantBooking> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new ListLagerArtikelPlantBooking();
				var listepositions = Infrastructure.Data.Access.Joins.Logistics.LagerArtikelAccess.GetListFilteredArtikelLagerPlantBooking(this._lagerID);
				//var listepositions = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.GetListFilteredArtikelLagerPlantBooking(this._lagerID);
				//if(listepositions is not null && listepositions.Count>0)
				//{
				//	var articles = ArtikelAccess.GetForPlantBooking(listepositions.Select(x => x.artikelNr).ToList());
				//	response.listeArtikel = listepositions.Select(x => new LagerArtikelPlantBookingArtikelModel(x, articles.Where(y => y.Artikel_Nr == x.artikelNr).FirstOrDefault().Artikelnummer)).ToList();
				//}
				//else
				//{
				//	response.listeArtikel = new List<LagerArtikelPlantBookingArtikelModel>();
				//}
				if(listepositions != null && listepositions.Count() > 0)
				{

					response.listeArtikel = listepositions.Select(k => new LagerArtikelPlantBookingArtikelModel(k)).ToList();

				}

				return ResponseModel<ListLagerArtikelPlantBooking>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ListLagerArtikelPlantBooking> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ListLagerArtikelPlantBooking>.AccessDeniedResponse();
			}

			return ResponseModel<ListLagerArtikelPlantBooking>.SuccessResponse();
		}
	}
}
