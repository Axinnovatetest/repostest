using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class CableWithoutOrderEntity
	{
		public CableWithoutOrderEntity(DataRow datarow)
		{

			Bestand = (datarow["Bestand"] == DBNull.Value) ? 0 : Convert.ToDecimal(datarow["Bestand"]);
			Artikelnummer = (datarow["Artikelnummer"] == DBNull.Value) ? "" : Convert.ToString(datarow["Artikelnummer"]);
			Bezeichnung1 = (datarow["Bezeichnung 1"] == DBNull.Value) ? "" : Convert.ToString(datarow["Bezeichnung 1"]);
		}
		public decimal Bestand { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }


	}
}
