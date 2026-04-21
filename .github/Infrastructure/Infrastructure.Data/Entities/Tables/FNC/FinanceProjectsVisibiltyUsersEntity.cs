using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class FinanceProjectsVisibiltyUsersEntity
	{
		public DateTime? DateTimeAdd { get; set; }
		public int Id { get; set; }
		public int? ProjectId { get; set; }
		public int? UserAddId { get; set; }
		public int? UserId { get; set; }

		public FinanceProjectsVisibiltyUsersEntity() { }

		public FinanceProjectsVisibiltyUsersEntity(DataRow dataRow)
		{
			DateTimeAdd = (dataRow["DateTimeAdd"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateTimeAdd"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProjectId = (dataRow["ProjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectId"]);
			UserAddId = (dataRow["UserAddId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserAddId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public FinanceProjectsVisibiltyUsersEntity ShallowClone()
		{
			return new FinanceProjectsVisibiltyUsersEntity
			{
				DateTimeAdd = DateTimeAdd,
				Id = Id,
				ProjectId = ProjectId,
				UserAddId = UserAddId,
				UserId = UserId
			};
		}
	}
}

