using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.MGO
{
	public class GeplantStundenEntity
	{
		public int id { get; set; }
		public string kunde { get; set; }
		public decimal stunden { get; set; }
		public decimal geschnittenStunden { get; set; }
		public decimal gestartetStunden { get; set; }
		public int jahr { get; set; }
		public int KW { get; set; }
		public DateTime? datum { get; set; }
		public GeplantStundenEntity()
		{
		}

		public GeplantStundenEntity(DataRow dataRow)
		{
			id = (dataRow["id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["id"]);
			kunde = (dataRow["kunde"] == System.DBNull.Value) ? "": Convert.ToString(dataRow["kunde"]);
			stunden = (dataRow["GesamtZeit"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["GesamtZeit"]);
			geschnittenStunden = (dataRow["GeschnittenZeit"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["GeschnittenZeit"]);
			gestartetStunden = (dataRow["GestartetZeit"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["GestartetZeit"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["KW"]);
			jahr = (dataRow["Jahr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Jahr"]);
			datum = (dataRow["LastUpdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdate"]);

		}
	}
}
