using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LagerOrt_Id
	{
		public int Lagerort_id { get; set; }

		public LagerOrt_Id(DataRow dataRow)
		{

			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort_id"]);


		}
	}
}
