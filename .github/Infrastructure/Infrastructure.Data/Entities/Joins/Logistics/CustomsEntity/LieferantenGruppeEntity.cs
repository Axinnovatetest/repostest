using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity;

public class LieferantenGruppeEntity
{
	public string Lieferantengruppe { get; set; }
	public LieferantenGruppeEntity(DataRow dataRow)
	{
		Lieferantengruppe = (dataRow["Lieferantengruppe"] == System.DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Lieferantengruppe"]);
	}

}

