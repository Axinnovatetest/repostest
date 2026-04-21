using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FAProduktionPlannungEntity
	{
		public string Status { get; set; }
		public string Kunde { get; set; }
		public string Atribut { get; set; }
		public int? FA { get; set; }
		public bool? Prio { get; set; }
		public DateTime? Kundentermin { get; set; }
		public DateTime? Plantermin { get; set; }
		public string Bemerkung1 { get; set; }
		public string Bemerkung2 { get; set; }
		public bool? Sonderfertigung { get; set; }
		public string Bemerkung_CS { get; set; }
		public Decimal? Originalmenge { get; set; }
		public Decimal? Menge_erledigt { get; set; }
		public Decimal? Menge_offen { get; set; }
		public string Sysmo { get; set; }
		public string PSZ_Nummer { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal? FA_Zeit { get; set; }
		public Decimal? FA_Lohn { get; set; }
		public Decimal? man { get; set; }
		public string Index { get; set; }
		public DateTime? Indexdatum { get; set; }
		public bool? Technik { get; set; }

		public FAProduktionPlannungEntity(DataRow dataRow)
		{
			Status = (dataRow["Stav Planovani/Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stav Planovani/Status"]);
			Kunde = (dataRow["Zákaznik/Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zákaznik/Kunde"]);
			Atribut = (dataRow["Atribut"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Atribut"]);
			FA = (dataRow["?islo Zakázky/FA#"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["?islo Zakázky/FA#"]);
			Prio = (dataRow["Prio"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Prio"]);
			Kundentermin = (dataRow["Termin Zákaznika/Kundentermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin Zákaznika/Kundentermin"]);
			Plantermin = (dataRow["Termin Výroba/Plantermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin Výroba/Plantermin"]);
			Bemerkung1 = (dataRow["Komentá? 1/Bemerkung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Komentá? 1/Bemerkung1"]);
			Bemerkung2 = (dataRow["Komentá? 2/Bemerkung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Komentá? 2/Bemerkung2"]);
			Sonderfertigung = (dataRow["Sonderfertigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Sonderfertigung"]);
			Bemerkung_CS = (dataRow["Komentá? ZS/Bemerkung CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Komentá? ZS/Bemerkung CS"]);
			Originalmenge = (dataRow["Original Množství/Originalmenge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Original Množství/Originalmenge"]);
			Menge_erledigt = (dataRow["Vyvezené Množství/Menge erledigt"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Vyvezené Množství/Menge erledigt"]);
			Menge_offen = (dataRow["Ot Množství/Menge offen"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Ot Množství/Menge offen"]);
			Sysmo = (dataRow["?islo Sysmo/Sysmo#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["?islo Sysmo/Sysmo#"]);
			PSZ_Nummer = (dataRow["?islo PSZ/PSZ Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["?islo PSZ/PSZ Nummer"]);
			Freigabestatus = (dataRow["Stav/Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stav/Freigabestatus"]);
			FA_Zeit = (dataRow["?as na Zakázku/FA Zeit"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["?as na Zakázku/FA Zeit"]);
			FA_Lohn = (dataRow["Peníze/FA Lohn"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Peníze/FA Lohn"]);
			man = (dataRow["Vyvezené Množství man"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Vyvezené Množství man"]);
			Index = (dataRow["Index"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index"]);
			Indexdatum = (dataRow["Indexdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Indexdatum"]);
			Technik = (dataRow["Technik"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Technik"]);

		}
	}
}
