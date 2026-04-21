using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using Psz.Core.FinanceControl.Models.Accounting.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class AusfuhrModel
{
	public string Vorname_NameFirma { get; set; }
	public double? Zolltarif_nr { get; set; }
	//public int TotalCount { get; set; }
	public double VK_Nettosumme { get; set; }
	public double Gewicht_in_kg { get; set; }
	public string Ursprungsland { get; set; }
	public AusfuhrModel(AusfuhrEntity data)
	{
		Vorname_NameFirma = data.Vorname_NameFirma;
		Zolltarif_nr = data.Zolltarif_nr;
		//TotalCount = data.TotalCount;
		VK_Nettosumme = data.VK_Nettosumme;
		Gewicht_in_kg = data.Gewicht_in_kg;
		Ursprungsland = data.Ursprungsland;
	}
}
public class AusfuhrRequestModel
{
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime fromdate { get; set; } = DateTime.Today.AddMonths(-6);

	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime todate { get; set; } = DateTime.Today.AddMonths(+6);
}
