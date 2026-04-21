using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Supplier_Article_BudgetEntity
	{
		public string Article_supplier_name { get; set; }
		public int? Lieferantennummer { get; set; }
		public int Nr { get; set; }
		public string Ort { get; set; }
		public string Anrede { get; set; }
		public string Vorname { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Ansprechpartner { get; set; }
		public string Abteilung { get; set; }
		public string Strasse { get; set; }
		public string Postfach { get; set; }
		public string StrassePostfach { get; set; }
		public string LandPLZ { get; set; }
		public string Briefanrede { get; set; }

		public string LieferantenNummer { get; set; }
		public string Versandart { get; set; }
		public string Zahlungsweise { get; set; }
		public string Konditionszuordnungs { get; set; }

		public string IhrZeichen { get; set; }
		public string Nummer { get; set; }

		public string Fax { get; set; }
		public string Email { get; set; }
		public string PLZ { get; set; }
		public string Land { get; set; }


		public Supplier_Article_BudgetEntity() { }

		public Supplier_Article_BudgetEntity(DataRow dataRow)
		{
			Article_supplier_name = (dataRow["Article_supplier_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_supplier_name"]);
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			//
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Vorname = (dataRow["Vorname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname"]);
			Name1 = (dataRow["name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["name1"]);
			Name2 = (dataRow["name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["name2"]);
			Name3 = (dataRow["name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["name3"]);
			Ansprechpartner = (dataRow["name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["name3"]);
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			StrassePostfach = (dataRow["StrassePostfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StrassePostfach"]);
			LandPLZ = (dataRow["LandPLZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LandPLZ"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			Nummer = (dataRow["nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["nummer"]);

			LieferantenNummer = (dataRow["LieferantenNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LieferantenNummer"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Konditionszuordnungs = (dataRow["Konditionszuordnungs"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Konditionszuordnungs"]);
			IhrZeichen = (dataRow["IhrZeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IhrZeichen"]);

			IhrZeichen = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);


			Strasse = (dataRow["Strasse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Strasse"]);
			PLZ = (dataRow["PLZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ"]);
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
		}
	}
}

