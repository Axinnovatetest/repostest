using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Roh_Artikelnummer_Level2Entity
	{
		public int Id { get; set; }
		public int? IdLevelOne { get; set; }
		public bool? ImpactNumberGeneration { get; set; }
		public bool? IncludeInDescription { get; set; }
		public bool? IsFreeText { get; set; }
		public bool? IsRange { get; set; }
		public string Name { get; set; }
		public int? OrderInDescription { get; set; }
		public string Part { get; set; }
		public int? PartOrder { get; set; }
		public string Prefix { get; set; }
		public bool? Required { get; set; }
		public string Sepertor { get; set; }
		public string Suffix { get; set; }

		public Roh_Artikelnummer_Level2Entity() { }

		public Roh_Artikelnummer_Level2Entity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			IdLevelOne = (dataRow["IdLevelOne"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdLevelOne"]);
			ImpactNumberGeneration = (dataRow["ImpactNumberGeneration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ImpactNumberGeneration"]);
			IncludeInDescription = (dataRow["IncludeInDescription"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IncludeInDescription"]);
			IsFreeText = (dataRow["IsFreeText"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsFreeText"]);
			IsRange = (dataRow["IsRange"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsRange"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			OrderInDescription = (dataRow["OrderInDescription"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderInDescription"]);
			Part = (dataRow["Part"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Part"]);
			PartOrder = (dataRow["PartOrder"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PartOrder"]);
			Prefix = (dataRow["Prefix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prefix"]);
			Required = (dataRow["Required"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Required"]);
			Sepertor = (dataRow["Sepertor"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sepertor"]);
			Suffix = (dataRow["Suffix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Suffix"]);
		}

		public Roh_Artikelnummer_Level2Entity ShallowClone()
		{
			return new Roh_Artikelnummer_Level2Entity
			{
				Id = Id,
				IdLevelOne = IdLevelOne,
				ImpactNumberGeneration = ImpactNumberGeneration,
				IncludeInDescription = IncludeInDescription,
				IsFreeText = IsFreeText,
				IsRange = IsRange,
				Name = Name,
				OrderInDescription = OrderInDescription,
				Part = Part,
				PartOrder = PartOrder,
				Prefix = Prefix,
				Required = Required,
				Sepertor = Sepertor,
				Suffix = Suffix
			};
		}
	}
}