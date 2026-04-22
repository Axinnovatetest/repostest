using System;
using System.Data;


namespace Infrastructure.Data.Entities.Tables.MTM.Orders
{
	public class Artikelstamm_KlassifizierungEntity
	{
		public string Bezeichnung { get; set; }
		public string Gewerk { get; set; }
		public int ID { get; set; }
		public string Klassifizierung { get; set; }
		public string Kupferzahl { get; set; }
		public string Nummernkreis { get; set; }

		public Artikelstamm_KlassifizierungEntity() { }

		public Artikelstamm_KlassifizierungEntity(DataRow dataRow)
		{
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			Gewerk = (dataRow["Gewerk"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
			Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kupferzahl"]);
			Nummernkreis = (dataRow["Nummernkreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummernkreis"]);
		}

		public Artikelstamm_KlassifizierungEntity ShallowClone()
		{
			return new Artikelstamm_KlassifizierungEntity
			{
				Bezeichnung = Bezeichnung,
				Gewerk = Gewerk,
				ID = ID,
				Klassifizierung = Klassifizierung,
				Kupferzahl = Kupferzahl,
				Nummernkreis = Nummernkreis
			};
		}
	}


	public class Artikelstamm_KlassifizierungTrimmedEntity
	{
		public int ID { get; set; }
		public string Klassifizierung { get; set; }


		public Artikelstamm_KlassifizierungTrimmedEntity() { }

		public Artikelstamm_KlassifizierungTrimmedEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
		}

	}

}
