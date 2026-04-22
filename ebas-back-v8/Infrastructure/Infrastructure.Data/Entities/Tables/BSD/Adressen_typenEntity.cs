using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Adressen_typenEntity
	{
		public string Bezeichnung { get; set; }
		public int ID_typ { get; set; }

		public Adressen_typenEntity() { }

		public Adressen_typenEntity(DataRow dataRow)
		{
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			ID_typ = Convert.ToInt32(dataRow["ID_typ"]);
		}
	}
}

