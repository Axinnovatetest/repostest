using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class EntnahmeWertTreeModel
	{
		public DateTime? datum { get; set; }
		public int totalLigne { get; set; }
		public decimal gesmtPreis { get; set; }
		public decimal percentPreis { get; set; }
		public List<EntnahmeWertTreeDetailsModel> details { get; set; }
	}
	public class EntnahmeWertTreeDetailsModel
	{
		public DateTime? datum { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public decimal anzahl { get; set; }
		public long zuFA { get; set; }
		public int grund { get; set; }
		public decimal kosten { get; set; }
		public string bemerkung { get; set; }
		public EntnahmeWertTreeDetailsModel()
		{

		}
		public EntnahmeWertTreeDetailsModel(Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity entnahmeWertEntity)
		{
			if(entnahmeWertEntity == null)
				return;
			datum = entnahmeWertEntity.datum;
			artikelNr = entnahmeWertEntity.artikelNr;
			artikelnummer = entnahmeWertEntity.artikelnummer;
			bezeichnung1 = entnahmeWertEntity.bezeichnung1;
			anzahl = entnahmeWertEntity.anzahl;
			zuFA = entnahmeWertEntity.zuFA;
			grund = entnahmeWertEntity.grund;
			kosten = entnahmeWertEntity.kosten;
			bemerkung = entnahmeWertEntity.bemerkung;


		}
	}
}
