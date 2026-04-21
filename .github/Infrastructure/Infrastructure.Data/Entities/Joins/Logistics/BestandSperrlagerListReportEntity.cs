using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class BestandSperrlagerListReportEntity
	{
		public BestandSperrlagerListReportEntity(DataRow datarow)
		{

			Artikelnummer = (datarow["Artikelnummer"] == DBNull.Value) ? "" : Convert.ToString(datarow["Artikelnummer"]);
			Bezeichnung1 = (datarow["Bezeichnung 1"] == DBNull.Value) ? "" : Convert.ToString(datarow["Bezeichnung 1"]);
			Bestand = (datarow["Bestand"] == DBNull.Value) ? 0 : Convert.ToDecimal(datarow["Bestand"]);
			Lagerort_id = (datarow["Lagerort_id"] == DBNull.Value) ? 0 : Convert.ToInt32(datarow["Lagerort_id"]);


		}

		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Bestand { get; set; }
		public int Lagerort_id { get; set; }

	}
}
