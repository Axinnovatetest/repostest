using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Roh_Artikelnummer_Level3Entity
	{
		public int Id { get; set; }
		public int? IdLevelOne { get; set; }
		public int? IdLevelTwo { get; set; }
		public bool? IncludeInDescription { get; set; }
		public bool? IsFreeText { get; set; }
		public string Name { get; set; }
		public string Part { get; set; }
		public int? PartOrder { get; set; }

		public Roh_Artikelnummer_Level3Entity() { }

		public Roh_Artikelnummer_Level3Entity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			IdLevelOne = (dataRow["IdLevelOne"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdLevelOne"]);
			IdLevelTwo = (dataRow["IdLevelTwo"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdLevelTwo"]);
			IncludeInDescription = (dataRow["IncludeInDescription"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IncludeInDescription"]);
			IsFreeText = (dataRow["IsFreeText"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsFreeText"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Part = (dataRow["Part"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Part"]);
			PartOrder = (dataRow["PartOrder"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PartOrder"]);
		}

		public Roh_Artikelnummer_Level3Entity ShallowClone()
		{
			return new Roh_Artikelnummer_Level3Entity
			{
				Id = Id,
				IdLevelOne = IdLevelOne,
				IdLevelTwo = IdLevelTwo,
				IncludeInDescription = IncludeInDescription,
				IsFreeText = IsFreeText,
				Name = Name,
				Part = Part,
				PartOrder = PartOrder
			};
		}
	}
}

