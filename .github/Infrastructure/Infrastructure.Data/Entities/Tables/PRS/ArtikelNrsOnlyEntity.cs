using System;
using System.Data.SqlClient;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class ArtikelNrsOnlyEntity
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public ArtikelNrsOnlyEntity()
		{

		}
		public ArtikelNrsOnlyEntity(SqlDataReader dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			ArtikelNummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
		}
	}
}
