using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSDashboardEntity
	{
		public decimal? ABBedarf { get; set; }
		public decimal? ABGesamt { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? ImmediatAmount { get; set; }
		public int OpenFa { get; set; }
		public decimal? ProductionAmount { get; set; }
		public bool SuspiciousPrice { get; set; }
		public int Artikel_Nr { get; set; }

		public CTSDashboardEntity() { }

		public CTSDashboardEntity(DataRow dataRow)
		{
			try
			{
				ABBedarf = (dataRow["ABBedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ABBedarf"]);
				ABGesamt = (dataRow["ABGesamt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ABGesamt"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				ImmediatAmount = (dataRow["ImmediatAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ImmediatAmount"]);
				OpenFa = Convert.ToInt32(dataRow["OpenFa"]);
				Artikel_Nr = Convert.ToInt32(dataRow["Artikel_Nr"]);
				ProductionAmount = (dataRow["ProductionAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionAmount"]);
				SuspiciousPrice = Convert.ToBoolean(dataRow["SuspiciousPrice"]);
			} catch(Exception)
			{

				//throw;
			}
		}

		public CTSDashboardEntity ShallowClone()
		{
			return new CTSDashboardEntity
			{
				ABBedarf = ABBedarf,
				ABGesamt = ABGesamt,
				Artikelnummer = Artikelnummer,
				Bestand = Bestand,
				ImmediatAmount = ImmediatAmount,
				OpenFa = OpenFa,
				ProductionAmount = ProductionAmount,
				SuspiciousPrice = SuspiciousPrice,
				Artikel_Nr = Artikel_Nr
			};
		}
	}
}
