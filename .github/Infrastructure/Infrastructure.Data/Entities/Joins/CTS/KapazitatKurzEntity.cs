using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class KapazitatKurzEntity
	{
		public string Kunde { get; set; }
		public int? FA { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public DateTime? TerminProd { get; set; }
		public int? Auftragsmenge { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunden { get; set; }
		public Decimal? Auftragszeit_h { get; set; }
		public Decimal? UmsatzCZ { get; set; }
		public Decimal? AnzahlMA { get; set; }
		public int? Lagerort_id { get; set; }
		public Decimal? Stundensatz { get; set; }

		public KapazitatKurzEntity(DataRow dataRow)
		{
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			FA = (dataRow["FA#"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA#"]);
			Wunschtermin = (dataRow["Wunschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin"]);
			TerminProd = (dataRow["TerminProd"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["TerminProd"]);
			Auftragsmenge = (dataRow["Auftragsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Auftragsmenge"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Kunden = (dataRow["Kunden#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunden#"]);
			Auftragszeit_h = (dataRow["Auftragszeit(h)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Auftragszeit(h)"]);
			UmsatzCZ = (dataRow["UmsatzCZ"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["UmsatzCZ"]);
			AnzahlMA = (dataRow["AnzahlMA"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["AnzahlMA"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
		}
	}
}
