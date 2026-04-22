using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PSZ_Eingangskontrolle_PrufungenEntity
	{
		public string Artikelnummer { get; set; }
		public string Hilfsmittel { get; set; }
		public int ID { get; set; }
		public string Prufung { get; set; }

		public PSZ_Eingangskontrolle_PrufungenEntity() { }

		public PSZ_Eingangskontrolle_PrufungenEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Hilfsmittel = (dataRow["Hilfsmittel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Hilfsmittel"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Prufung = (dataRow["Prüfung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfung"]);
		}
	}
}

