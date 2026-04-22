using System.ComponentModel;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class PrintedDataPlantBookingEntity
	{
		public int? Aktiv { get; set; }

		public string Artikelnummer { get; set; }

		public DateTime? Datum { get; set; }

		public decimal? Menge { get; set; }
		public int? Gesamtmenge { get; set; }
		public string? Inspektor { get; set; }

		public int? LagerortID { get; set; }

		public DateTime? MHDDatum { get; set; }
		public int Nummer_Verpackung { get; set; }

		public string? Resultat { get; set; }
		public string WE_LS_VOH { get; set; }
		public string WE_VOH_Nr { get; set; }

		public PrintedDataPlantBookingEntity() { }


		public PrintedDataPlantBookingEntity(DataRow dataRow)
		{
			Aktiv = (dataRow["Aktiv"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aktiv"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Gesamtmenge = (dataRow["Gesamtmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Gesamtmenge"]);
			Inspektor = (dataRow["Inspektor"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Inspektor"]);
			LagerortID = (dataRow["LagerortID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerortID"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			MHDDatum = (dataRow["MHDDatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MHDDatum"]);
			Nummer_Verpackung = Convert.ToInt32(dataRow["Nummer Verpackung"]);
			Resultat = (dataRow["Resultat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Resultat"]);
			WE_LS_VOH = (dataRow["WE_LS_VOH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WE_LS_VOH"]);
			WE_VOH_Nr = (dataRow["WE_VOH_Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WE_VOH_Nr"]);
		}

		public PrintedDataPlantBookingEntity(DataRow dataRow, LagerAccessEnum choice)
		{
			Aktiv = (dataRow["Aktiv"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aktiv"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Gesamtmenge = (dataRow["Gesamtmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Gesamtmenge"]);
			Inspektor = (dataRow["Inspektor"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Inspektor"]);
			LagerortID = (dataRow["LagerortID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerortID"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			MHDDatum = (dataRow["MHDDatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MHDDatum"]);
			Resultat = (dataRow["Resultat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Resultat"]);
			WE_LS_VOH = (dataRow["WE_LS_VOH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WE_LS_VOH"]);
			WE_VOH_Nr = (dataRow["WE_VOH_Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WE_VOH_Nr"]);

			if(choice == LagerAccessEnum.TN || choice == LagerAccessEnum.BETN || choice == LagerAccessEnum.WS || choice == LagerAccessEnum.GZTN)
			{
				Nummer_Verpackung = Convert.ToInt32(dataRow["Nummer Verpackung"]);
			}
			else
			{
				Nummer_Verpackung = Convert.ToInt32(dataRow["Verpackungsnr"]);
			}
		}

	}


	public enum LagerAccessEnum
	{
		[Description("TN")]
		TN = 7,
		[Description("BE_TN")]
		BETN = 60,
		[Description("GZ-TN")]
		GZTN = 102,
		[Description("WS")]
		WS = 42,
		[Description("Albanien")]
		Albanien = 26,
		[Description("Eigenfertigung")]
		Eigenfertigung = 6,
		[Description("Fertigung D")]
		FertigungD = 15
	}

}
