using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class LagerorteEntity
	{
		public string Lagerort { get; set; }
		public int Lagerort_id { get; set; }
		public int? Simulieren { get; set; }
		public bool? Standard { get; set; }
		public int? StandardLagerortId { get; set; }
		public string User_Simulieren { get; set; }
		public int? WerkNachId { get; set; }
		public int? WerkVonId { get; set; }

		public LagerorteEntity() { }

		public LagerorteEntity(DataRow dataRow)
		{
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Lagerort_id = Convert.ToInt32(dataRow["Lagerort_id"]);
			Simulieren = (dataRow["Simulieren"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Simulieren"]);
			Standard = (dataRow["Standard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standard"]);
			StandardLagerortId = (dataRow["StandardLagerortId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StandardLagerortId"]);
			User_Simulieren = (dataRow["User_Simulieren"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["User_Simulieren"]);
			WerkNachId = (dataRow["WerkNachId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WerkNachId"]);
			WerkVonId = (dataRow["WerkVonId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WerkVonId"]);
		}

		public LagerorteEntity ShallowClone()
		{
			return new LagerorteEntity
			{
				Lagerort = Lagerort,
				Lagerort_id = Lagerort_id,
				Simulieren = Simulieren,
				Standard = Standard,
				StandardLagerortId = StandardLagerortId,
				User_Simulieren = User_Simulieren,
				WerkNachId = WerkNachId,
				WerkVonId = WerkVonId
			};
		}
	}
}

