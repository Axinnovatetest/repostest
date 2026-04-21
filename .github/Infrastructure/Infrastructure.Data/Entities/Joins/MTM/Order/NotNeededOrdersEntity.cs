using System;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class NotNeededOrdersEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public int TotlaCount { get; set; }
		public bool? ProjectPurchase { get; set; }

		public NotNeededOrdersEntity(System.Data.DataRow datarow)
		{
			ArtikelNr = Convert.ToInt32(datarow["ArtikelNr"].ToString());
			Artikelnummer = (datarow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["Artikelnummer"]);
			TotlaCount = Convert.ToInt32(datarow["TotlaCount"].ToString());
			ProjectPurchase = (datarow["ProjectPurchase"] == System.DBNull.Value) ? false : Convert.ToBoolean(datarow["ProjectPurchase"]);
		}
	}
	public class NotNeededOrdersArticleEntity
	{
		public decimal Anzahl { get; set; }
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Week { get; set; }
		public NotNeededOrdersArticleEntity(System.Data.DataRow datarow)
		{
			ArtikelNr = Convert.ToInt32(datarow["ArtikelNr"].ToString());
			Week = (datarow["Week"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["Week"]);
			Anzahl = (datarow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(datarow["Anzahl"]);
			Artikelnummer = (datarow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["Artikelnummer"]);
		}
	}

	public class NotNeededOrdersAllEntity
	{
		public decimal Anzahl { get; set; }
		public decimal Total { get; set; }
		public string Week { get; set; }
		public NotNeededOrdersAllEntity(System.Data.DataRow datarow)
		{
			Total = Convert.ToDecimal(datarow["Total"].ToString());
			Week = (datarow["Week"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["Week"]);
			Anzahl = (datarow["anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(datarow["anzahl"]);
		}
	}

	public class NotNeededOrderArticleDetailsEntity
	{
		public int Nr { get; set; }
		public string Lieferant { get; set; }
		public decimal Total { get; set; }
		public decimal Anzahl { get; set; }
		public string BestellungNr { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int TotlaCount { get; set; }

		public NotNeededOrderArticleDetailsEntity(System.Data.DataRow datarow)
		{
			Nr = Convert.ToInt32(datarow["Nr"].ToString());
			Total = Convert.ToDecimal(datarow["Total"].ToString());
			TotlaCount = Convert.ToInt32(datarow["TotlaCount"].ToString());
			Anzahl = (datarow["anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(datarow["anzahl"]);
			Total = (datarow["Total"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(datarow["Total"]);
			Lieferant = (datarow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["Lieferant"]);
			Bestatigter_Termin = (datarow["Bestätigter_Termin"] == System.DBNull.Value) ? null : Convert.ToDateTime(datarow["Bestätigter_Termin"]);
			Liefertermin = (datarow["Liefertermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(datarow["Liefertermin"]);
			BestellungNr = (datarow["BestellungNr"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["BestellungNr"]);
		}
	}


	public class NotNeededOrderDetailsAllEntity
	{
		public string Lieferant { get; set; }
		public decimal Total { get; set; }
		public decimal Anzahl { get; set; }
		public string BestellungNr { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public int TotlaCount { get; set; }

		public NotNeededOrderDetailsAllEntity(System.Data.DataRow datarow)
		{
			TotlaCount = Convert.ToInt32(datarow["TotlaCount"].ToString());
			Anzahl = (datarow["anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(datarow["anzahl"]);
			Total = (datarow["Total"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(datarow["Total"]);
			Lieferant = (datarow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["Lieferant"]);
			Bestatigter_Termin = (datarow["Bestätigter_Termin"] == System.DBNull.Value) ? null : Convert.ToDateTime(datarow["Bestätigter_Termin"]);
			Wunschtermin = (datarow["Wünschtermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(datarow["Wünschtermin"]);
			BestellungNr = (datarow["BestellungNr"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["BestellungNr"]);
		}
	}
}
