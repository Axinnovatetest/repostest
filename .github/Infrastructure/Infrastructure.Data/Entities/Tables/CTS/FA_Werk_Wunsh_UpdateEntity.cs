using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class FA_Werk_Wunsh_UpdateEntity
	{
		public DateTime? Dateupdate { get; set; }
		public int Id { get; set; }
		public int? IdUser { get; set; }
		public string Typ { get; set; }
		public string userName { get; set; }

		public FA_Werk_Wunsh_UpdateEntity() { }

		public FA_Werk_Wunsh_UpdateEntity(DataRow dataRow)
		{
			Dateupdate = (dataRow["Dateupdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Dateupdate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdUser = (dataRow["IdUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdUser"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			userName = (dataRow["userName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["userName"]);
		}
	}
}

