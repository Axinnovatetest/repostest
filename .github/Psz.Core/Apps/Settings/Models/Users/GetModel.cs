namespace Psz.Core.Apps.Settings.Models.Users
{
	public class GetModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string TelephoneMobile { get; set; }
		public string TelephoneHome { get; set; }
		public string TelephoneIP { get; set; }
		public string Fax { get; set; }
		public GetModel()
		{

		}
		public GetModel(Infrastructure.Data.Entities.Tables.COR.UserEntity entity)
		{
			Id = entity.Id;
			Username = entity?.Username;
			Name = entity?.Name;
			Email = entity?.Email;
		}

		public GetModel(Tools.ActiveDirectoryManager.UserModel userModel)
		{
			Id = -1;
			Username = userModel?.Username;
			Name = $"{userModel?.LastName}"?.Trim(); // - {userModel?.FirstName} 
			Email = userModel?.Email;
			TelephoneMobile = userModel?.TelephoneMobile;
			TelephoneHome = userModel?.TelephoneHome;
			TelephoneIP = userModel?.TelephoneIP;
			Fax = userModel?.Fax;
		}
	}
}
