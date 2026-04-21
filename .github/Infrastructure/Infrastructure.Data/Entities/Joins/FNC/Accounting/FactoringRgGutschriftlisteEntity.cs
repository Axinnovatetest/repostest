using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class FactoringRgGutschriftlisteEntity
	{
		public int Debitor { get; set; }
		public int Beleg_Nr { get; set; }
		public string Typ { get; set; }
		public DateTime? Datum { get; set; }
		public double Betrag { get; set; }
		public string Wahrung { get; set; }
		public double MwSt_Satz { get; set; }
		public DateTime? Fallig_am { get; set; }
		public int Netto_Laufzeit { get; set; }
		public string Bezugbeleg_Nr { get; set; }
		public int Skontotage_1 { get; set; }
		public int TotalCount { get; set; }
		public double Kondition_1 { get; set; }
		public string Skontotage_2 { get; set; }
		public string Kondition_2 { get; set; }
		public FactoringRgGutschriftlisteEntity(DataRow dataRow)
		{
			//TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			//Debitor = ((string.IsNullOrEmpty(dataRow["Debitor"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Debitor"].ToString())) || dataRow["Debitor"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Debitor"].ToString());
			//Beleg_Nr = ((string.IsNullOrEmpty(dataRow["Beleg-Nr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Beleg-Nr"].ToString())) || dataRow["Beleg-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Beleg-Nr"].ToString());
			//Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"].ToString());
			//Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"].ToString());
			//Wahrung = (dataRow["Währung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Währung"].ToString());
			//MwSt_Satz = ((string.IsNullOrEmpty(dataRow["MwSt-Satz"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["MwSt-Satz"].ToString())) || dataRow["MwSt-Satz"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["MwSt-Satz"].ToString());
			//Fallig_am = (dataRow["Fällig am"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Fällig am"].ToString());
			//Netto_Laufzeit = ((string.IsNullOrEmpty(dataRow["Netto-Laufzeit"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Netto-Laufzeit"].ToString())) || dataRow["Netto-Laufzeit"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Netto-Laufzeit"].ToString());
			//Skontotage_1 = ((string.IsNullOrEmpty(dataRow["Skontotage 1"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Skontotage 1"].ToString())) || dataRow["Skontotage 1"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Skontotage 1"].ToString());
			//Kondition_1 = ((string.IsNullOrEmpty(dataRow["Kondition 1"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Kondition 1"].ToString())) || dataRow["Kondition 1"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Kondition 1"].ToString());

			TotalCount = ConversionHelpers.ConvertToInt32("TotalCount", ref dataRow);
			Debitor = ConversionHelpers.ConvertToInt32("Debitor", ref dataRow);
			Beleg_Nr = ConversionHelpers.ConvertToInt32("Beleg-Nr", ref dataRow);
			Typ = ConversionHelpers.ConvertToString("Typ", ref dataRow);
			Datum = ConversionHelpers.ConvertToDateTime("Datum", ref dataRow);
			Wahrung = ConversionHelpers.ConvertToString("Währung", ref dataRow);
			MwSt_Satz = ConversionHelpers.ConvertToDouble("MwSt-Satz", ref dataRow);
			Fallig_am = ConversionHelpers.ConvertToDateTime("Fällig am", ref dataRow);
			Netto_Laufzeit = ConversionHelpers.ConvertToInt32("Netto-Laufzeit", ref dataRow);
			Skontotage_1 = ConversionHelpers.ConvertToInt32("Skontotage 1", ref dataRow);
			Kondition_1 = ConversionHelpers.ConvertToDouble("Kondition 1", ref dataRow);
			Betrag = ConversionHelpers.ConvertToDouble("Betrag", ref dataRow);


		}
	}
}
