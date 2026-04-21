using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.FAUpdate
{
	public class FACRPUpdateEntity
	{
		public int Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Kennzeichen { get; set; }
		public bool? gedruckt { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public string KundenIndex { get; set; }
		public decimal? Zeit { get; set; }
		public DateTime? Datum { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public bool CanUpdate { get; set; } = false;
		public bool InFrozenZone { get; set; } = false;
		public bool IsStarted { get; set; } = false;
		public int? OrderNumber { get; set; }
		public int? OrderId { get; set; }
		public string OrderType { get; set; }
		public decimal? Anzahl { get; set; }
		public FACRPUpdateEntity()
		{

		}
		public FACRPUpdateEntity(DataRow dataRow)
		{
			Fertigungsnummer = Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow[columnName: "Artikelnummer"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow[columnName: "Kennzeichen"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow[columnName: "gedruckt"]);
			FA_Druckdatum = (dataRow["FA_Druckdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow[columnName: "FA_Druckdatum"]);
			KundenIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow[columnName: "KundenIndex"]);
			Zeit = (dataRow["Zeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow[columnName: "Zeit"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow[columnName: "Datum"]);
			Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow[columnName: "Termin_Bestätigt1"]);
			CanUpdate = Convert.ToBoolean(dataRow[columnName: "CanUpdate"]);
			InFrozenZone = Convert.ToBoolean(dataRow[columnName: "InFrozenZone"]);
			IsStarted = Convert.ToBoolean(dataRow[columnName: "IsStarted"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow[columnName: "OrderNumber"]);
			OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow[columnName: "OrderId"]);
			OrderType = (dataRow["OrderType"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow[columnName: "OrderType"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow[columnName: "Anzahl"]);
		}
	}
}