using System;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Entity
	{
		public PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Entity(System.Data.DataRow dataRow)
		{
			PSZ = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestand"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Lagerort_id"]);
		}
		public string PSZ { get; set; }
		public decimal Bestand { get; set; }
		public string Lagerort { get; set; }
		public int? Lagerort_id { get; set; }
	}
}
