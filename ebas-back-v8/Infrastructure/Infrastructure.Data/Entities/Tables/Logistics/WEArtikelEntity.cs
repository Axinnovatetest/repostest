namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class WEArtikelEntity
	{
		public bool MHD { get; set; }
		public int weId { get; set; }
		public int position { get; set; }
		public string bezeichnung1 { get; set; }
		public string artikelnummer { get; set; }
		public int anzahl { get; set; }
		public int startAnzahl { get; set; }
		public string Einheit { get; set; }
		public DateTime? liefertermin { get; set; }
		public WEArtikelEntity()
		{

		}
		public WEArtikelEntity(DataRow dataRow)
		{
			MHD = ((dataRow["MHD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MHD"])) ?? false;
			weId = Convert.ToInt32(dataRow["WE_ID"]);
			position = Convert.ToInt32(dataRow["Position"]);
			bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			anzahl = Convert.ToInt32(dataRow["Anzahl"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			startAnzahl = Convert.ToInt32(dataRow["Start Anzahl"]);
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);

		}
	}
	public class TransferWEArtikelEntity
	{
		public bool MHD { get; set; }
		public int weId { get; set; }
		public int ID { get; set; }
		public int position { get; set; }
		public int lagerNach { get; set; }
		public int lagerVon { get; set; }
		public string? GebuchtVon { get; set; }
		public string bezeichnung1 { get; set; }
		public string artikelnummer { get; set; }
		public int anzahl { get; set; }
		public int startAnzahl { get; set; }
		public string Einheit { get; set; }
		public DateTime? liefertermin { get; set; }
		public TransferWEArtikelEntity()
		{

		}
		public TransferWEArtikelEntity(DataRow dataRow)
		{
			MHD = ((dataRow["MHD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MHD"])) ?? false;
			weId = Convert.ToInt32(dataRow["WE_ID"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			position = Convert.ToInt32(dataRow["Position"]);
			lagerNach = Convert.ToInt32(dataRow["lagerNach"]);
			lagerVon = Convert.ToInt32(dataRow["lagerVon"]);
			bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			anzahl = Convert.ToInt32(dataRow["Anzahl"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			startAnzahl = Convert.ToInt32(dataRow["Start Anzahl"]);
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			GebuchtVon = (dataRow["GebuchtVon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["GebuchtVon"]);

		}
	}

}
