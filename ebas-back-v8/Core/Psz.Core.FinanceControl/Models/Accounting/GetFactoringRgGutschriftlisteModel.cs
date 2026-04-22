using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using Psz.Core.FinanceControl.Models.Accounting.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class GetFactoringRgGutschriftlisteModel
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
	//public int TotalCount { get; set; }
	public double Kondition_1 { get; set; }
	public string Skontotage_2 { get; set; }
	public string Kondition_2 { get; set; }
	public GetFactoringRgGutschriftlisteModel(FactoringRgGutschriftlisteEntity data)
	{
		Debitor = data.Debitor;
		Beleg_Nr = data.Beleg_Nr;
		Typ = data.Typ;
		Datum = data.Datum;
		Betrag = data.Betrag;
		Wahrung = data.Wahrung;
		MwSt_Satz = data.MwSt_Satz;
		Fallig_am = data.Fallig_am;
		Netto_Laufzeit = data.Netto_Laufzeit;
		Bezugbeleg_Nr = data.Bezugbeleg_Nr;
		Skontotage_1 = data.Skontotage_1;
		//TotalCount = data.TotalCount;
		Kondition_1 = data.Kondition_1;
	}
}
public class GetFactoringRgGutschriftlisteRequestModel: IPaginatedRequestModel
{
	[Required]
	public string typ { get; set; }
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime fromdate { get; set; } = DateTime.Today.AddMonths(-6);
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime todate { get; set; } = DateTime.Today.AddMonths(+6);
}
