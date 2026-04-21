namespace Psz.Core.FinanceControl.Models.Accounting;

public class LieferantenGruppeModel
{
	public string Lieferantengruppe { get; set; }
	public LieferantenGruppeModel(Infrastructure.Data.Entities.Joins.FNC.Accounting.LieferantenGruppeEntity data)
	{
		Lieferantengruppe = data.Lieferantengruppe;
	}
}
