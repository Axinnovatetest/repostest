using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRF
{
	public class CAO_DecoupageEntity
	{
		public bool? Aktive { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung { get; set; }
		public int? BOM_version { get; set; }
		public string changee_par { get; set; }
		public int? CP_version { get; set; }
		public string cree_par { get; set; }
		public DateTime? date_changement { get; set; }
		public DateTime? date_creation { get; set; }
		public DateTime? Date_index { get; set; }
		public DateTime? date_validee { get; set; }
		public int ID_Nr { get; set; }
		public string Kunde { get; set; }
		public string Kunden_Index { get; set; }
		public int? Lager { get; set; }
		public bool? Validee { get; set; }
		public string validee_par { get; set; }

		public CAO_DecoupageEntity() { }

		public CAO_DecoupageEntity(DataRow dataRow)
		{
			Aktive = (dataRow["Aktive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Aktive"]);
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			BOM_version = (dataRow["BOM_version"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BOM_version"]);
			changee_par = (dataRow["changee_par"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["changee_par"]);
			CP_version = (dataRow["CP_version"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CP_version"]);
			cree_par = (dataRow["cree_par"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["cree_par"]);
			date_changement = (dataRow["date_changement"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["date_changement"]);
			date_creation = (dataRow["date_creation"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["date_creation"]);
			Date_index = (dataRow["Date_index"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date_index"]);
			date_validee = (dataRow["date_validee"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["date_validee"]);
			ID_Nr = Convert.ToInt32(dataRow["ID_Nr"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			Kunden_Index = (dataRow["Kunden_Index"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunden_Index"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lager"]);
			Validee = (dataRow["Validee"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Validee"]);
			validee_par = (dataRow["validee_par"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["validee_par"]);
		}
	}
}

