using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class TeamsEntity
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public int? SiteId { get; set; }
		public string SitePrefix { get; set; }
		public char? TeamCategory { get; set; }
		public int? TeamIndex { get; set; }

		public TeamsEntity() { }

		public TeamsEntity(DataRow dataRow)
		{
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			SiteId = (dataRow["SiteId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SiteId"]);
			SitePrefix = (dataRow["SitePrefix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SitePrefix"]);
			TeamCategory = (dataRow["TeamCategory"] == System.DBNull.Value) ? (char?)null : Convert.ToChar(dataRow["TeamCategory"]);
			TeamIndex = (dataRow["TeamIndex"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TeamIndex"]);
		}

		public TeamsEntity ShallowClone()
		{
			return new TeamsEntity
			{
				Description = Description,
				Id = Id,
				Name = Name,
				SiteId = SiteId,
				SitePrefix = SitePrefix,
				TeamCategory = TeamCategory,
				TeamIndex = TeamIndex
			};
		}
	}
}

