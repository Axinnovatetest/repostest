using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class LieferPlannung_2Model
	{
		public string Vorname_NameFirma { get; set; }
		public int? Jahr { get; set; }
		public int? KW { get; set; }
		public List<LieferPlannung_2DetailsModel> Details { get; set; }
	}
	public class LieferPlannung_2DetailsModel
	{
		public DateTime? Wunschtermin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Menge_Offen { get; set; }
		public int? Bestand { get; set; }
		public Decimal? Gesamtpreis { get; set; }
		public string CSInterneBemerkung { get; set; }
		public string LName2 { get; set; }
		public string LLand_PLZ_Ort { get; set; }
	}
}
