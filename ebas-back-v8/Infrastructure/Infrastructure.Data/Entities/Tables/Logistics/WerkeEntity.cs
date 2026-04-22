using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class WerkeEntity
	{

		public string City1 { get; set; }
		public string City2 { get; set; }
		public string Country { get; set; }
		public int Id { get; set; }
		public int? IdCompany { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string SiteName { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string ZipCode { get; set; }

		public WerkeEntity() { }

		public WerkeEntity(DataRow dataRow)
		{
			City1 = (dataRow["City1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["City1"]);
			City2 = (dataRow["City2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["City2"]);
			Country = (dataRow["Country"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Country"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdCompany = (dataRow["IdCompany"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdCompany"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			SiteName = (dataRow["SiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SiteName"]);
			Street1 = (dataRow["Street1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Street1"]);
			Street2 = (dataRow["Street2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Street2"]);
			ZipCode = (dataRow["ZipCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ZipCode"]);
		}

		public WerkeEntity ShallowClone()
		{
			return new WerkeEntity
			{
				City1 = City1,
				City2 = City2,
				Country = Country,
				Id = Id,
				IdCompany = IdCompany,
				Name1 = Name1,
				Name2 = Name2,
				SiteName = SiteName,
				Street1 = Street1,
				Street2 = Street2,
				ZipCode = ZipCode
			};
		}
	}
}

