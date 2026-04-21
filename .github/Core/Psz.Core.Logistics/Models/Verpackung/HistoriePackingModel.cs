using System;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class HistoriePackingModel
	{
		public long AngebotNr { get; set; }
		public string artikelnummer { get; set; }
		public decimal menge { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZOrt { get; set; }
		public string versandart { get; set; }
		public DateTime? versanddatum { get; set; }
		public bool versandt { get; set; }
		public bool gepackt { get; set; }
		public bool gedruckt { get; set; }
		public bool gebucht { get; set; }
		public bool vda { get; set; }
		public HistoriePackingModel(Infrastructure.Data.Entities.Joins.Logistics.HistoriePackingEntity PackingEntity)
		{

			if(PackingEntity != null)
			{
				AngebotNr = PackingEntity.AngebotNr;
				artikelnummer = PackingEntity.artikelnummer;
				menge = PackingEntity.menge;
				lVornameNameFirma = PackingEntity.lVornameNameFirma;
				lStrassePostfach = PackingEntity.lStrassePostfach;
				versandart = PackingEntity.versandart;
				versanddatum = PackingEntity.versanddatum;
				versandt = PackingEntity.versandt;
				gepackt = PackingEntity.gepackt;
				gedruckt = PackingEntity.gedruckt;
				gebucht = PackingEntity.gebucht;
				vda = PackingEntity.vda;


			}
		}
	}
}
