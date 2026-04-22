namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class CRP_FA_EmailUsersEntity
	{
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public int Id { get; set; }
		public int? UserId { get; set; }
		public string UserName { get; set; }

		public CRP_FA_EmailUsersEntity() { }

		public CRP_FA_EmailUsersEntity(DataRow dataRow)
		{
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}

		public CRP_FA_EmailUsersEntity ShallowClone()
		{
			return new CRP_FA_EmailUsersEntity
			{
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				Id = Id,
				UserId = UserId,
				UserName = UserName
			};
		}
	}
}
