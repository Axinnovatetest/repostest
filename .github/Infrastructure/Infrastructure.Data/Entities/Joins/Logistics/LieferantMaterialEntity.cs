using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LieferantMaterialEntity
	{
		public LieferantMaterialEntity(DataRow dataRow)
		{
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
		}
		public string Name1 { get; set; }
	}
}
