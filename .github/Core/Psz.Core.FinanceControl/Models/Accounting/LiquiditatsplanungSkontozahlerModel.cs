using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using System;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class LiquiditatsplanungSkontozahlerModel
{
	public string Name1 { get; set; }
	public DateTime? Ausliefertermin { get; set; }
	public string Konditionen { get; set; }
	public DateTime? Zahlungseingang { get; set; }
	public double Brutto_inkl_Skonto { get; set; }
	public LiquiditatsplanungSkontozahlerModel(LiquiditatsplanungSkontozahlerEntity data)
	{
		Name1 = data.Name1;
		Ausliefertermin = data.Ausliefertermin;
		Konditionen = data.Konditionen;
		Zahlungseingang = data.Zahlungseingang;
		Brutto_inkl_Skonto = data.Brutto_inkl_Skonto;
	}
}
public class LiquiditatsplanungSkontozahlerRequestModel //: IPaginatedRequestModel
{

}
