using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class AdressenExtensionEntity
	{
		public int AdressenNr { get; set; }
		public string Duns { get; set; }
		public int Id { get; set; }
		public DateTime? LastUpdate { get; set; }

		public AdressenExtensionEntity() { }

		public AdressenExtensionEntity(DataRow dataRow)
		{
			AdressenNr = Convert.ToInt32(dataRow["AdressenNr"]);
			Duns = (dataRow["Duns"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Duns"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdate = (dataRow["LastUpdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdate"]);
		}
	}
}

