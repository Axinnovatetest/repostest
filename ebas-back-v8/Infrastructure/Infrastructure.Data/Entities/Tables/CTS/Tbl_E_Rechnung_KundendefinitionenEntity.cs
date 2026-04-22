using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Tbl_E_Rechnung_KundendefinitionenEntity
	{
		public string Betreff { get; set; }
		public string Email { get; set; }
		public string EmailVermerk { get; set; }
		public int ID { get; set; }
		public string Kundenname { get; set; }
		public int? Kundennummer { get; set; }
		public string Rechnung_Name { get; set; }
		public string Typ { get; set; }
		public DateTime? Versand { get; set; }

		public Tbl_E_Rechnung_KundendefinitionenEntity() { }

		public Tbl_E_Rechnung_KundendefinitionenEntity(DataRow dataRow)
		{
			Betreff = (dataRow["Betreff"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Betreff"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			EmailVermerk = (dataRow["EmailVermerk"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailVermerk"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kundenname = (dataRow["Kundenname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundenname"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Rechnung_Name = (dataRow["Rechnung_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rechnung_Name"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Versand = (dataRow["Versand"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Versand"]);
		}

		public Tbl_E_Rechnung_KundendefinitionenEntity ShallowClone()
		{
			return new Tbl_E_Rechnung_KundendefinitionenEntity
			{
				Betreff = Betreff,
				Email = Email,
				EmailVermerk = EmailVermerk,
				ID = ID,
				Kundenname = Kundenname,
				Kundennummer = Kundennummer,
				Rechnung_Name = Rechnung_Name,
				Typ = Typ,
				Versand = Versand
			};
		}
	}
}

