using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class WarengruppenEntity
	{
		public string Bezeichnung { get; set; }
		public string Hinweis { get; set; }
		public int ID { get; set; }
		public string Warengruppe { get; set; }

		public WarengruppenEntity() { }

		public WarengruppenEntity(DataRow dataRow)
		{
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			Hinweis = (dataRow["Hinweis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Hinweis"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
		}

		public WarengruppenEntity ShallowClone()
		{
			return new WarengruppenEntity
			{
				Bezeichnung = Bezeichnung,
				Hinweis = Hinweis,
				ID = ID,
				Warengruppe = Warengruppe
			};
		}
	}
}
