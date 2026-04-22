using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
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
		public string WE_VOH_LS { get; set; }
		public string WE_VOH_Nr { get; set; }

		public PrintedDataPlantBookingModel()
		{

		}
		public PrintedDataPlantBookingModel(Infrastructure.Data.Entities.Tables.Logistics.PrintedDataPlantBookingEntity data)
		{
			Aktiv = (data.Aktiv == null ? 0 : data.Aktiv).ToString();
			Artikelnummer = (data.Artikelnummer == null ? "" : data.Artikelnummer).ToString();
			Datum = data.Datum?.ToString("dd/MM/yyyy");
			Gesamtmenge = data.Gesamtmenge.ToString();
			Inspektor = (data.Inspektor == null ? "" : data.Inspektor).ToString().ToUpper();
			Menge = data.Menge.ToString();
			// Lager Testing 
			if(data.LagerortID is int && data.LagerortID > 0)
			{
				if(data.LagerortID == 42 || data.LagerortID == 102 || data.LagerortID == 7)
				{
					LagerortID = "M " + data.LagerortID.ToString();
				}
				else if(data.LagerortID == 26)
				{
					LagerortID = "L " + data.LagerortID.ToString();
				}
			}
			else
			{
				LagerortID = string.Empty;
			}
			// MHD testing 
			if(data.MHDDatum is not null && data.MHDDatum is DateTime)
			{
				MHDDatum = "MHD :" + data.MHDDatum?.ToString("dd/MM/yyyy");
			}
			else
			{
				MHDDatum = string.Empty;
			}
			Nummer_Verpackung = data.Nummer_Verpackung.ToString();
			Resultat = data.Resultat.ToString();
			WE_VOH_LS = data.WE_LS_VOH;
			WE_VOH_Nr = data.WE_VOH_Nr;

		}
	}
}


