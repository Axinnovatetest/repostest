using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSCustomerGroupEntity
	{
		public string Kunde { get; set; }
		public int? Kundennummer { get; set; }
		public string Name { get; set; }
		public int Nr { get; set; }
		public string Stufe { get; set; }

		public CTSCustomerGroupEntity() { }

		public CTSCustomerGroupEntity(DataRow dataRow)
		{
			Kunde = Convert.ToString(dataRow["Kunde"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Stufe = Convert.ToString(dataRow["Stufe"]);
		}

		public CTSCustomerGroupEntity ShallowClone()
		{
			return new CTSCustomerGroupEntity
			{
				Kunde = Kunde,
				Kundennummer = Kundennummer,
				Name = Name,
				Nr = Nr,
				Stufe = Stufe
			};
		}
	}
}
