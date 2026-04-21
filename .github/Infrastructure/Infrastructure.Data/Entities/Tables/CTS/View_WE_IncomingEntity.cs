using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class View_WE_IncomingEntity
    {
		public string Artikelnummer { get; set; }
		public string Eingangslieferscheinnr { get; set; }
		public int? Pos { get; set; }
		public int WE_ID { get; set; }
		public DateTime? WE_VOH_Datum { get; set; }
		public decimal? WE_VOH_Menge { get; set; }

        public View_WE_IncomingEntity() { }

        public View_WE_IncomingEntity(DataRow dataRow)
        {
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Eingangslieferscheinnr = (dataRow["Eingangslieferscheinnr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Eingangslieferscheinnr"]);
			Pos = (dataRow["Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Pos"]);
			WE_ID = Convert.ToInt32(dataRow["WE_ID"]);
			WE_VOH_Datum = (dataRow["WE_VOH_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["WE_VOH_Datum"]);
			WE_VOH_Menge = (dataRow["WE_VOH_Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["WE_VOH_Menge"]);
        }
    
        public View_WE_IncomingEntity ShallowClone()
        {
            return new View_WE_IncomingEntity
            {
			Artikelnummer = Artikelnummer,
			Eingangslieferscheinnr = Eingangslieferscheinnr,
			Pos = Pos,
			WE_ID = WE_ID,
			WE_VOH_Datum = WE_VOH_Datum,
			WE_VOH_Menge = WE_VOH_Menge
            };
        }
    }
}

