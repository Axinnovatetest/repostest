using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class ArtikelKalkulatorischeKostenEntity
	{
		public int? Artikel_nr { get; set; }
		public decimal? Betrag { get; set; }
		public int ID { get; set; }
		public string Kostenart { get; set; }

		public ArtikelKalkulatorischeKostenEntity() { }
		public ArtikelKalkulatorischeKostenEntity(DataRow dataRow)
		{
			Artikel_nr = (dataRow["artikel-nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["artikel-nr"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kostenart = (dataRow["Kostenart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kostenart"]);
		}
	}
}

