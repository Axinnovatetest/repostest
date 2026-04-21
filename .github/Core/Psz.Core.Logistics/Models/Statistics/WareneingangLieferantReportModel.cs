using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class WareneingangLieferantReportModel
	{
		public List<HeaderName> ListeName1 { get; set; }
		public List<ListWareneingangRapportDetailsByKundeUndDatumModel> grouping { get; set; }

		public List<WareneingangLieferantRapportDetailsModel> Details { get; set; }
	}

	public class HeaderName
	{
		public string name1 { get; set; }
		public HeaderName()
		{

		}
		public HeaderName(string name1)
		{
			this.name1 = name1;
		}
	}
	public class ListWareneingangRapportDetailsByKundeUndDatumModel
	{
		public string name1 { get; set; }
		public string name1Lower { get; set; }
		public int mois { get; set; }
		public int annee { get; set; }
		public string moisEnLettre { get; set; }
		public List<WareneingangLieferantRapportDetailsModel> details { get; set; }

		public ListWareneingangRapportDetailsByKundeUndDatumModel()
		{

		}
		public ListWareneingangRapportDetailsByKundeUndDatumModel(string name1Lower, int mois, int annee, string moisEnLettre, List<WareneingangLieferantRapportDetailsModel> details)
		{
			this.name1Lower = name1Lower;
			this.mois = mois;
			this.annee = annee;
			this.moisEnLettre = moisEnLettre;
		}
	}

	public class WareneingangLieferantRapportDetailsModel
	{
		public long projektNr { get; set; }
		public string typ { get; set; }
		public string artikelnummer { get; set; }
		public decimal SummeVonAnzahl { get; set; }
		public string einheit { get; set; }
		public string name1 { get; set; }
		public string name1Lower { get { return name1.ToLower(); } }
		public string liefertermin { get; set; }
		public int mois { get; set; }
		public int annee { get; set; }
		public WareneingangLieferantRapportDetailsModel()
		{

		}
		public WareneingangLieferantRapportDetailsModel(Infrastructure.Data.Entities.Joins.Logistics.WareneingangLieferantDetailsEntity wareneingangtEntity)
		{
			if(wareneingangtEntity == null)
				return;
			projektNr = wareneingangtEntity.projektNr;
			typ = wareneingangtEntity.typ;
			artikelnummer = wareneingangtEntity.artikelnummer;
			SummeVonAnzahl = wareneingangtEntity.SummeVonAnzahl;
			einheit = wareneingangtEntity.einheit;
			name1 = wareneingangtEntity.name1;
			liefertermin = wareneingangtEntity.liefertermin != null ? wareneingangtEntity.liefertermin.Value.ToString("dd-MM-yyyy") : "";
			mois = wareneingangtEntity.mois;
			annee = wareneingangtEntity.annee;



		}
	}
}
