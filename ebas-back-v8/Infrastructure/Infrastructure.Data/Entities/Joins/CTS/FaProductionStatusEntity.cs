using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class FaProductionStatusEntity
	{
		public int? Anzahl { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int Flag { get; set; }
		public int ID { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Produktionstermin { get; set; }
		public DateTime? Werktermin { get; set; }
		public string Bemerkung { get; set; }
		public string Status { get; set; }
		public int TotalCount { get; set; }

		public FaProductionStatusEntity() { }

		public FaProductionStatusEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Flag = Convert.ToInt32(dataRow["Flag"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Produktionstermin = (dataRow["Produktionstermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Produktionstermin"]);
			Werktermin = (dataRow["Werktermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Werktermin"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			TotalCount = Convert.ToInt32(dataRow["TotalCount"]);

		}

		public FaProductionStatusEntity ShallowClone()
		{
			return new FaProductionStatusEntity
			{
				Anzahl = Anzahl,
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Fertigungsnummer = Fertigungsnummer,
				Flag = Flag,
				ID = ID,
				Lagerort_id = Lagerort_id,
				Produktionstermin = Produktionstermin,
				Werktermin = Werktermin,
				Status = Status
			};
		}
	}
}
