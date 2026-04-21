using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class LagerWertEntity
	{
		public string warengroup { get; set; }
		public string lagerort { get; set; }
		public decimal bestandWert { get; set; }
		public LagerWertEntity()
		{
		}

		public LagerWertEntity(DataRow dataRow)
		{
			warengroup = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ?"" : Convert.ToString(dataRow["Lagerort"]);
			bestandWert = (dataRow["Bestandswert (Summe EUR)"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestandswert (Summe EUR)"]);

		}
	}
}
