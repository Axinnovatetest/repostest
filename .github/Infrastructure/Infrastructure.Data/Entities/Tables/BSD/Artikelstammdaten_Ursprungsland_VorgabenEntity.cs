using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Artikelstammdaten_Ursprungsland_VorgabenEntity
	{
		public string Hinweis { get; set; }
		public int ID { get; set; }
		public string Land { get; set; }

		public Artikelstammdaten_Ursprungsland_VorgabenEntity() { }

		public Artikelstammdaten_Ursprungsland_VorgabenEntity(DataRow dataRow)
		{
			Hinweis = (dataRow["Hinweis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Hinweis"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Land = Convert.ToString(dataRow["Land"]);
		}

		public Artikelstammdaten_Ursprungsland_VorgabenEntity ShallowClone()
		{
			return new Artikelstammdaten_Ursprungsland_VorgabenEntity
			{
				Hinweis = Hinweis,
				ID = ID,
				Land = Land
			};
		}
	}
}

