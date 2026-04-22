using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class BomLog
	{
		public DateTime? Aenderungsdatum { get; set; }//update date time
		public string Alter_menge { get; set; }
		public string Bearbeiter { get; set; }//update user
		public string FG_Artikelnummer { get; set; }
		public int ID { get; set; }
		public string Neuer_menge { get; set; }
		public string Status { get; set; }//change
		public string Stück_Artikelnummer_Aktuell { get; set; }
		public string Stück_Artikelnummer_Voränderung { get; set; }

		public BomLog()
		{

		}
		public BomLog(Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity logEntity)
		{
			Aenderungsdatum = logEntity.Aenderungsdatum;
			Alter_menge = logEntity.Alter_menge;
			Bearbeiter = logEntity.Bearbeiter;
			FG_Artikelnummer = logEntity.FG_Artikelnummer;
			ID = logEntity.ID;
			Neuer_menge = logEntity.Neuer_menge;
			Status = logEntity.Status;
			Stück_Artikelnummer_Aktuell = logEntity.Stück_Artikelnummer_Aktuell;
			Stück_Artikelnummer_Voränderung = logEntity.Stück_Artikelnummer_Voränderung;

		}
	}
}
