using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Entities.Tables.MGO
{
	public class GeplantStundenByTypEntity
	{
		public int id { get; set; }
		public int typ { get; set; }
		public decimal stunden { get; set; }
		public int jahr { get; set; }
		public int KW { get; set; }
		public DateTime? datum { get; set; }
		public GeplantStundenByTypEntity()
		{
		}

		public GeplantStundenByTypEntity(DataRow dataRow)
		{
			id = (dataRow["id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["id"]);
			typ = (dataRow["typ"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["typ"]);
			stunden = (dataRow["GesamtZeit"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["GesamtZeit"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["KW"]);
			jahr = (dataRow["Jahr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Jahr"]);
			datum = (dataRow["LastUpdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdate"]);

		}
	}
}
