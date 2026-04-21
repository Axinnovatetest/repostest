using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity
{
	public class SupplierEntity
	{
		public int? Standardlieferant { get; set; }
		public string? Name1 { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Bezeichnung { get; set; }
		public string? BestellNr { get; set; }
		public decimal? Grosse { get; set; }
		public decimal? Pruftiefe { get; set; }

		public SupplierEntity()
		{

		}
		public SupplierEntity(DataRow dataRow)
		{
			Standardlieferant = Convert.ToInt32(dataRow["Standardlieferant"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			BestellNr = (dataRow["BestellNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BestellNr"]);
			Grosse = (dataRow["Grosse"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Grosse"]);
			Pruftiefe = (dataRow["Pruftiefe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Pruftiefe"]);

		}
	}
}
