using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LieferantMitAnzahEntity
	{
		public string name1 { get; set; }
		public int anzahlVonWareneingang { get; set; }
		public LieferantMitAnzahEntity()
		{

		}
		public LieferantMitAnzahEntity(DataRow dataRow)
		{
			name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			anzahlVonWareneingang = (dataRow["AnzahlvonLiefertermin"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["AnzahlvonLiefertermin"]);

		}
	}
}
