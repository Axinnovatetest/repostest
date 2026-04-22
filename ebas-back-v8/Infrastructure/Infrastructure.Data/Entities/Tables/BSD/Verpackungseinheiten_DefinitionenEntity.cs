using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Verpackungseinheiten_DefinitionenEntity
	{
		public string Artikelnummer { get; set; }
		public int Id { get; set; }
		public string Masse_LxBxH__in_mm_ { get; set; }
		public string Packmittel_Karton { get; set; }

		public Verpackungseinheiten_DefinitionenEntity() { }

		public Verpackungseinheiten_DefinitionenEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Id = Convert.ToInt32(dataRow["ID"]);
			Masse_LxBxH__in_mm_ = (dataRow["Masse:LxBxH (in mm)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Masse:LxBxH (in mm)"]);
			Packmittel_Karton = (dataRow["Packmittel/Karton"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Packmittel/Karton"]);
		}
	}
}

