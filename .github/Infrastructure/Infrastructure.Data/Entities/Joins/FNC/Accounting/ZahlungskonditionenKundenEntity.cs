using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class ZahlungskonditionenKundenEntity
	{
		public string Name1 { get; set; }
		public int adressenNr { get; set; }
		public int KonditionszuordnungstabelleNr { get; set; }
		public string PLZ_Strabe { get; set; }
		public string Ort { get; set; }
		public string Land { get; set; }
		public int Kundennummer { get; set; }
		public int TotalCount { get; set; }
		public string Text { get; set; }
		public ZahlungskonditionenKundenEntity(DataRow dataRow)
		{
			TotalCount = ((string.IsNullOrEmpty(dataRow["TotalCount"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["TotalCount"].ToString())) || dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			adressenNr = ((string.IsNullOrEmpty(dataRow["adressenNr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["adressenNr"].ToString())) || dataRow["adressenNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["adressenNr"].ToString());
			KonditionszuordnungstabelleNr = ((string.IsNullOrEmpty(dataRow["KonditionszuordnungstabelleNr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["KonditionszuordnungstabelleNr"].ToString())) || dataRow["KonditionszuordnungstabelleNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["KonditionszuordnungstabelleNr"].ToString());
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"].ToString());
			PLZ_Strabe = (dataRow["PLZ_Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ_Straße"].ToString());
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"].ToString());
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"].ToString());
			Kundennummer = ((string.IsNullOrEmpty(dataRow["Kundennummer"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Kundennummer"].ToString())) || dataRow["Kundennummer"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Kundennummer"].ToString());
			Text = (dataRow["Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text"].ToString());
		}
		public ZahlungskonditionenKundenEntity()
		{

		}
	}
}

