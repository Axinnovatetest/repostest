using System;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class LagerbewegungDetailsModel
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string? bezeichnung1 { get; set; }
		public string? einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		public int artikelNrNach { get; set; }
		public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		public string gebuchtVon { get; set; }
		public int grund { get; set; }
		public long? fertigungsnummer { get; set; }
		public string? bemerkung { get; set; }

		//------------Pour Bestand vor und nach Transfer
		public decimal bestandLagervonVor { get; set; }
		public decimal bestandLagervonNach { get; set; }
		public decimal bestandLagernachVor { get; set; }
		public decimal bestandLagernachNach { get; set; }
		public string lagerortVon { get; set; }
		public string lagerortNach { get; set; }
		public bool changed { get; set; }
		public DateTime? datum { get; set; }


		public LagerbewegungDetailsModel()
		{
		}

		public LagerbewegungDetailsModel(Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity lagerbewegungDetailsEntit)
		{
			if(lagerbewegungDetailsEntit == null)
				return;
			id = lagerbewegungDetailsEntit.id;
			idLagerbewegung = lagerbewegungDetailsEntit.idLagerbewegung;
			artikelNr = lagerbewegungDetailsEntit.artikelNr;
			artikelnummer = lagerbewegungDetailsEntit.artikelnummer;
			bezeichnung1 = lagerbewegungDetailsEntit.bezeichnung1;
			einheit = lagerbewegungDetailsEntit.einheit;
			anzahl = lagerbewegungDetailsEntit.anzahl;
			lagerVon = lagerbewegungDetailsEntit.lagerVon;
			artikelNrNach = lagerbewegungDetailsEntit.artikelNrNach;
			artikelnummerNach = lagerbewegungDetailsEntit.artikelnummerNach;
			bezeichnung1Nach = lagerbewegungDetailsEntit.bezeichnung1Nach;
			anzahlNach = lagerbewegungDetailsEntit.anzahlNach;
			lagerNach = lagerbewegungDetailsEntit.lagerNach;
			gebuchtVon = lagerbewegungDetailsEntit.gebuchtVon;
			grund = lagerbewegungDetailsEntit.grund;
			fertigungsnummer = lagerbewegungDetailsEntit.fertigungsnummer;
			bemerkung = lagerbewegungDetailsEntit.bemerkung;
			datum = lagerbewegungDetailsEntit.datum;


		}
	}


	public class LagerbewegungDetailsPlantBookingModel
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string? bezeichnung1 { get; set; }
		public string? einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		public int artikelNrNach { get; set; }
		public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		public string gebuchtVon { get; set; }
		public int grund { get; set; }
		public long? fertigungsnummer { get; set; }
		public string? bemerkung { get; set; }
		public int WereingangId { get; set; }
		public decimal? receivedQuantity { get; set; }



		//------------Pour Bestand vor und nach Transfer
		public decimal bestandLagervonVor { get; set; }
		public decimal bestandLagervonNach { get; set; }
		public decimal bestandLagernachVor { get; set; }
		public decimal bestandLagernachNach { get; set; }
		public string lagerortVon { get; set; }
		public string lagerortNach { get; set; }
		public int WereingangIdNach { get; set; }
		public bool changed { get; set; }
		public DateTime? datum { get; set; }
		public decimal TransferableQuantity { get; set; }
		public bool? FromRealOrder { get; set; }

		public LagerbewegungDetailsPlantBookingModel()
		{
		}

		public LagerbewegungDetailsPlantBookingModel(Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsPlantBookingEntity lagerbewegungDetailsEntit)
		{
			if(lagerbewegungDetailsEntit == null)
				return;
			id = lagerbewegungDetailsEntit.id;
			idLagerbewegung = lagerbewegungDetailsEntit.idLagerbewegung;
			artikelNr = lagerbewegungDetailsEntit.artikelNr;
			artikelnummer = lagerbewegungDetailsEntit.artikelnummer;
			bezeichnung1 = lagerbewegungDetailsEntit.bezeichnung1;
			einheit = lagerbewegungDetailsEntit.einheit;
			anzahl = lagerbewegungDetailsEntit.anzahl;
			lagerVon = lagerbewegungDetailsEntit.lagerVon;
			artikelNrNach = lagerbewegungDetailsEntit.artikelNrNach;
			artikelnummerNach = lagerbewegungDetailsEntit.artikelnummerNach;
			bezeichnung1Nach = lagerbewegungDetailsEntit.bezeichnung1Nach;
			anzahlNach = lagerbewegungDetailsEntit.anzahlNach;
			lagerNach = lagerbewegungDetailsEntit.lagerNach;
			gebuchtVon = lagerbewegungDetailsEntit.gebuchtVon;
			grund = lagerbewegungDetailsEntit.grund;
			fertigungsnummer = lagerbewegungDetailsEntit.fertigungsnummer;
			bemerkung = lagerbewegungDetailsEntit.bemerkung;
			datum = lagerbewegungDetailsEntit.datum;
			WereingangId = lagerbewegungDetailsEntit.WereingangId;
			WereingangIdNach = lagerbewegungDetailsEntit.WereingangIdNach;
			TransferableQuantity = lagerbewegungDetailsEntit.TransferableQuantity;
			receivedQuantity = lagerbewegungDetailsEntit.receivedQuantity;
		}
	}
}
