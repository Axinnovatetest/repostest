using System;
using System.Data;
using System.Net.NetworkInformation;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class CapacityEntity
	{
		public int Artikel_Nr { get; set; }
		public decimal? abAnzahl { get; set; }
		public int? abPosCount { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Bestand { get; set; }
		public int? faAnzahl { get; set; }
		public int? faCount { get; set; }
		public int? lpAnzahl { get; set; }
		public int? lpCount { get; set; }
		public int? frcAnzahl { get; set; }
		public int? frcCount { get; set; }
		public decimal? Mindestbestand { get; set; }
		public decimal? Summe { get; set; }

		public CapacityEntity() { }

		public CapacityEntity(DataRow dataRow)
		{
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel_Nr"]);
			abAnzahl = (dataRow["abAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["abAnzahl"]);
			abPosCount = (dataRow["abPosCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["abPosCount"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
			faAnzahl = (dataRow["faAnzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["faAnzahl"]);
			faCount = (dataRow["faCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["faCount"]);
			lpAnzahl = (dataRow["lpAnzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["lpAnzahl"]);
			lpCount = (dataRow["lpCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["lpCount"]);
			frcAnzahl = (dataRow["frcAnzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["frcAnzahl"]);
			frcCount = (dataRow["frcCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["frcCount"]);
			Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
			Summe = (dataRow["Summe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Summe"]);
		}

		public CapacityEntity ShallowClone()
		{
			return new CapacityEntity
			{
				abAnzahl = abAnzahl,
				abPosCount = abPosCount,
				Artikelnummer = Artikelnummer,
				Bestand = Bestand,
				faAnzahl = faAnzahl,
				faCount = faCount,
				lpAnzahl = lpAnzahl,
				lpCount = lpCount,
				frcAnzahl = frcAnzahl,
				frcCount=frcCount,
				Mindestbestand = Mindestbestand,
				Summe = Summe
			};
		}
	}
	public class CapacityABEntity
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }


		public int? AbId { get; set; }
		public string AbBezug { get; set; }
		public string AbNummer { get; set; }
		public string AbCustomer { get; set; }
		public string AbPosition { get; set; }
		public decimal? AbAnzahl { get; set; }

		public int? FaId { get; set; }
		public string FaNummer { get; set; }
		public string FaStatus { get; set; }
		public string FaLager { get; set; }
		public int? FaAnzahl { get; set; }

		public CapacityABEntity() { }

		public CapacityABEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);

			AbId = (dataRow["AbNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AbNr"]);
			AbBezug = (dataRow["AbBezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AbBezug"]);
			AbNummer = (dataRow["AbNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AbNummer"]);
			AbCustomer = (dataRow["AbCustomer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AbCustomer"]);
			AbPosition = (dataRow["AbPosition"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AbPosition"]);
			AbAnzahl = (dataRow["abAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["abAnzahl"]);

			FaId = (dataRow["FaId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaId"]);
			FaNummer = (dataRow["FaNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaNummer"]);
			FaStatus = (dataRow["FaStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaStatus"]);
			FaLager = (dataRow["FaLager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaLager"]);
			FaAnzahl = (dataRow["faAnzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["faAnzahl"]);
		}
	}

	public class CapacityLPEntity
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }


		public int? HeaderId { get; set; }
		public int? LineItemId { get; set; }
		public int? LineItemPlanId { get; set; }
		public string Nummer { get; set; }
		public string Customer { get; set; }
		public string Position { get; set; }
		public decimal? Anzahl { get; set; }
		public bool? IsManual { get; set; }
		public int? PSZCustomernumber { get; set; }

		public CapacityLPEntity() { }

		public CapacityLPEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);

			PSZCustomernumber = (dataRow["PSZCustomernumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PSZCustomernumber"]);
			HeaderId = (dataRow["HeaderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HeaderId"]);
			LineItemId = (dataRow["LineItemId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LineItemId"]);
			LineItemPlanId = (dataRow["LineItemPlanId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LineItemPlanId"]);
			Nummer = (dataRow["Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummer"]);
			Customer = (dataRow["Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			IsManual = (dataRow["IsManual"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsManual"]);

		}
	}
}
