using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using Psz.Core.FinanceControl.Models.Accounting.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class LiquiditatsplanungOffeneMaterialbestellungenModel
{
	//public int TotalCount { get; set; }
	public string Benutzer { get; set; }
	public DateTime? Bestatigter_Termin { get; set; }
	public int Lieferantennr { get; set; }
	public string Lieferant { get; set; }
	public int Bestellung_Nr { get; set; }
	public double Anzahl { get; set; }
	public double Mindestbestellmenge { get; set; }
	public double Verpackungseinheit { get; set; }
	public string Bezeichnung_1 { get; set; }
	public string Artikelnummer { get; set; }
	public string Bestellnummer { get; set; }
	public decimal Einzelpreis { get; set; }
	public double Gesamtpreis { get; set; }
	public DateTime? Anlieferung { get; set; }
	public int Zahlungsziel_Netto { get; set; }
	public DateTime? Falligkeit { get; set; }
	public string Produktionsstatte { get; set; }
	public string Mandant { get; set; }
	public int Bearbeiter { get; set; }
	public DateTime? Belegdatum { get; set; }
	public DateTime? Wunschtermin { get; set; }
	public string Bemerkung_Pos { get; set; }
	public bool Standardlieferant { get; set; }
	public LiquiditatsplanungOffeneMaterialbestellungenModel(LiquiditatsplanungOffeneMaterialbestellungenEntity data)
	{
		//TotalCount = data.TotalCount;
		Benutzer = data.Benutzer ?? string.Empty;
		Bestatigter_Termin = data.Bestätigter_Termin;
		Lieferantennr = data.Lieferantennr;
		Lieferant = data.Lieferant;
		Bestellung_Nr = data.Bestellung_Nr;
		Anzahl = data.Anzahl;
		Mindestbestellmenge = data.Mindestbestellmenge;
		Verpackungseinheit = data.Verpackungseinheit;
		Bezeichnung_1 = data.Bezeichnung_1;
		Artikelnummer = data.Artikelnummer;
		Bestellnummer = data.Bestellnummer;
		Einzelpreis = data.Einzelpreis;
		Gesamtpreis = data.Gesamtpreis;
		Anlieferung = data.Anlieferung;
		Zahlungsziel_Netto = data.Zahlungsziel_Netto;
		Falligkeit = data.Falligkeit;
		Produktionsstatte = data.Produktionsstatte;
		Mandant = data.Mandant;
		Bearbeiter = data.Bearbeiter;
		Belegdatum = data.Belegdatum;
		Wunschtermin = data.Wunschtermin;
		Bemerkung_Pos = data.Bemerkung_Pos;
		Standardlieferant = data.Standardlieferant;
	}
}
public class LiquiditatsplanungOffeneMaterialbestellungenRequestModel: IPaginatedRequestModel
{
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime fromdate { get; set; } = DateTime.Today.AddMonths(-6);
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime todate { get; set; } = DateTime.Today.AddMonths(+6);
}
