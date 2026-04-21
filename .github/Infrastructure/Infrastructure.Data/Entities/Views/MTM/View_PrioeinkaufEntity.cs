using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Views.MTM
{
	public class View_PrioeinkaufEntity
	{
		public decimal? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public DateTime? Datum { get; set; }
		public bool? erledigt { get; set; }
		public bool? erledigt_pos { get; set; }
		public string Fax { get; set; }
		public bool? gebucht { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string Name1 { get; set; }
		public bool? Position_erledigt { get; set; }
		public string Telefon { get; set; }
		public string Typ { get; set; }

		public View_PrioeinkaufEntity() { }

		public View_PrioeinkaufEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Position erledigt"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
		}

		public View_PrioeinkaufEntity ShallowClone()
		{
			return new View_PrioeinkaufEntity
			{
				Anzahl = Anzahl,
				Artikelnummer = Artikelnummer,
				Bestatigter_Termin = Bestatigter_Termin,
				Bestellung_Nr = Bestellung_Nr,
				Bezeichnung_1 = Bezeichnung_1,
				Datum = Datum,
				erledigt = erledigt,
				erledigt_pos = erledigt_pos,
				Fax = Fax,
				gebucht = gebucht,
				Lagerort_id = Lagerort_id,
				Liefertermin = Liefertermin,
				Name1 = Name1,
				Position_erledigt = Position_erledigt,
				Telefon = Telefon,
				Typ = Typ
			};
		}
	}
}

