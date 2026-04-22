using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_Protokollierung_Angebote2Entity
	{
		public string Angebot_Nr { get; set; }
		public string bestellung_Typ { get; set; }
		public string Bezug { get; set; }
		public DateTime? Gelöscht_am { get; set; }
		public string Gelöscht_durch { get; set; }
		public int ID { get; set; }
		public string Kunden_Nr { get; set; }
		public string Name { get; set; }
		public int? Projekt_Nr { get; set; }

		public PSZ_Protokollierung_Angebote2Entity() { }

		public PSZ_Protokollierung_Angebote2Entity(DataRow dataRow)
		{
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot-Nr"]);
			bestellung_Typ = (dataRow["bestellung_Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["bestellung_Typ"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			Gelöscht_am = (dataRow["Gelöscht-am"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Gelöscht-am"]);
			Gelöscht_durch = (dataRow["Gelöscht-durch"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gelöscht-durch"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kunden_Nr = (dataRow["Kunden-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunden-Nr"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Projekt-Nr"]);
		}
	}
}

