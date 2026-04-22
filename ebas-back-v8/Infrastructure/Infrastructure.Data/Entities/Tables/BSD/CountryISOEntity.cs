using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class CountryISOEntity
	{
		public string alpha2Code { get; set; }
		public string alpha3Code { get; set; }
		public string Capital { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public string Description { get; set; }
		public string Flag { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string NativeName { get; set; }
		public string NumericCode { get; set; }
		public string Region { get; set; }
		public string Subregion { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }

		public CountryISOEntity() { }

		public CountryISOEntity(DataRow dataRow)
		{
			alpha2Code = (dataRow["alpha2Code"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["alpha2Code"]);
			alpha3Code = (dataRow["alpha3Code"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["alpha3Code"]);
			Capital = (dataRow["Capital"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Capital"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Flag = (dataRow["Flag"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Flag"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			NativeName = (dataRow["NativeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["NativeName"]);
			NumericCode = (dataRow["NumericCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["NumericCode"]);
			Region = (dataRow["Region"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Region"]);
			Subregion = (dataRow["Subregion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Subregion"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
		}
	}
}

