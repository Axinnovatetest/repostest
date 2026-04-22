using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LagerPlantBookingArtikelEntity
	{

		public int artikelNr { get; set; }
		public string? artikelnummer { get; set; }
		public decimal Anzahl { get; set; }
		public int? wereingangId { get; set; }
		public bool? transfered { get; set; }
		public bool? FromRealOrder { get; set; }
		public int Nr { get; set; }
		public decimal? UbertrageneMenge { get; set; }

		public LagerPlantBookingArtikelEntity()
		{

		}
		public LagerPlantBookingArtikelEntity(DataRow dataRow)
		{
			FromRealOrder = dataRow["FromRealOrder"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["FromRealOrder"]);
			transfered = dataRow["transfered"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["transfered"]);
			artikelNr = dataRow["Artikel-Nr"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			artikelnummer = dataRow["Artikelnummer"] == DBNull.Value ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl"]);
			wereingangId = dataRow["wereingangId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["wereingangId"]);
			Nr = dataRow["BestelleNr"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BestelleNr"]);
			UbertrageneMenge = (dataRow["UbertrageneMenge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["UbertrageneMenge"]);
		}
	}
}

