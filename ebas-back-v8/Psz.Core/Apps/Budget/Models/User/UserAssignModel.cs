namespace Psz.Core.Apps.Budget.Models.User
{
	public class UserAssignModel
	{
		public int Id { get; set; }
		public string Username { get; set; }

		public UserAssignModel() { }
		public UserAssignModel(Infrastructure.Data.Entities.Tables.COR.UserEntity budget_UserEntity)
		{
			Id = budget_UserEntity.Id;
			Username = budget_UserEntity.Username;

		}

		public Infrastructure.Data.Entities.Tables.COR.UserEntity UserEntity()
		{
			return new Infrastructure.Data.Entities.Tables.COR.UserEntity
			{
				Id = Id,
				Username = Username,
			};
		}
	}
}

