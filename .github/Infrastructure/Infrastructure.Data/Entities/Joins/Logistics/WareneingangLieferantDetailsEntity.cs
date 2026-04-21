using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class WareneingangLieferantDetailsEntity
	{
		public long projektNr { get; set; }
		public string typ { get; set; }
		public string artikelnummer { get; set; }
		public decimal SummeVonAnzahl { get; set; }
		public string einheit { get; set; }
		public string name1 { get; set; }
		public string name1Lower { get { return name1.ToLower(); } }
		public DateTime? liefertermin { get; set; }
		public int mois { get; set; }
		public int annee { get; set; }
		public WareneingangLieferantDetailsEntity()
		{

		}
		public WareneingangLieferantDetailsEntity(DataRow dataRow)
		{
			projektNr = (dataRow["ProjektNr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["ProjektNr"]);
			typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			SummeVonAnzahl = (dataRow["SummevonAnzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SummevonAnzahl"]);
			einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			mois = (dataRow["Mois"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Mois"]);
			annee = (dataRow["annee"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["annee"]);

		}
	}
}
