using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using Psz.Core.FinanceControl.Models.Accounting.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class EinfuhrModel
{
	public string Lieferantengruppe { get; set; }
	public string Name1 { get; set; }
	public string Zolltarif_nr { get; set; }
	//public int TotalCount { get; set; }
	public string Ursprungsland { get; set; }
	public double Nettopreis { get; set; }
	public double Gewicht_in_kg { get; set; }
	public EinfuhrModel(EinfuhrEntity data)
	{
		Lieferantengruppe = data.Lieferantengruppe;
		Name1 = data.Name1;
		Zolltarif_nr = data.Zolltarif_nr;
		//TotalCount = data.TotalCount;
		Ursprungsland = data.Ursprungsland;
		Nettopreis = data.Nettopreis;
		Gewicht_in_kg = data.Gewicht_in_kg;
	}
}
public class EinfuhrRequestModel
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
