using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class StatisticsEntity
	{
		public class StatCustomerEntity
		{
			public string CustomerName { get; set; }
			public int? Count { get; set; }
			public StatCustomerEntity()
			{

			}
			public StatCustomerEntity(DataRow dataRow)
			{
				CustomerName = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatPayEntity
		{
			public string Pay { get; set; }
			public int? Count { get; set; }
			public StatPayEntity()
			{

			}
			public StatPayEntity(DataRow dataRow)
			{
				Pay = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatTypEntity
		{
			public string Type { get; set; }
			public int? Count { get; set; }
			public StatTypEntity()
			{

			}
			public StatTypEntity(DataRow dataRow)
			{
				Type = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatEdiEntity
		{
			public string Type { get; set; }
			public int? Count { get; set; }
			public int? Edi { get; set; }
			public StatEdiEntity()
			{

			}
			public StatEdiEntity(DataRow dataRow)
			{
				Type = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
				Edi = (dataRow["EDI_Order_Neu"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EDI_Order_Neu"]);
			}
		}
		public class StatLagerEntity
		{
			public string LagerName { get; set; }
			public int? Count { get; set; }
			public int? LagerNum { get; set; }
			public StatLagerEntity()
			{

			}
			public StatLagerEntity(DataRow dataRow)
			{
				LagerName = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
				LagerNum = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			}
		}
		public class StatLagerZuEntity
		{
			public string LagerName { get; set; }
			public int? Count { get; set; }
			public int? LagerNum { get; set; }
			public StatLagerZuEntity()
			{

			}
			public StatLagerZuEntity(DataRow dataRow)
			{
				LagerName = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
				LagerNum = (dataRow["Lagerort_id zubuchen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id zubuchen"]);
			}
		}
		public class StatKenzEntity
		{
			public string TypeKennzeichen { get; set; }
			public int? Count { get; set; }
			public StatKenzEntity()
			{

			}
			public StatKenzEntity(DataRow dataRow)
			{
				TypeKennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatGestEntity
		{
			public string Getstartet { get; set; }
			public int? Count { get; set; }
			public StatGestEntity()
			{

			}
			public StatGestEntity(DataRow dataRow)
			{
				Getstartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FA_Gestartet"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatKomCoEntity
		{
			public string KommComp { get; set; }
			public int? Count { get; set; }
			public StatKomCoEntity()
			{

			}
			public StatKomCoEntity(DataRow dataRow)
			{
				KommComp = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kommisioniert_komplett"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatKomTrEntity
		{
			public string KommTreil { get; set; }
			public int? Count { get; set; }
			public StatKomTrEntity()
			{

			}
			public StatKomTrEntity(DataRow dataRow)
			{
				KommTreil = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kommisioniert_teilweise"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatTechEntity
		{
			public string Technik { get; set; }
			public int? Count { get; set; }
			public StatTechEntity()
			{

			}
			public StatTechEntity(DataRow dataRow)
			{
				Technik = (dataRow["Technik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Technik"]);
				Count = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatARTEntity
		{
			public string ArtikelNum { get; set; }
			public int? ArtikelNr { get; set; }
			public int? NbFA { get; set; }
			public string LagerName { get; set; }
			public StatARTEntity()
			{

			}
			public StatARTEntity(DataRow dataRow)
			{
				ArtikelNum = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				NbFA = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
				LagerName = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			}
		}
		public class StatCustomerFAEntity
		{
			public string ArtikelNum { get; set; }
			public string Kunde { get; set; }

			public int? ArtikelNr { get; set; }
			public int? NbFA { get; set; }
			public StatCustomerFAEntity()
			{

			}
			public StatCustomerFAEntity(DataRow dataRow)
			{
				ArtikelNum = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);

				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);

				NbFA = (dataRow["NB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NB"]);
			}
		}
		public class StatTimeFAEntity
		{
			public int? Time { get; set; }
			public int? Fertigungsnummer { get; set; }

			public StatTimeFAEntity()
			{

			}
			public StatTimeFAEntity(DataRow dataRow)
			{
				Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);

				Time = (dataRow["_time"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["_time"]);
			}
		}
		public class StatTimeAREntity
		{
			public decimal? Time { get; set; }
			public string ArtikelNum { get; set; }
			public StatTimeAREntity()
			{

			}
			public StatTimeAREntity(DataRow dataRow)
			{
				ArtikelNum = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);

				Time = (dataRow["_time"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["_time"]);
			}
		}
		public class StatTimeByLagerEntity
		{
			public decimal? Time { get; set; }
			public string LagerName { get; set; }
			public StatTimeByLagerEntity()
			{

			}
			public StatTimeByLagerEntity(DataRow dataRow)
			{
				LagerName = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
				Time = (dataRow["_time"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["_time"]);
			}
		}
		//public class StatTypeEntity
		//{
		//    public string Type { get; set; }

		//    public StatTypeEntity()
		//    {

		//    }
		//    public StatTypeEntity(DataRow dataRow)
		//    {
		//        Type = (dataRow["typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["typ"]);
		//    }
		//}
	}
}
