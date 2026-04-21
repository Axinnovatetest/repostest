using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class HistoriePackingEntity
	{
		public long AngebotNr { get; set; }
		public string artikelnummer { get; set; }
		public decimal menge { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZOrt { get; set; }
		public string versandart { get; set; }
		public DateTime? versanddatum { get; set; }
		public bool versandt { get; set; }
		public bool gepackt { get; set; }
		public bool gedruckt { get; set; }
		public bool gebucht { get; set; }
		public bool vda { get; set; }
		public int nombreTotal { get; set; }

		public HistoriePackingEntity(DataRow dataRow)
		{

			AngebotNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt64(dataRow["Angebot-Nr"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			menge = (dataRow["Menge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Menge"]);
			lVornameNameFirma = (dataRow["LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVorname/NameFirma"]);
			lStrassePostfach = (dataRow["LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStraße/Postfach"]);
			lLandPLZOrt = (dataRow["LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand/PLZ/Ort"]);
			versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			versanddatum = (dataRow["Versanddatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Versanddatum"]);
			versandt = (dataRow["versandt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["versandt"]);
			gepackt = (dataRow["gepackt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gepackt"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gedruckt"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gebucht"]);
			vda = (dataRow["VDA_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["VDA_gedruckt"]);
			nombreTotal = (dataRow["nombreTotal"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["nombreTotal"]);

		}
	}
}
