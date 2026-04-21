using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class CreatePlantBookingRequestModel
	{

		public int LagerId { get; set; }
		public string? ArtikelNummer { get; set; }
		public int? NrBestellteArtikel { get; set; }
		public bool Choice { get; set; }
		public DateTime? MHDDatum { get; set; }
		public int? LagerbewegungenId { get; set; }
		public int? LagerNach { get; set; }
		public int? AnzahlNach { get; set; }
		public bool ChoiceTransfer { get; set; }
		public string Inspector { get; set; }
		public string Remarks { get; set; }

		public CreatePlantBookingRequestModel()
		{
		}

		public CreatePlantBookingRequestModel(int lagerId, string artikelNummer, int nrBestellteArtikel, int lagerbewegungenId ,bool choice,DateTime mHDDatum,int lagerNach)
		{
			LagerId = lagerId;
			ArtikelNummer = artikelNummer;
			NrBestellteArtikel = nrBestellteArtikel;
			Choice = choice;
			MHDDatum = mHDDatum;
			LagerbewegungenId = lagerbewegungenId;
			LagerNach = lagerNach;
		}
	}

}
