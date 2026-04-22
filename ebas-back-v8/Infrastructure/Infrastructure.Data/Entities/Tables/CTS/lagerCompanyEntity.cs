using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class lagerCompanyEntity
	{
		public int? Company_id { get; set; }
		public int Id { get; set; }
		public int? Lagerort_id { get; set; }

		public lagerCompanyEntity() { }

		public lagerCompanyEntity(DataRow dataRow)
		{
			Company_id = (dataRow["Company_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Company_id"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
		}
	}
}

