namespace Psz.Core.Logistics.Models.Statistics.CustomsModel;

public class LieferantenGruppeModel
{
	public string Lieferantengruppe { get; set; }
	public LieferantenGruppeModel(Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.LieferantenGruppeEntity data)
	{
		Lieferantengruppe = data.Lieferantengruppe;
	}
}
