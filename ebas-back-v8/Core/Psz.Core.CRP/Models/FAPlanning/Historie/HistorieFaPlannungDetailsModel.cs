using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning.Historie
{
	public class HistorieFaPlannungDetailsModel
	{
		public DateTime? Ack_Date { get; set; }
		public string Atribut { get; set; }
		public string Bemerkung { get; set; }
		public string Bemerkung_Kommissionierung_AL { get; set; }
		public string Comment_1 { get; set; }
		public string Comment_2 { get; set; }
		public decimal? Costs { get; set; }
		public string CS_Kontakt { get; set; }
		public string Customer { get; set; }
		public int CustomerNumber { get; set; }
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
		public HistorieFaPlannungDetailsModel()
		{

		}
		public HistorieFaPlannungDetailsModel(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity entity)
		{
			Ack_Date = entity.Ack_Date;
			Atribut = entity.Atribut;
			Bemerkung = entity.Bemerkung;
			Bemerkung_Kommissionierung_AL = entity.Bemerkung_Kommissionierung_AL;
			Comment_1 = entity.Comment_1;
			Comment_2 = entity.Comment_2;
			Costs = entity.Costs;
			CS_Kontakt = entity.CS_Kontakt;
			Customer = entity.Customer;
			CustomerNumber = entity.CustomerNumber ?? -1;
			erstelldatum = entity.erstelldatum;
			FA_Number = entity.FA_Number;
			FA_Qty = entity.FA_Qty;
			FA_Druckdatum = entity.FA_Druckdatum;
			Freigabestatus = entity.Freigabestatus;
			Gewerk_Teilweise_Bemerkung = entity.Gewerk_Teilweise_Bemerkung;
			HeaderId = entity.HeaderId;
			Id = entity.Id;
			Kabel_geschnitten = entity.Kabel_geschnitten;
			Kabel_geschnitten_Datum = entity.Kabel_geschnitten_Datum;
			Kommisioniert_komplett = entity.Kommisioniert_komplett;
			Kommisioniert_teilweise = entity.Kommisioniert_teilweise;
			Kontakt = entity.Kontakt;
			KW = entity.KW;
			Losgroesse = entity.Losgroesse;
			Open_Qty = entity.Open_Qty;
			Order_Time = entity.Order_Time;
			PB = entity.PB;
			Planungsstatus = entity.Planungsstatus;
			PN_PSZ = entity.PN_PSZ;
			Shipped_Qty = entity.Shipped_Qty;
			Shipped_Qty_Man = entity.Shipped_Qty_Man;
			Short = entity.Short;
			Status_Intern = entity.Status_Intern;
			Status_TN = entity.Status_TN;
			Technik_Kontakt_TN = entity.Technik_Kontakt_TN;
			Techniker = entity.Techniker;
			Termin_Werk = entity.Termin_Werk;
			Verpackungsart = entity.Verpackungsart;
			Verpackungsmenge = entity.Verpackungsmenge;
			Werk = entity.Werk;
			Wish_Date = entity.Wish_Date;
		}

		public Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity
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
			};
		}
	}
}