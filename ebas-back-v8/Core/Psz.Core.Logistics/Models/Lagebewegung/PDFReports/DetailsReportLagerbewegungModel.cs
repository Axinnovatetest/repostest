namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class DetailsReportLagerbewegungModel
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		public int artikelNrNach { get; set; }
		public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		public string gebuchtVon { get; set; }
		public int grund { get; set; }
		public string bemerkung { get; set; }
		//------------Pour Bestand vor und nach Transfer
		public decimal bestandLagervonVor { get; set; }
		public decimal bestandLagervonNach { get; set; }
		public decimal bestandLagernachVor { get; set; }
		public decimal bestandLagernachNach { get; set; }
		public string lagerortVon { get; set; }
		public string lagerortNach { get; set; }
		public DetailsReportLagerbewegungModel()
		{

		}
		public DetailsReportLagerbewegungModel(Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity lagerbewegungDetailsEntit)
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
			bemerkung = lagerbewegungDetailsEntit.bemerkung == null ? "" : lagerbewegungDetailsEntit.bemerkung;
		}
		public DetailsReportLagerbewegungModel(Psz.Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel lagerbewegungDetailsEntit)
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
			bemerkung = lagerbewegungDetailsEntit.bemerkung == null ? "" : lagerbewegungDetailsEntit.bemerkung;
			bestandLagervonVor = lagerbewegungDetailsEntit.bestandLagervonVor;
			bestandLagervonNach = lagerbewegungDetailsEntit.bestandLagervonNach;
			bestandLagernachVor = lagerbewegungDetailsEntit.bestandLagernachVor;
			bestandLagernachNach = lagerbewegungDetailsEntit.bestandLagernachNach;
			lagerortVon = lagerbewegungDetailsEntit.lagerortVon;
			lagerortNach = lagerbewegungDetailsEntit.lagerortNach;
		}
	}

	public class DetailsReportPlantBookingLagerbewegungModel
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		public int artikelNrNach { get; set; }
		public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		public string gebuchtVon { get; set; }
		public int grund { get; set; }
		public int WereingangId { get; set; }
		public string bemerkung { get; set; }

		//------------Pour Bestand vor und nach Transfer
		public decimal bestandLagervonVor { get; set; }
		public decimal bestandLagervonNach { get; set; }
		public decimal bestandLagernachVor { get; set; }
		public decimal bestandLagernachNach { get; set; }
		public string lagerortVon { get; set; }
		public string lagerortNach { get; set; }
		public decimal TransferableQuantity { get; set; }
		public decimal TransferbestandNachUmb { get; set; }
		public DetailsReportPlantBookingLagerbewegungModel()
		{

		}
		public DetailsReportPlantBookingLagerbewegungModel(Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsPlantBookingEntity lagerbewegungDetailsEntit)
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
			WereingangId = lagerbewegungDetailsEntit.WereingangId;
			TransferableQuantity = lagerbewegungDetailsEntit.TransferableQuantity;
			bemerkung = lagerbewegungDetailsEntit.bemerkung == null ? "" : lagerbewegungDetailsEntit.bemerkung;
			TransferbestandNachUmb = lagerbewegungDetailsEntit.TransferbestandNachUmb;
		}
		public DetailsReportPlantBookingLagerbewegungModel(Psz.Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsPlantBookingModel lagerbewegungDetailsEntit)
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
			bemerkung = lagerbewegungDetailsEntit.bemerkung == null ? "" : lagerbewegungDetailsEntit.bemerkung;
			bestandLagervonVor = lagerbewegungDetailsEntit.bestandLagervonVor;
			bestandLagervonNach = lagerbewegungDetailsEntit.bestandLagervonNach;
			bestandLagernachVor = lagerbewegungDetailsEntit.bestandLagernachVor;
			bestandLagernachNach = lagerbewegungDetailsEntit.bestandLagernachNach;
			lagerortVon = lagerbewegungDetailsEntit.lagerortVon;
			lagerortNach = lagerbewegungDetailsEntit.lagerortNach;
			TransferableQuantity = lagerbewegungDetailsEntit.TransferableQuantity;
			WereingangId = lagerbewegungDetailsEntit.WereingangId;
			TransferbestandNachUmb = lagerbewegungDetailsEntit.anzahl - anzahlNach;
		}
	}
}
