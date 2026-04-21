using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class GrundeEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public GrundeEntity() { }
		public GrundeEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Id"]);
			Name = (dataRow["Name"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Name"]);
		}
	}
}

