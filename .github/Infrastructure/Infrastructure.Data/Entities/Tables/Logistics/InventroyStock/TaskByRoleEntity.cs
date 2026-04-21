namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class TaskByRoleEntity
	{
		public int Id { get; set; }
		public int? lagerId { get; set; }
		public string phase { get; set; }
		public string role { get; set; }
		public int status { get; set; }
		public string title { get; set; }

		public TaskByRoleEntity() { }

		public TaskByRoleEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			lagerId = (dataRow["lagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["lagerId"]);
			phase = Convert.ToString(dataRow["phase"]);
			role = Convert.ToString(dataRow["role"]);
			status = Convert.ToInt32(dataRow["status"]);
			title = Convert.ToString(dataRow["title"]);
		}

		public TaskByRoleEntity ShallowClone()
		{
			return new TaskByRoleEntity
			{
				Id = Id,
				lagerId = lagerId,
				phase = phase,
				role = role,
				status = status,
				title = title
			};
		}
	}
}
