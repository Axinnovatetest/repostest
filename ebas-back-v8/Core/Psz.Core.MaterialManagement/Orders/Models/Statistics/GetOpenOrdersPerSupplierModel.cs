using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{

	public class GetOpenOrdersPerSupplierResponseModel
	{
		/*
		15	Lagerort_id --
		16	Bestellung-Nr --
		17	Rahmenbestellung --
		18	Bestellmenge --
		19	Offen --
		20	Liefertermin --
		21	Bestätigter_Termin --
		22	AB-Nr_Lieferant --
		23	Einzelpreis
		Nr
		*/
		public int Lagerort_id { get; set; } //
		public int Bestellung_Nr { get; set; } //
		public bool Rahmenbestellung { get; set; } //
		public string AB_Nr_Lieferant { get; set; } //
		public int Nr { get; set; }//
		public double Bestellmenge { get; set; } //
		public double Offen { get; set; } //
		public double Einzelpreis { get; set; } //
		public DateTime? Liefertermin { get; set; } //
		public DateTime? Bestatigter_Termin { get; set; } //
		public int Lieferanten_Nr { get; set; }
		public int TotalCount { get; set; }
		public GetOpenOrdersPerSupplierResponseModel(Dispows120DetailsOffenBestellungenEntity data)
		{
			Lagerort_id = data.Lagerort_id;
			Bestellung_Nr = data.Bestellung_Nr;
			Rahmenbestellung = data.Rahmenbestellung;
			AB_Nr_Lieferant = data.AB_Nr_Lieferant;
			Nr = data.Nr;
			Bestellmenge = data.Bestellmenge;
			Offen = data.Offen;
			Einzelpreis = data.Einzelpreis;
			Liefertermin = data.Liefertermin;
			Bestatigter_Termin = data.Bestätigter_Termin;
			Lieferanten_Nr = data.Lieferanten_Nr;
			TotalCount = data.TotalCount;
		}
	}
	public class GetOpenOrdersPerSupplierRequestModel: IPaginatedRequestModel
	{
		[Required]
		public int ArtikelNr { get; set; }
		[Required]
		public int Lieferanten_Nr { get; set; }

		[Required]
		public int Dispo { get; set; }
	}

}
