using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class PSZ_PV_ListeSearchModel
	{
		public List<PSZ_PV_ListeModel> PSZ_PV_ListeModel { get; set; } = new List<PSZ_PV_ListeModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}

	public class PSZ_PV_ListeModel
	{
		public PSZ_PV_ListeModel(Infrastructure.Data.Entities.Joins.Logistics.PSZ_PV_ListeEntity PSZ_PV_ListeEntity)
		{
			if(PSZ_PV_ListeEntity == null)
			{
				return;
			}
			ArtikelNr = PSZ_PV_ListeEntity.ArtikelNr;
			Artikelnummer = PSZ_PV_ListeEntity.Artikelnummer;
			Bezeichnung1 = PSZ_PV_ListeEntity.Bezeichnung1;
			Bestand = PSZ_PV_ListeEntity.Bestand;
			Einheit = PSZ_PV_ListeEntity.Einheit;
			Lagerort = PSZ_PV_ListeEntity.Lagerort;
			EK = PSZ_PV_ListeEntity.EK;
			EK_Summe = PSZ_PV_ListeEntity.EK_Summe;
			Gewicht = PSZ_PV_ListeEntity.Gewicht;
			Gesamtgewicht = PSZ_PV_ListeEntity.Gesamtgewicht;
			Zolltarif_nr = PSZ_PV_ListeEntity.Zolltarif_nr;
			Ursprungsland = PSZ_PV_ListeEntity.Ursprungsland;
			LieferantenNr = PSZ_PV_ListeEntity.LieferantenNr;
			Name1 = PSZ_PV_ListeEntity.Name1;
			BestellNr = PSZ_PV_ListeEntity.BestellNr;
			BezeichnungAL = PSZ_PV_ListeEntity.BezeichnungAL;
			Praferenz = PSZ_PV_ListeEntity.Praferenz;
			Sendungsnummer = PSZ_PV_ListeEntity.Sendungsnummer;
			totalRows = PSZ_PV_ListeEntity.totalRows;
		}
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Bestand { get; set; }
		public string Einheit { get; set; }
		public string Lagerort { get; set; }
		public decimal EK { get; set; }
		public decimal EK_Summe { get; set; }
		public decimal Gewicht { get; set; }
		public decimal Gesamtgewicht { get; set; }
		public string Zolltarif_nr { get; set; }
		public string Ursprungsland { get; set; }
		public int LieferantenNr { get; set; }
		public string Name1 { get; set; }
		public string BestellNr { get; set; }
		public string BezeichnungAL { get; set; }
		public string Praferenz { get; set; }
		public int Sendungsnummer { get; set; }
		public int totalRows { get; set; }
	}
}
