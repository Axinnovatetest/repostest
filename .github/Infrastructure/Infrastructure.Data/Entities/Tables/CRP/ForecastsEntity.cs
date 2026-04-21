using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class ForecastsEntity
	{
		public DateTime? DateCreation { get; set; }
		public DateTime? Datum { get; set; }
		public int Id { get; set; }
		public string kunden { get; set; }
		public int? kundennummer { get; set; }
		public string Type { get; set; }
		public int? TypeId { get; set; }
		public int? UserId { get; set; }
		public int? Version { get; set; }

		public ForecastsEntity() { }

		public ForecastsEntity(DataRow dataRow)
		{
			DateCreation = (dataRow["DateCreation"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateCreation"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			kunden = (dataRow["kunden"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["kunden"]);
			kundennummer = (dataRow["kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["kundennummer"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			TypeId = (dataRow["TypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TypeId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Version = (dataRow["Version"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Version"]);
		}

		public ForecastsEntity ShallowClone()
		{
			return new ForecastsEntity
			{
				DateCreation = DateCreation,
				Datum = Datum,
				Id = Id,
				kunden = kunden,
				kundennummer = kundennummer,
				Type = Type,
				TypeId = TypeId,
				UserId = UserId,
				Version = Version
			};
		}
	}
}

