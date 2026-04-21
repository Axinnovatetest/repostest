using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class FA_Werk_Wunsh_Update_detailsEntity
	{
		public int? FA { get; set; }
		public int Id { get; set; }
		public int? Id_update { get; set; }
		public bool? updated { get; set; }
		public DateTime? Werk { get; set; }

		public FA_Werk_Wunsh_Update_detailsEntity() { }

		public FA_Werk_Wunsh_Update_detailsEntity(DataRow dataRow)
		{
			FA = (dataRow["FA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Id_update = (dataRow["Id_update"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_update"]);
			updated = (dataRow["updated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["updated"]);
			Werk = (dataRow["Werk"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Werk"]);
		}
	}
}

