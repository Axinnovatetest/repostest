using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class ReasonChangeCommitteeEntity
	{
		public string Typ { get; set; }
		public DateTime? Datum { get; set; }
		public decimal? Anzahl { get; set; }
		public int? LagerId { get; set; }
		public int Id { get; set; }
		public int ArtikelNr { get; set; }

		public string Grund { get; set; }
		public string Articlenummer { get; set; }

		public ReasonChangeCommitteeEntity() { }
		public ReasonChangeCommitteeEntity(DataRow dataRow)
		{
			LagerId = (dataRow["Lager_von"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lager_von"]);
			Grund = (dataRow["Grund"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Grund"]);
			Typ = (dataRow["Typ"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Typ"]);
			Articlenummer = (dataRow["Artikelnummer"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Artikelnummer"]);
			Datum = (dataRow["Datum"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Anzahl = (dataRow["Anzahl"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Id = (dataRow["Id"] == DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Id"]);
			ArtikelNr = (dataRow["Artikel-Nr"] == DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
		}
	}
}