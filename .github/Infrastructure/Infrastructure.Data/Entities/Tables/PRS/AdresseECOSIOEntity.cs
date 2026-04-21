using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class AdresseECOSIOEntity
	{
		public int? AnlieferLagerort { get; set; }
		public string Bezeichnung { get; set; }
		public long? DUNSNummer { get; set; }
		public string Firma { get; set; }
		public int Id { get; set; }
		public string LOrt { get; set; }
		public string LPLZ { get; set; }
		public string LStrasse { get; set; }
		public string ROrt { get; set; }
		public string RPLZ { get; set; }
		public string RStrasse { get; set; }
		public int? Werksnummer { get; set; }

		public AdresseECOSIOEntity() { }

		public AdresseECOSIOEntity(DataRow dataRow)
		{
			AnlieferLagerort = (dataRow["AnlieferLagerort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AnlieferLagerort"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			DUNSNummer = (dataRow["DUNSNummer"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["DUNSNummer"]);
			Firma = (dataRow["Firma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Firma"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LOrt = (dataRow["LOrt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LOrt"]);
			LPLZ = (dataRow["LPLZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LPLZ"]);
			LStrasse = (dataRow["LStrasse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStrasse"]);
			ROrt = (dataRow["ROrt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ROrt"]);
			RPLZ = (dataRow["RPLZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RPLZ"]);
			RStrasse = (dataRow["RStrasse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RStrasse"]);
			Werksnummer = (dataRow["Werksnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Werksnummer"]);
		}
	}
}

