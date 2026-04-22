using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM.Orders
{
	public class PSZ_Protokolierung_Geloschte_Bestellungen2Entity
	{
		public string Bestellung_Typ { get; set; }
		public int? Bestellung_Nr { get; set; }
		public DateTime? Geloscht_AM { get; set; }
		public string Geloscht_durch { get; set; }
		public int ID { get; set; }
		public int? Lieferanten_nr { get; set; }
		public string Name { get; set; }
		public string Projekt_Nr { get; set; }

		public PSZ_Protokolierung_Geloschte_Bestellungen2Entity() { }

		public PSZ_Protokolierung_Geloschte_Bestellungen2Entity(DataRow dataRow)
		{
			Bestellung_Typ = dataRow["Bestellung_Typ"] == DBNull.Value ? "" : Convert.ToString(dataRow["Bestellung_Typ"]);
			Bestellung_Nr = dataRow["Bestellung-Nr"] == DBNull.Value ? null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Geloscht_AM = dataRow["Gelöscht AM"] == DBNull.Value ? null : Convert.ToDateTime(dataRow["Gelöscht AM"]);
			Geloscht_durch = dataRow["Gelöscht durch"] == DBNull.Value ? "" : Convert.ToString(dataRow["Gelöscht durch"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Lieferanten_nr = dataRow["Lieferanten-nr"] == DBNull.Value ? null : Convert.ToInt32(dataRow["Lieferanten-nr"]);
			Name = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);
			Projekt_Nr = dataRow["Projekt-Nr"] == DBNull.Value ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
		}

		public PSZ_Protokolierung_Geloschte_Bestellungen2Entity ShallowClone()
		{
			return new PSZ_Protokolierung_Geloschte_Bestellungen2Entity
			{
				Bestellung_Typ = Bestellung_Typ,
				Bestellung_Nr = Bestellung_Nr,
				Geloscht_AM = Geloscht_AM,
				Geloscht_durch = Geloscht_durch,
				ID = ID,
				Lieferanten_nr = Lieferanten_nr,
				Name = Name,
				Projekt_Nr = Projekt_Nr
			};
		}
	}
}

