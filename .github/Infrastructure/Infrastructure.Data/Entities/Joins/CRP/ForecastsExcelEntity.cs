using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class ForecastsExcelEntity
	{
		public string Kunden { get; set; }
		public string Type { get; set; }
		public DateTime? Bedarfstermin { get; set; }
		public string Artikelnummer { get; set; }
		public string Material { get; set; }
		public int? Menge { get; set; }
		public int? Jahr { get; set; }
		public int? KW { get; set; }
		public decimal? VKE { get; set; }
		public decimal? GesamtPreis { get; set; }
		public DateTime? Datum { get; set; }
		public ForecastsExcelEntity(DataRow dataRow)
		{
			Kunden = (dataRow[columnName: "Kunden"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunden"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			Bedarfstermin = (dataRow["Bedarfstermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bedarfstermin"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
			Jahr = (dataRow["Jahr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Jahr"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			VKE = (dataRow["VKE"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKE"]);
			GesamtPreis = (dataRow["GesamtPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtPreis"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
		}
	}
}