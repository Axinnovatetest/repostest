using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RahmenLinkToAbPosEntity
	{
		public int? AngeboteNr { get; set; }
		public int? KundunNr { get; set; }

		public string ArtikelNummer { get; set; }

		public int? Anzahl { get; set; }

		public int? Nr { get; set; }
		public int? NrRA { get; set; }
		public int? Position { get; set; }


		public DateTime? DateDebut { get; set; }
		public DateTime? DateFin { get; set; }
		public RahmenLinkToAbPosEntity(DataRow dataRow)
		{
			AngeboteNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			KundunNr = (dataRow["Kunden-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kunden-Nr"]);
			ArtikelNummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			NrRA = (dataRow["NrRA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NrRA"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);

			DateDebut = (dataRow["GultigAb"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigAb"]);
			DateFin = (dataRow["GultigBis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigBis"]);
		}
	}
}
