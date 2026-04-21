using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class ZahlungskonditionenLieferantenEntity
	{
		public string Name1 { get; set; }
		public int KonditionszuordnungstabelleNr { get; set; }
		public int adressenNr { get; set; }
		public string PLZ_Strabe { get; set; }
		public string Ort { get; set; }
		public string Land { get; set; }
		public int Lieferantennummer { get; set; }
		public int TotalCount { get; set; }
		public string Text { get; set; }
		public ZahlungskonditionenLieferantenEntity(DataRow dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			KonditionszuordnungstabelleNr = (dataRow["KonditionszuordnungstabelleNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["KonditionszuordnungstabelleNr"].ToString());
			adressenNr = (dataRow["adressenNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["adressenNr"].ToString());
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"].ToString());
			PLZ_Strabe = (dataRow["PLZ_Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ_Straße"].ToString());
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"].ToString());
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"].ToString());
			Lieferantennummer = ((string.IsNullOrEmpty(dataRow["Lieferantennummer"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Lieferantennummer"].ToString())) || dataRow["Lieferantennummer"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lieferantennummer"].ToString());
			Text = (dataRow["Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text"].ToString());
		}
		public ZahlungskonditionenLieferantenEntity()
		{

		}
	}
}
