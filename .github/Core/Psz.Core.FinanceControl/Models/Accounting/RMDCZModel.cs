using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using Psz.Core.FinanceControl.Models.Accounting.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class RMDCZModel
{
	public string Ursprungsland { get; set; }
	public string Zolltarif_nr { get; set; }
	//public int TotalCount { get; set; }
	public double Gewichte { get; set; }
	public double Warenwert { get; set; }
	public RMDCZModel(RMDCZEntity data)
	{
		Ursprungsland = data.Ursprungsland;
		Zolltarif_nr = data.Zolltarif_nr;
		//TotalCount = data.TotalCount;
		Gewichte = data.Gewichte;
		Warenwert = data.Warenwert;
	}
}
public class RMDCZRequestModel
{
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime fromdate { get; set; } = DateTime.Today.AddMonths(-6);
	[Required]
	[FNCDateValidation(SpanStart = "01-01-1950", SpanEnd = "01-01-3000", ErrorMessage = "Invalid Period Of Time or invalid Datetime values !")]
	public DateTime todate { get; set; } = DateTime.Today.AddMonths(+6);
}
