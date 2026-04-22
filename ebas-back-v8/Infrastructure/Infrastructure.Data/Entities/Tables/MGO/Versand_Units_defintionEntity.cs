using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.MGO
{
    public class Versand_Units_defintionEntity
    {
		public int ID { get; set; }
		public decimal? Max_hours { get; set; }
		public string Unit { get; set; }

        public Versand_Units_defintionEntity() { }

        public Versand_Units_defintionEntity(DataRow dataRow)
        {
			ID = Convert.ToInt32(dataRow["ID"]);
			Max_hours = (dataRow["Max_hours"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Max_hours"]);
			Unit = (dataRow["Unit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Unit"]);
        }
    
        public Versand_Units_defintionEntity ShallowClone()
        {
            return new Versand_Units_defintionEntity
            {
			ID = ID,
			Max_hours = Max_hours,
			Unit = Unit
            };
        }
    }
}

