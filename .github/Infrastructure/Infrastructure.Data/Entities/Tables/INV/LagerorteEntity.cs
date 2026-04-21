using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.INV
{
	public class LagerorteEntity
	{
		public string Lagerort { get; set; }
		public int LagerortId { get; set; }
		public bool? Standard { get; set; }

		public LagerorteEntity() { }
		public LagerorteEntity(DataRow dataRow)
		{
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			LagerortId = Convert.ToInt32(dataRow["Lagerort_id"]);
			Standard = (dataRow["Standard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standard"]);
		}
	}
}

