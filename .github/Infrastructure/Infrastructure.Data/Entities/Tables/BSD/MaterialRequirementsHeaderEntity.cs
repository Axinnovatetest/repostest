using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class MaterialRequirementsHeaderEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal BedarfPO { get; set; }
		public string Bestellung { get; set; }
		public decimal? EK { get; set; }
		public decimal GesamtbedarfOffeneFA360 { get; set; }
		public decimal? Lager { get; set; }
		public string LieferantArtikelnummer { get; set; }
		public string Lieferzeit { get; set; }
		public decimal Min_Lagerbestand { get; set; }
		public string PRIO1_Lieferant { get; set; }
		public decimal SummePO { get; set; }
		public decimal? Verfugbarbestand { get; set; }
		public Single? VPE_Losgroesse { get; set; }
		public int SyncId { get; set; }
		public DateTime? SyncDate { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public string artikelklassifizierung { get; set; }
		public string Description2 { get; set; }

		public MaterialRequirementsHeaderEntity() { }

		public MaterialRequirementsHeaderEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			BedarfPO = Convert.ToDecimal(dataRow["BedarfPO"]);
			Bestellung = (dataRow["Bestellung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellung"]);
			EK = (dataRow["EK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK"]);
			GesamtbedarfOffeneFA360 = Convert.ToDecimal(dataRow["GesamtbedarfOffeneFA360"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lager"]);
			LieferantArtikelnummer = (dataRow["LieferantArtikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LieferantArtikelnummer"]);
			Lieferzeit = (dataRow["Lieferzeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferzeit"]);
			Min_Lagerbestand = Convert.ToDecimal(dataRow["Min_Lagerbestand"]);
			PRIO1_Lieferant = (dataRow["PRIO1_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PRIO1_Lieferant"]);
			SummePO = Convert.ToDecimal(dataRow["SummePO"]);
			Verfugbarbestand = (dataRow["Verfugbarbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verfugbarbestand"]);
			VPE_Losgroesse = (dataRow["VPE_Losgroesse"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["VPE_Losgroesse"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellmenge"]);
			SyncId = Convert.ToInt32(dataRow["SyncId"]);
			SyncDate = (dataRow["SyncDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SyncDate"]);
			artikelklassifizierung = (dataRow["artikelklassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelklassifizierung"]);
			Description2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
		}
	}
}
