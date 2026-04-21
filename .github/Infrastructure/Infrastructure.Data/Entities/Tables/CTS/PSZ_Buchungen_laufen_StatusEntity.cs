using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_Buchungen_laufen_StatusEntity
	{
		public bool? AB_lauft { get; set; }
		public bool? Bedarfsvorschau_lauft { get; set; }
		public bool? FA_lauft { get; set; }
		public bool? FAPlannungAgent_lauft { get; set; }
		public bool? FAStappeldruck_lauft { get; set; }
		public bool? GS_lauft { get; set; }
		public int ID { get; set; }
		public bool? Kanban_lauft { get; set; }
		public bool? LS_lauft { get; set; }
		public bool? PO_lauft { get; set; }
		public bool? PRSStockWarnings_lauft { get; set; }
		public bool? Rahmen_AB_lauft { get; set; }
		public bool? RG_lauft { get; set; }
		public bool? UBGPlannungAgent_lauft { get; set; }
		public bool? WE_lauft { get; set; }

		public PSZ_Buchungen_laufen_StatusEntity() { }

		public PSZ_Buchungen_laufen_StatusEntity(DataRow dataRow)
		{
			AB_lauft = (dataRow["AB läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AB läuft"]);
			Bedarfsvorschau_lauft = (dataRow["Bedarfsvorschau läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bedarfsvorschau läuft"]);
			FA_lauft = (dataRow["FA läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA läuft"]);
			FAPlannungAgent_lauft = (dataRow["FAPlannungAgent laüft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannungAgent laüft"]);
			FAStappeldruck_lauft = (dataRow["FAStappeldruck läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStappeldruck läuft"]);
			GS_lauft = (dataRow["GS läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GS läuft"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kanban_lauft = (dataRow["Kanban läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban läuft"]);
			LS_lauft = (dataRow["LS läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LS läuft"]);
			PO_lauft = (dataRow["PO läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PO läuft"]);
			PRSStockWarnings_lauft = (dataRow["PRSStockWarnings laüft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PRSStockWarnings laüft"]);
			Rahmen_AB_lauft = (dataRow["Rahmen AB läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen AB läuft"]);
			RG_lauft = (dataRow["RG läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RG läuft"]);
			UBGPlannungAgent_lauft = (dataRow["UBGPlannungAgent laüft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBGPlannungAgent laüft"]);
			WE_lauft = (dataRow["WE läuft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["WE läuft"]);
		}

		public PSZ_Buchungen_laufen_StatusEntity ShallowClone()
		{
			return new PSZ_Buchungen_laufen_StatusEntity
			{
				AB_lauft = AB_lauft,
				Bedarfsvorschau_lauft = Bedarfsvorschau_lauft,
				FA_lauft = FA_lauft,
				FAPlannungAgent_lauft = FAPlannungAgent_lauft,
				FAStappeldruck_lauft = FAStappeldruck_lauft,
				GS_lauft = GS_lauft,
				ID = ID,
				Kanban_lauft = Kanban_lauft,
				LS_lauft = LS_lauft,
				PO_lauft = PO_lauft,
				PRSStockWarnings_lauft = PRSStockWarnings_lauft,
				Rahmen_AB_lauft = Rahmen_AB_lauft,
				RG_lauft = RG_lauft,
				UBGPlannungAgent_lauft = UBGPlannungAgent_lauft,
				WE_lauft = WE_lauft
			};
		}
	}
}

