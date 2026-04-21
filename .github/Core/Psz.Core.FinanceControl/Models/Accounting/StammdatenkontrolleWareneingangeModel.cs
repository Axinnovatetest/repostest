using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using Psz.Core.FinanceControl.Models.Accounting.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class StammdatenkontrolleWareneingangeModel
{
	public string Lieferantengruppe { get; set; }
	public DateTime? Datum { get; set; }
	public string Artikelnummer { get; set; }
	public double Anzahl { get; set; }
	public string Warengruppe { get; set; }
	public double Gewicht_in_gr { get; set; }
	public string Zolltarif_nr { get; set; }
	public string Ursprungsland { get; set; }
	public string Name1 { get; set; }
	public double Gesamtpreis { get; set; }
	//public int TotalCount { get; set; }
	public StammdatenkontrolleWareneingangeModel(StammdatenkontrolleWareneingangeEntity data)
	{
		Lieferantengruppe = data.Lieferantengruppe;
		Datum = data.Datum;
		Artikelnummer = data.Artikelnummer;
		Anzahl = data.Anzahl;
		Warengruppe = data.Warengruppe;
		Gewicht_in_gr = data.Gewicht_in_gr;
		Zolltarif_nr = data.Zolltarif_nr;
		Ursprungsland = data.Ursprungsland;
		Name1 = data.Name1;
		Gesamtpreis = data.Gesamtpreis;
		//TotalCount = data.TotalCount;
	}
}
public class StammdatenkontrolleWareneingangeRequestModel: IPaginatedRequestModel
{
	[Required]
	public string gruppe { get; set; }
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime fromdate { get; set; } = DateTime.Today.AddMonths(-6);
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime todate { get; set; } = DateTime.Today.AddMonths(+6);
}
