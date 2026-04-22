using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSDashboardKwEntity
	{
		public decimal? ABBedarf { get; set; }
		public decimal? ABGesamt { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArticleNr { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? ImmediatAmount { get; set; }
		public int OpenFa { get; set; }
		public int? DYear { get; set; }
		public int? DKw { get; set; }
		public decimal? ProductionAmount { get; set; }
		public DateTime? ArtikelDate { get; set; }


		public CTSDashboardKwEntity() { }

		public CTSDashboardKwEntity(DataRow dataRow)
		{
			ABBedarf = (dataRow["ABBedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ABBedarf"]);
			ABGesamt = (dataRow["ABGesamt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ABGesamt"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
			ImmediatAmount = (dataRow["ImmediatAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ImmediatAmount"]);
			DYear = (dataRow["DYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DYear"]);
			DKw = (dataRow["DKw"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DKw"]);
			OpenFa = Convert.ToInt32(dataRow["OpenFa"]);
			ProductionAmount = (dataRow["ProductionAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionAmount"]);
			ArtikelDate = (dataRow["ArtikelDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArtikelDate"]);
			ArticleNr = (dataRow["ArticleNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleNr"]);


		}

		public CTSDashboardKwEntity ShallowClone()
		{
			return new CTSDashboardKwEntity
			{
				ABBedarf = ABBedarf,
				ABGesamt = ABGesamt,
				Artikelnummer = Artikelnummer,
				Bestand = Bestand,
				ImmediatAmount = ImmediatAmount,
				OpenFa = OpenFa,
				ProductionAmount = ProductionAmount,
				ArtikelDate = ArtikelDate
			};
		}
	}
}
