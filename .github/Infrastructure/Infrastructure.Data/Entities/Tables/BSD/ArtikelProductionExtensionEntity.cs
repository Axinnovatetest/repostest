using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArtikelProductionExtensionEntity
	{
		public bool? AlternativeProductionPlace { get; set; }
		public int ArticleId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public int Id { get; set; }
		public int? ProductionPlace1_Id { get; set; }
		public string ProductionPlace1_Name { get; set; }
		public int? ProductionPlace2_Id { get; set; }
		public string ProductionPlace2_Name { get; set; }
		public int? ProductionPlace3_Id { get; set; }
		public string ProductionPlace3_Name { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }

		public ArtikelProductionExtensionEntity() { }

		public ArtikelProductionExtensionEntity(DataRow dataRow)
		{
			AlternativeProductionPlace = (dataRow["AlternativeProductionPlace"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AlternativeProductionPlace"]);
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProductionPlace1_Id = (dataRow["ProductionPlace1_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionPlace1_Id"]);
			ProductionPlace1_Name = (dataRow["ProductionPlace1_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionPlace1_Name"]);
			ProductionPlace2_Id = (dataRow["ProductionPlace2_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionPlace2_Id"]);
			ProductionPlace2_Name = (dataRow["ProductionPlace2_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionPlace2_Name"]);
			ProductionPlace3_Id = (dataRow["ProductionPlace3_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionPlace3_Id"]);
			ProductionPlace3_Name = (dataRow["ProductionPlace3_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionPlace3_Name"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
		}
	}
}

