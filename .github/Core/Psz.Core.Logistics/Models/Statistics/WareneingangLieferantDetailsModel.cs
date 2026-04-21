using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class WareneingangLieferantDetailsModel
	{
		public long projektNr { get; set; }
		public string typ { get; set; }
		public string artikelnummer { get; set; }
		public decimal SummeVonAnzahl { get; set; }
		public string einheit { get; set; }
		public string name1 { get; set; }
		public string name1Lower { get { return name1.ToLower(); } }
		public DateTime? liefertermin { get; set; }
		public int mois { get; set; }
		public int annee { get; set; }
		public WareneingangLieferantDetailsModel()
		{

		}
		public WareneingangLieferantDetailsModel(Infrastructure.Data.Entities.Joins.Logistics.WareneingangLieferantDetailsEntity wareneingangtEntity)
		{
			if(wareneingangtEntity == null)
				return;
			projektNr = wareneingangtEntity.projektNr;
			typ = wareneingangtEntity.typ;
			artikelnummer = wareneingangtEntity.artikelnummer;
			SummeVonAnzahl = wareneingangtEntity.SummeVonAnzahl;
			einheit = wareneingangtEntity.einheit;
			name1 = wareneingangtEntity.name1;
			liefertermin = wareneingangtEntity.liefertermin;
			mois = wareneingangtEntity.mois;
			annee = wareneingangtEntity.annee;



		}
	}
}
