using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class LagerExtensionEntity
	{
		public int ArtikelNr { get; set; }
		public decimal? Bestand { get; set; }
		public int Id { get; set; }
		public string Index_Kunde { get; set; }
		public int Lagerort_id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }

		public LagerExtensionEntity() { }

		public LagerExtensionEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Index_Kunde = Convert.ToString(dataRow["Index_Kunde"]);
			Lagerort_id = Convert.ToInt32(dataRow["Lagerort_id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
		}
	}
}

