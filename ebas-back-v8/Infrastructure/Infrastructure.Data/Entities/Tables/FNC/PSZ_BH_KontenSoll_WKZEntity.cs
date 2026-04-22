using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class PSZ_BH_KontenSoll_WKZEntity
	{
		public string Beschreibung { get; set; }
		public int ID { get; set; }
		public string Niederlassung { get; set; }
		public string Sollkto { get; set; }
		public string Warengruppe { get; set; }

		public PSZ_BH_KontenSoll_WKZEntity() { }

		public PSZ_BH_KontenSoll_WKZEntity(DataRow dataRow)
		{
			Beschreibung = (dataRow["Beschreibung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Beschreibung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Niederlassung = (dataRow["Niederlassung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Niederlassung"]);
			Sollkto = (dataRow["Sollkto"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sollkto"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
		}
	}
}

