using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class PSZ_BearbeiterEntity
	{
		public string AccessName { get; set; }
		public string Email { get; set; }
		public string Faxnummer { get; set; }
		public int ID { get; set; }
		public string LandDisponent { get; set; }
		public string Name { get; set; }
		public int? Nummer { get; set; }
		public string Telefonnummer { get; set; }

		public PSZ_BearbeiterEntity() { }

		public PSZ_BearbeiterEntity(DataRow dataRow)
		{
			AccessName = (dataRow["AccessName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessName"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			Faxnummer = (dataRow["Faxnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Faxnummer"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			LandDisponent = (dataRow["LandDisponent"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LandDisponent"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Nummer = (dataRow["Nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nummer"]);
			Telefonnummer = (dataRow["Telefonnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefonnummer"]);
		}

		public PSZ_BearbeiterEntity ShallowClone()
		{
			return new PSZ_BearbeiterEntity
			{
				AccessName = AccessName,
				Email = Email,
				Faxnummer = Faxnummer,
				ID = ID,
				LandDisponent = LandDisponent,
				Name = Name,
				Nummer = Nummer,
				Telefonnummer = Telefonnummer
			};
		}
	}
}

