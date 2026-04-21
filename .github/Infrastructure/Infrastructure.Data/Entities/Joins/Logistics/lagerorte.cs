using System;
using System.Data;
namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class lagerorte
	{
		public int Lagerort_id { get; set; }
		public string Lagerort { get; set; }
		public lagerorte(DataRow dataRow)
		{

			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort_id"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);

		}
	}
}
