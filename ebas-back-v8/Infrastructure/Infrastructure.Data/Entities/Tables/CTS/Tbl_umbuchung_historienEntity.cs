using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Tbl_umbuchung_historienEntity
	{
		public int? Artikel_Nr { get; set; }
		public string Bearbeiter { get; set; }
		public DateTime? Datum_Planung { get; set; }
		public DateTime Datum_umbuchung { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int ID { get; set; }
		public int? ID_Fertigung { get; set; }
		public int? ID_Fertigung_HL { get; set; }
		public int? IDMHD1 { get; set; }
		public int? IDMHD2 { get; set; }
		public int? IDMHD3 { get; set; }
		public int? IDMHD4 { get; set; }
		public int? IDMHD5 { get; set; }
		public int? IDMHD6 { get; set; }
		public string InternStatus { get; set; }
		public int? LagerID_Ziel { get; set; }
		public int? Lagerort_ID { get; set; }
		public string Matrikelnummer { get; set; }
		public decimal? Menge_gebucht { get; set; }
		public int? reserve_zurueck { get; set; }
		public int? Rollennummer { get; set; }
		public int? Sens { get; set; }
		public int? Status { get; set; }

		public Tbl_umbuchung_historienEntity() { }

		public Tbl_umbuchung_historienEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Bearbeiter = (dataRow["Bearbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bearbeiter"]);
			Datum_Planung = (dataRow["Datum_Planung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum_Planung"]);
			Datum_umbuchung = Convert.ToDateTime(dataRow["Datum_umbuchung"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Fertigung = (dataRow["ID_Fertigung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fertigung"]);
			ID_Fertigung_HL = (dataRow["ID_Fertigung_HL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fertigung_HL"]);
			IDMHD1 = (dataRow["IDMHD1"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IDMHD1"]);
			IDMHD2 = (dataRow["IDMHD2"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IDMHD2"]);
			IDMHD3 = (dataRow["IDMHD3"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IDMHD3"]);
			IDMHD4 = (dataRow["IDMHD4"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IDMHD4"]);
			IDMHD5 = (dataRow["IDMHD5"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IDMHD5"]);
			IDMHD6 = (dataRow["IDMHD6"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IDMHD6"]);
			InternStatus = (dataRow["InternStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InternStatus"]);
			LagerID_Ziel = (dataRow["LagerID_Ziel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerID_Ziel"]);
			Lagerort_ID = (dataRow["Lagerort_ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_ID"]);
			Matrikelnummer = (dataRow["Matrikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Matrikelnummer"]);
			Menge_gebucht = (dataRow["Menge_gebucht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge_gebucht"]);
			reserve_zurueck = (dataRow["reserve_zurueck"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["reserve_zurueck"]);
			Rollennummer = (dataRow["Rollennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rollennummer"]);
			Sens = (dataRow["Sens"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Sens"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status"]);
		}

		public Tbl_umbuchung_historienEntity ShallowClone()
		{
			return new Tbl_umbuchung_historienEntity
			{
				Artikel_Nr = Artikel_Nr,
				Bearbeiter = Bearbeiter,
				Datum_Planung = Datum_Planung,
				Datum_umbuchung = Datum_umbuchung,
				Fertigungsnummer = Fertigungsnummer,
				ID = ID,
				ID_Fertigung = ID_Fertigung,
				ID_Fertigung_HL = ID_Fertigung_HL,
				IDMHD1 = IDMHD1,
				IDMHD2 = IDMHD2,
				IDMHD3 = IDMHD3,
				IDMHD4 = IDMHD4,
				IDMHD5 = IDMHD5,
				IDMHD6 = IDMHD6,
				InternStatus = InternStatus,
				LagerID_Ziel = LagerID_Ziel,
				Lagerort_ID = Lagerort_ID,
				Matrikelnummer = Matrikelnummer,
				Menge_gebucht = Menge_gebucht,
				reserve_zurueck = reserve_zurueck,
				Rollennummer = Rollennummer,
				Sens = Sens,
				Status = Status
			};
		}
	}
}

