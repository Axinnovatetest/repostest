using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities
{
	public class AccessInventurEntity
	{
		public bool? Blocking_FA_Start { get; set; }
		public bool? Blocking_ScannerInventur { get; set; }
		public bool? Blocking_Schneidenerei_Gewerk_1_3 { get; set; }
		public bool? Blocking_Typ1HL_PL { get; set; }
		public bool? Blocking_Typ1PL_HL { get; set; }
		public bool? Blocking_Typ2HL_PL { get; set; }
		public bool? Blocking_Typ2PL_HL { get; set; }
		public bool? Blocking_WarehouseMovements { get; set; }
		public int Lagerort_id { get; set; }

		public AccessInventurEntity() { }

		public AccessInventurEntity(DataRow dataRow)
		{
			Blocking_FA_Start = (dataRow["Blocking_FA_Start"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_FA_Start"]);
			Blocking_ScannerInventur = (dataRow["Blocking_ScannerInventur"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_ScannerInventur"]);
			Blocking_Schneidenerei_Gewerk_1_3 = (dataRow["Blocking_Schneidenerei_Gewerk_1_3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_Schneidenerei_Gewerk_1_3"]);
			Blocking_Typ1HL_PL = (dataRow["Blocking_Typ1HL_PL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_Typ1HL_PL"]);
			Blocking_Typ1PL_HL = (dataRow["Blocking_Typ1PL_HL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_Typ1PL_HL"]);
			Blocking_Typ2HL_PL = (dataRow["Blocking_Typ2HL_PL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_Typ2HL_PL"]);
			Blocking_Typ2PL_HL = (dataRow["Blocking_Typ2PL_HL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_Typ2PL_HL"]);
			Blocking_WarehouseMovements = (dataRow["Blocking_WarehouseMovements"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blocking_WarehouseMovements"]);
			Lagerort_id = Convert.ToInt32(dataRow["Lagerort_id"]);
		}

		public AccessInventurEntity ShallowClone()
		{
			return new AccessInventurEntity
			{
				Blocking_FA_Start = Blocking_FA_Start,
				Blocking_ScannerInventur = Blocking_ScannerInventur,
				Blocking_Schneidenerei_Gewerk_1_3 = Blocking_Schneidenerei_Gewerk_1_3,
				Blocking_Typ1HL_PL = Blocking_Typ1HL_PL,
				Blocking_Typ1PL_HL = Blocking_Typ1PL_HL,
				Blocking_Typ2HL_PL = Blocking_Typ2HL_PL,
				Blocking_Typ2PL_HL = Blocking_Typ2PL_HL,
				Blocking_WarehouseMovements = Blocking_WarehouseMovements,
				Lagerort_id = Lagerort_id
			};
		}
	}
}

