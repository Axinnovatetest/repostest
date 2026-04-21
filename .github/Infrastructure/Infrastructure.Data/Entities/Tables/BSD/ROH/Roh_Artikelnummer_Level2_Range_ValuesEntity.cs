using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
    public class Roh_Artikelnummer_Level2_Range_ValuesEntity
    {
		public int? FromOrTwo { get; set; }
		public int Id { get; set; }
		public int? IdLevelTwo { get; set; }
		public string RangeValue { get; set; }

        public Roh_Artikelnummer_Level2_Range_ValuesEntity() { }

        public Roh_Artikelnummer_Level2_Range_ValuesEntity(DataRow dataRow)
        {
			FromOrTwo = (dataRow["FromOrTwo"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FromOrTwo"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdLevelTwo = (dataRow["IdLevelTwo"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdLevelTwo"]);
			RangeValue = (dataRow["RangeValue"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RangeValue"]);
        }
    
        public Roh_Artikelnummer_Level2_Range_ValuesEntity ShallowClone()
        {
            return new Roh_Artikelnummer_Level2_Range_ValuesEntity
            {
			FromOrTwo = FromOrTwo,
			Id = Id,
			IdLevelTwo = IdLevelTwo,
			RangeValue = RangeValue
            };
        }
    }
}

