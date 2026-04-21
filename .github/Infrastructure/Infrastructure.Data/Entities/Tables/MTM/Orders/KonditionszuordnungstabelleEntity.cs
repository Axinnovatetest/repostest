using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class KonditionszuordnungstabelleEntity
	{
		public string Bemerkung { get; set; }
		public int? Nettotage { get; set; }
		public int Nr { get; set; }
		public Single? Skonto { get; set; }
		public int? Skontotage { get; set; }
		public string Text { get; set; }

		public KonditionszuordnungstabelleEntity() { }

		public KonditionszuordnungstabelleEntity(DataRow dataRow)
		{
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Nettotage = (dataRow["Nettotage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nettotage"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Skonto = (dataRow["Skonto"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Skonto"]);
			Skontotage = (dataRow["Skontotage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Skontotage"]);
			Text = (dataRow["Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text"]);
		}

		public KonditionszuordnungstabelleEntity ShallowClone()
		{
			return new KonditionszuordnungstabelleEntity
			{
				Bemerkung = Bemerkung,
				Nettotage = Nettotage,
				Nr = Nr,
				Skonto = Skonto,
				Skontotage = Skontotage,
				Text = Text
			};
		}
	}
}

