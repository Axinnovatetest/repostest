using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Reporting.Models.LGT
{
	public class PrintedDataPlantBookingModel
	{
		public string Aktiv { get; set; }

		public string Artikelnummer { get; set; }

		public string Datum { get; set; }

		public string Menge { get; set; }
		public string Gesamtmenge { get; set; }
		public string Inspektor { get; set; }

		public string LagerortID { get; set; }

		public string MHDDatum { get; set; }
		public string Nummer_Verpackung { get; set; }

		public string Resultat { get; set; }

		public PrintedDataPlantBookingModel()
		{

		}
		public PrintedDataPlantBookingModel(Infrastructure.Data.Entities.Tables.Logistics.PrintedDataPlantBookingEntity data)
		{
			Aktiv = (data.Aktiv == null ? 0 : data.Aktiv).ToString();
			Artikelnummer = data.Artikelnummer.ToString();
			Datum = data.Datum.ToString();
			Gesamtmenge = data.Gesamtmenge.ToString();
			Inspektor = data.Inspektor.ToString();
			LagerortID = data.LagerortID.ToString();
			Menge = data.Menge.ToString();

			if(MHDDatum is not null)
			{
				MHDDatum = data.MHDDatum.ToString();
			}
			Nummer_Verpackung = data.Nummer_Verpackung.ToString();
			Resultat = data.Resultat.ToString();

		}
	}
}
