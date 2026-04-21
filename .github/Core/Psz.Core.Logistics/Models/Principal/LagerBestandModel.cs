using System;

namespace Psz.Core.Logistics.Models.Principal
{
	public class LagerBestandModel
	{
		public int nombreTotal { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public string lagerort { get; set; }
		public decimal bestand { get; set; }
		public bool cCID { get; set; }
		public DateTime? cCID_Datum { get; set; }

		public LagerBestandModel(Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity lagerBestandEntity)
		{

			if(lagerBestandEntity != null)
			{
				nombreTotal = lagerBestandEntity.nombreTotal;
				artikelNr = lagerBestandEntity.artikelNr;
				artikelnummer = lagerBestandEntity.artikelnummer;
				bezeichnung1 = lagerBestandEntity.bezeichnung1;
				bezeichnung2 = lagerBestandEntity.bezeichnung2;
				lagerort = lagerBestandEntity.lagerort;
				bestand = lagerBestandEntity.bestand;
				cCID = lagerBestandEntity.cCID;
				cCID_Datum = lagerBestandEntity.cCID_Datum;


			}
		}

	}
}
