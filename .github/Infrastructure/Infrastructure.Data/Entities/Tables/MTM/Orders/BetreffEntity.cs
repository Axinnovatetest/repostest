using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class BetreffEntity
	{
		public string Betreff { get; set; }
		public int ID { get; set; }

		public BetreffEntity() { }

		public BetreffEntity(DataRow dataRow)
		{
			Betreff = (dataRow["Betreff"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Betreff"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}

		public BetreffEntity ShallowClone()
		{
			return new BetreffEntity
			{
				Betreff = Betreff,
				ID = ID
			};
		}
	}
}

