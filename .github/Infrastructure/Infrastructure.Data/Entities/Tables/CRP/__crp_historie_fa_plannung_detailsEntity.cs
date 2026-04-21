using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class __crp_historie_fa_plannung_detailsEntity
	{
		public int FAId { get; set; }
		public int ArtikelNr { get; set; }
		public DateTime? Ack_Date { get; set; }
		public string Atribut { get; set; }
		public string Bemerkung { get; set; }
		public string Bemerkung_Kommissionierung_AL { get; set; }
		public string Comment_1 { get; set; }
		public string Comment_2 { get; set; }
		public decimal? Costs { get; set; }
		public string CS_Kontakt { get; set; }
		public string Customer { get; set; }
		public int? CustomerNumber { get; set; }
		public DateTime? erstelldatum { get; set; }
		public int? FA_Number { get; set; }
		public int? FA_Qty { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public string Freigabestatus { get; set; }
		public string Gewerk_Teilweise_Bemerkung { get; set; }
		public int? HeaderId { get; set; }
		public int Id { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public DateTime? Kabel_geschnitten_Datum { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public string Kontakt { get; set; }
		public int? KW { get; set; }
		public decimal? Losgroesse { get; set; }
		public int? Open_Qty { get; set; }
		public decimal? Order_Time { get; set; }
		public string PB { get; set; }
		public string Planungsstatus { get; set; }
		public string PN_PSZ { get; set; }
		public int? Shipped_Qty { get; set; }
		public int? Shipped_Qty_Man { get; set; }
		public string Short { get; set; }
		public string Status_Intern { get; set; }
		public string Status_TN { get; set; }
		public string Technik_Kontakt_TN { get; set; }
		public string Techniker { get; set; }
		public DateTime? Termin_Werk { get; set; }
		public string Verpackungsart { get; set; }
		public int? Verpackungsmenge { get; set; }
		public int? Werk { get; set; }
		public DateTime? Wish_Date { get; set; }
		public DateTime? Datum { get; set; }

		public __crp_historie_fa_plannung_detailsEntity() { }

		public __crp_historie_fa_plannung_detailsEntity(DataRow dataRow, bool forDetails = false)
		{
			if(forDetails)
			{
				FAId = Convert.ToInt32(dataRow["ID"]);
				ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			}
			Ack_Date = (dataRow["Ack Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ack Date"]);
			Atribut = (dataRow["Atribut"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Atribut"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bemerkung_Kommissionierung_AL = (dataRow["Bemerkung_Kommissionierung_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Kommissionierung_AL"]);
			Comment_1 = (dataRow["Comment 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment 1"]);
			Comment_2 = (dataRow["Comment 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment 2"]);
			Costs = (dataRow["Costs"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Costs"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			Customer = (dataRow["Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			erstelldatum = (dataRow["erstelldatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["erstelldatum"]);
			FA_Number = (dataRow["FA Number"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA Number"]);
			FA_Qty = (dataRow["FA Qty"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA Qty"]);
			FA_Druckdatum = (dataRow["FA_Druckdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Druckdatum"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Gewerk_Teilweise_Bemerkung = (dataRow["Gewerk_Teilweise_Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk_Teilweise_Bemerkung"]);
			HeaderId = (dataRow["HeaderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HeaderId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
			Kabel_geschnitten_Datum = (dataRow["Kabel_geschnitten_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kabel_geschnitten_Datum"]);
			Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
			Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
			Kontakt = (dataRow["Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Losgroesse"]);
			Open_Qty = (dataRow["Open Qty"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Open Qty"]);
			Order_Time = (dataRow["Order Time"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Order Time"]);
			PB = (dataRow["PB"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["PB"]);
			Planungsstatus = (dataRow["Planungsstatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Planungsstatus"]);
			PN_PSZ = (dataRow["PN PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PN PSZ"]);
			Shipped_Qty = (dataRow["Shipped Qty"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Shipped Qty"]);
			Shipped_Qty_Man = (dataRow["Shipped Qty Man"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Shipped Qty Man"]);
			Short = (dataRow["Short"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Short"]);
			Status_Intern = (dataRow["Status Intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status Intern"]);
			Status_TN = (dataRow["Status TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status TN"]);
			Technik_Kontakt_TN = (dataRow["Technik Kontakt TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Technik Kontakt TN"]);
			Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
			Termin_Werk = (dataRow["Termin Werk"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin Werk"]);
			Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
			Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
			Werk = (dataRow["Werk"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Werk"]);
			Wish_Date = (dataRow["Wish Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wish Date"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
		}

		public __crp_historie_fa_plannung_detailsEntity ShallowClone()
		{
			return new __crp_historie_fa_plannung_detailsEntity
			{
				Ack_Date = Ack_Date,
				Atribut = Atribut,
				Bemerkung = Bemerkung,
				Bemerkung_Kommissionierung_AL = Bemerkung_Kommissionierung_AL,
				Comment_1 = Comment_1,
				Comment_2 = Comment_2,
				Costs = Costs,
				CS_Kontakt = CS_Kontakt,
				Customer = Customer,
				CustomerNumber = CustomerNumber,
				erstelldatum = erstelldatum,
				FA_Number = FA_Number,
				FA_Qty = FA_Qty,
				FA_Druckdatum = FA_Druckdatum,
				Freigabestatus = Freigabestatus,
				Gewerk_Teilweise_Bemerkung = Gewerk_Teilweise_Bemerkung,
				HeaderId = HeaderId,
				Id = Id,
				Kabel_geschnitten = Kabel_geschnitten,
				Kabel_geschnitten_Datum = Kabel_geschnitten_Datum,
				Kommisioniert_komplett = Kommisioniert_komplett,
				Kommisioniert_teilweise = Kommisioniert_teilweise,
				Kontakt = Kontakt,
				KW = KW,
				Losgroesse = Losgroesse,
				Open_Qty = Open_Qty,
				Order_Time = Order_Time,
				PB = PB,
				Planungsstatus = Planungsstatus,
				PN_PSZ = PN_PSZ,
				Shipped_Qty = Shipped_Qty,
				Shipped_Qty_Man = Shipped_Qty_Man,
				Short = Short,
				Status_Intern = Status_Intern,
				Status_TN = Status_TN,
				Technik_Kontakt_TN = Technik_Kontakt_TN,
				Techniker = Techniker,
				Termin_Werk = Termin_Werk,
				Verpackungsart = Verpackungsart,
				Verpackungsmenge = Verpackungsmenge,
				Werk = Werk,
				Wish_Date = Wish_Date,
				Datum = Datum
			};
		}
	}
}

