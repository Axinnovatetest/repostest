using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Roh_Artikelnummer_Level1Entity
	{
		public int? ClassificationId { get; set; }
		public int Id { get; set; }
		public bool? IncludeInDescription { get; set; }
		public string Name { get; set; }
		public int? OrderInDescription { get; set; }
		public string Part { get; set; }
		public int? PartOrder { get; set; }
		public string Seperator { get; set; }
		public string ValueAtBeginningOfDescription { get; set; }
		public string ValueInDescription { get; set; }

		public Roh_Artikelnummer_Level1Entity() { }

		public Roh_Artikelnummer_Level1Entity(DataRow dataRow)
		{
			ClassificationId = (dataRow["ClassificationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ClassificationId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IncludeInDescription = (dataRow["IncludeInDescription"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IncludeInDescription"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			OrderInDescription = (dataRow["OrderInDescription"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderInDescription"]);
			Part = (dataRow["Part"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Part"]);
			PartOrder = (dataRow["PartOrder"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PartOrder"]);
			Seperator = (dataRow["Seperator"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Seperator"]);
			ValueAtBeginningOfDescription = (dataRow["ValueAtBeginningOfDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValueAtBeginningOfDescription"]);
			ValueInDescription = (dataRow["ValueInDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValueInDescription"]);
		}

		public Roh_Artikelnummer_Level1Entity ShallowClone()
		{
			return new Roh_Artikelnummer_Level1Entity
			{
				ClassificationId = ClassificationId,
				Id = Id,
				IncludeInDescription = IncludeInDescription,
				Name = Name,
				OrderInDescription = OrderInDescription,
				Part = Part,
				PartOrder = PartOrder,
				Seperator = Seperator,
				ValueAtBeginningOfDescription = ValueAtBeginningOfDescription,
				ValueInDescription = ValueInDescription
			};
		}
	}
}

