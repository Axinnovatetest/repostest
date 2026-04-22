using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class RahmensPositionsConsumptionEntity
	{
		public int Nr { get; set; }
		public int? Position { get; set; }
		public int? Angebot_Nr { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? OriginalAnzahl { get; set; }
		public decimal? Geliefert { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? Consumption { get; set; }
		public decimal? GesamtpreisDefault { get; set; }
		public DateTime? GultigBis { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public string StatusName { get; set; }
		public int? StatusId { get; set; }
		public string Projekt_Nr { get; set; }
		public decimal? NeededInBOM { get; set; }
		public decimal? SumNeeded { get; set; }
		public decimal? Total { get; set; }
		public string Supplier { get; set; }
		public int? SupplierId { get; set; }
		public string DocumentNumber { get; set; }
		public RahmensPositionsConsumptionEntity(DataRow dataRow)
		{
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Position = (dataRow[columnName: "Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Angebot_Nr = (dataRow[columnName: "Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Artikel_Nr = (dataRow[columnName: "Artikel-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow[columnName: "Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			OriginalAnzahl = (dataRow[columnName: "OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OriginalAnzahl"]);
			Geliefert = (dataRow[columnName: "Geliefert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Geliefert"]);
			Anzahl = (dataRow[columnName: "Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			NeededInBOM = (dataRow[columnName: "NeededInBOM"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["NeededInBOM"]);
			SumNeeded = (dataRow[columnName: "SumNeeded"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumNeeded"]);
			Total = (dataRow[columnName: "Total"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Total"]);
			Consumption = (dataRow[columnName: "Consumption"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Consumption"]);
			GesamtpreisDefault = (dataRow[columnName: "GesamtpreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisDefault"]);
			GultigBis = (dataRow[columnName: "GultigBis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigBis"]);
			ExtensionDate = (dataRow[columnName: "ExtensionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExtensionDate"]);
			StatusName = (dataRow[columnName: "StatusName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StatusName"]);
			StatusId = (dataRow[columnName: "StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
			Projekt_Nr = (dataRow[columnName: "Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			Supplier = (dataRow[columnName: "Supplier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier"]);
			SupplierId = (dataRow[columnName: "SupplierId"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["SupplierId"]);
			DocumentNumber = (dataRow[columnName: "DocumentNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentNumber"]);
		}
	}
}
