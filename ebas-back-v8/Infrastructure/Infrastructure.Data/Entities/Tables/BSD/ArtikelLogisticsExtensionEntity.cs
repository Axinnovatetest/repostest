using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArtikelLogisticsExtensionEntity
	{
		public string Abladeort { get; set; }
		public string Anlieferadresse { get; set; }
		public DateTime? Anlieferadresse_Abladeort { get; set; }
		public int ArticleId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public int Id { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public bool? VDALabel { get; set; }

		public ArtikelLogisticsExtensionEntity() { }

		public ArtikelLogisticsExtensionEntity(DataRow dataRow)
		{
			Abladeort = (dataRow["Abladeort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladeort"]);
			Anlieferadresse = (dataRow["Anlieferadresse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anlieferadresse"]);
			Anlieferadresse_Abladeort = (dataRow["Anlieferadresse_Abladeort"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Anlieferadresse_Abladeort"]);
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
			VDALabel = (dataRow["VDALabel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDALabel"]);
		}
	}
}

