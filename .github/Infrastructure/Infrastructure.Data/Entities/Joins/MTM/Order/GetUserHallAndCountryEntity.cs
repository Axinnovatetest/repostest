using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class GetUserHallAndCountryEntity
	{
		public int Country_Id { get; set; }
		public int Hall_Id { get; set; }
		public string HallName { get; set; }
		public string CountryName { get; set; }
		public GetUserHallAndCountryEntity(DataRow dataRow)
		{
			Country_Id = (dataRow["Country_Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Country_Id"]);
			Hall_Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			HallName = (dataRow["HallName"] == System.DBNull.Value) ? "" : dataRow["HallName"].ToString();
			CountryName = (dataRow["CountryName"] == System.DBNull.Value) ? "" : dataRow["CountryName"].ToString();
		}
	}
}
