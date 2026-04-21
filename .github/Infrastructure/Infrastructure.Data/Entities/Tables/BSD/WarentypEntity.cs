using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class WarentypEntity
	{
		public string Warentyp { get; set; }
		public int Warentyp_ID { get; set; }

		public WarentypEntity() { }

		public WarentypEntity(DataRow dataRow)
		{
			Warentyp = (dataRow["Warentyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warentyp"]);
			Warentyp_ID = Convert.ToInt32(dataRow["Warentyp-ID"]);
		}
	}
}

