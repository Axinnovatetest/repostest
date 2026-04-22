using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_Order_level_StatusEntity
	{
		public int Id_BOL { get; set; }
		public int Id_Level_Order { get; set; }
		public int Id_Status_Order { get; set; }
		public string Level_Order { get; set; }
		public string Status_Order { get; set; }

		public Budget_Order_level_StatusEntity() { }

		public Budget_Order_level_StatusEntity(DataRow dataRow)
		{
			Id_BOL = Convert.ToInt32(dataRow["Id_BOL"]);
			Id_Level_Order = Convert.ToInt32(dataRow["Id_Level_Order"]);
			Id_Status_Order = Convert.ToInt32(dataRow["Id_Status_Order"]);
			Level_Order = Convert.ToString(dataRow["Level_Order"]);
			Status_Order = Convert.ToString(dataRow["Status_Order"]);
		}
	}
}

