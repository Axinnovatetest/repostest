using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class CRPAuswertungRahmenFGArtikelEntity
	{
		public int? Rahmen { get; set; }
		public string Type { get; set; }
		public string Kunde { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Originalmenge { get; set; }
		public decimal? Restmenge { get; set; }
		public decimal? Einzelpreis { get; set; }
		public decimal? PreisRestmenge { get; set; }
		public DateTime? Enddatum { get; set; }
		public string Status { get; set; }
		public CRPAuswertungRahmenFGArtikelEntity()
		{

		}
		public CRPAuswertungRahmenFGArtikelEntity(DataRow dataRow)
		{
			Rahmen = (dataRow[columnName: "Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Type = (dataRow[columnName: "BlanketTypeName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["BlanketTypeName"]);
			Kunde = (dataRow[columnName: "CustomerName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerName"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow[columnName: "Bezeichnung"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Bezeichnung"]);
			Originalmenge = (dataRow[columnName: "OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OriginalAnzahl"]);
			Restmenge = (dataRow[columnName: "Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Einzelpreis = (dataRow[columnName: "PreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PreisDefault"]);
			PreisRestmenge = (dataRow[columnName: "GesamtpreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisDefault"]);
			Enddatum = (dataRow[columnName: "GultigBis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigBis"]);
			Status = (dataRow[columnName: "StatusName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["StatusName"]);
		}
	}
}