using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArticleContactTechnicEntity
	{
		public string Email { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public ArticleContactTechnicEntity() { }

		public ArticleContactTechnicEntity(DataRow dataRow)
		{
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}
	}
}

