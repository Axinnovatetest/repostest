namespace Psz.Core.Models.Main.ActiveDirectory.Groups
{
	public class GetModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string GroupName { get; set; }
		public string Email { get; set; }
		public GetModel() { }
		public GetModel(Psz.Core.Tools.ActiveDirectoryManager.GroupModel groupModel)
		{
			Id = -1;
			Name = groupModel.Name;
			GroupName = groupModel.Groupname;
			Email = groupModel.Email;
		}
	}
}
