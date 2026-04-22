using System;
using System.Data;
using System.Diagnostics;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FALagersPlannungEntity
	{
		public int? Werk { get; set; }
		public string Planungsstatus { get; set; }
		public string Customer { get; set; }
		public string CS_Kontakt { get; set; }
		public string PB { get; set; }
		public string Atribut { get; set; }
		public string Short { get; set; }
		public int? FA_Number { get; set; }
		public bool? Prio { get; set; }
		public string Comment_1 { get; set; }
		public string Comment_2 { get; set; }
		public Decimal? FA_Qty { get; set; }
		public Decimal? Shipped_Qty { get; set; }
		public Decimal? Open_Qty { get; set; }
		public string PN_PSZ { get; set; }
		public string Status_TN { get; set; }
		public Decimal? Order_Time { get; set; }
		public Decimal? Costs { get; set; }
		public Decimal? Shipped_Qty_Man { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public DateTime? Kabel_geschnitten_Datum { get; set; }
		public DateTime? Termin_Werk { get; set; }
		public DateTime? Ack_Date { get; set; }
		public int? KW { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public string Freigabestatus { get; set; }
		public DateTime? Wish_Date { get; set; }
		public string Bemerkung { get; set; }
		public string Gewerk_Teilweise_Bemerkung { get; set; }
		public string Verpackungsart { get; set; }
		public Decimal? Verpackungsmenge { get; set; }
		public Decimal? Losgroesse { get; set; }
		public string Techniker { get; set; }
		public string Technik_Kontakt { get; set; }
		public string Technik_Kontakt_TN { get; set; }
		public string Status_Intern { get; set; }
		public DateTime? erstelldatum { get; set; }
		public string Bemerkung_Kommissionierung_AL { get; set; }
		public string FertigungType { get; set; }
		public string ProjectNr { get; set; }
		public bool? ProdBiggerThanMOQ { get; set; }

		public FALagersPlannungEntity(DataRow dataRow)
		{
			Werk = (dataRow["Werk"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Werk"]);
			Planungsstatus = (dataRow["Planungsstatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Planungsstatus"]);
			Customer = (dataRow["Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			PB = (dataRow["PB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PB"]);
			Atribut = (dataRow["Atribut"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Atribut"]);
			Short = (dataRow["Short"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Short"]);
			FA_Number = (dataRow["FA Number"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA Number"]);
			Prio = (dataRow["Prio"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Prio"]);
			Comment_1 = (dataRow["Comment 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment 1"]);
			Comment_2 = (dataRow["Comment 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment 2"]);
			FA_Qty = (dataRow["FA Qty"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["FA Qty"]);
			Shipped_Qty = (dataRow["Shipped Qty"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Shipped Qty"]);
			Open_Qty = (dataRow["Open Qty"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Open Qty"]);
			PN_PSZ = (dataRow["PN PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PN PSZ"]);
			Status_TN = (dataRow["Status TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status TN"]);
			Order_Time = (dataRow["Order Time"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Order Time"]);
			Costs = (dataRow["Costs"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Costs"]);
			Shipped_Qty_Man = (dataRow["Shipped Qty Man"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Shipped Qty Man"]);
			Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
			Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
			Kabel_geschnitten_Datum = (dataRow["Kabel_geschnitten_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kabel_geschnitten_Datum"]);
			Termin_Werk = (dataRow["Termin Werk"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin Werk"]);
			Ack_Date = (dataRow["Ack Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ack Date"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			FA_Druckdatum = (dataRow["FA_Druckdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Druckdatum"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Wish_Date = (dataRow["Wish Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wish Date"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Gewerk_Teilweise_Bemerkung = (dataRow["Gewerk_Teilweise_Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk_Teilweise_Bemerkung"]);
			Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
			Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Verpackungsmenge"]);
			Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Losgroesse"]);
			Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
			Technik_Kontakt = (dataRow["Technik Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Technik Kontakt"]);
			Technik_Kontakt_TN = (dataRow["Technik Kontakt TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Technik Kontakt TN"]);
			Status_Intern = (dataRow["Status Intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status Intern"]);
			erstelldatum = (dataRow["erstelldatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["erstelldatum"]);
			Bemerkung_Kommissionierung_AL = (dataRow["Bemerkung_Kommissionierung_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Kommissionierung_AL"]);
			FertigungType = (dataRow["FertigungType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FertigungType"]);
			ProjectNr = (dataRow["ProjectNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectNr"]);
			ProdBiggerThanMOQ = (dataRow["ProdBiggerThanMOQ"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProdBiggerThanMOQ"]);
		}
	}
}
