using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class NeedsInRahmenSaleEntity
	{
		public int? Nr { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public int? Position { get; set; }
		public string Bezug { get; set; }
		public decimal? RestQty { get; set; }
		public decimal? NeededInBOM { get; set; }
		public decimal? SumNeeded { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public string Customer { get; set; }
		public NeedsInRahmenSaleEntity(DataRow dataRow)
		{
			Nr = (dataRow[columnName: "Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			Angebot_Nr = (dataRow[columnName: "Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Customer = (dataRow[columnName: "Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer"]);
			ArtikelNr = (dataRow[columnName: "Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Position = (dataRow[columnName: "Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Bezug = (dataRow[columnName: "Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			RestQty = (dataRow[columnName: "RestQty"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RestQty"]);
			NeededInBOM = (dataRow[columnName: "NeededInBOM"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["NeededInBOM"]);
			SumNeeded = (dataRow[columnName: "SumNeeded"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumNeeded"]);
			ExtensionDate = (dataRow[columnName: "ExtensionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExtensionDate"]);
		}
	}
}