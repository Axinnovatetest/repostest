namespace Psz.Core.Models.Main.ActiveDirectory.Groups
{
	public class UserModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public UserModel() { }
		public UserModel(Psz.Core.Tools.ActiveDirectoryManager.UserModel userModel)
		{
			Id = -1;
			FirstName = userModel.FirstName;
			LastName = userModel.LastName;
			UserName = userModel.Username;
			Email = userModel.Email;
		}
	}
}
