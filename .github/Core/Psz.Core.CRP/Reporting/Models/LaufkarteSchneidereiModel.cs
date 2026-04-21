using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class LaufkarteSchneidereiModel
	{
		public string Klassifizierung { get; set; }
		public string Gewerk { get; set; }
		public string Artikelnummer { get; set; }
		public string Anzahl { get; set; }
		public string Termin_Bestätigt1 { get; set; }
		public int Fertigungsnummer { get; set; }
		public string Bezeichnung { get; set; }
		public string FGArtikelBZ1 { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public LaufkarteSchneidereiModel(Infrastructure.Data.Entities.Joins.FAPlannung.LaufkarteSchneidereiEntity entity)
		{
			Klassifizierung = entity.Klassifizierung;
			Gewerk = entity.Gewerk;
			Artikelnummer = entity.Artikelnummer;
			Anzahl = entity.Anzahl.HasValue ? entity.Anzahl.Value.ToString() : "";
			Termin_Bestätigt1 = entity.Termin_Bestätigt1.HasValue ? entity.Termin_Bestätigt1.Value.ToString("dd/MM/yyyy") : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Bezeichnung = entity.Bezeichnung;
			FGArtikelBZ1 = entity.FGArtikelBZ1;
			Artikelfamilie_Kunde = entity.Artikelfamilie_Kunde;
		}
	}
}