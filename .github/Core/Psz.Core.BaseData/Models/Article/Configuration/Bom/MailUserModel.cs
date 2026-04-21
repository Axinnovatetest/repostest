namespace Psz.Core.BaseData.Models.Article.Configuration.Bom
{
	public class MailUserModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserFullName { get; set; }
		public string UserEmail { get; set; }
		public int SiteId { get; set; }
		public string SiteCode { get; set; }
		public string SiteName { get; set; }
		public MailUserModel()
		{

		}
		public MailUserModel(Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity bomMailUsersEntity)
		{
			if(bomMailUsersEntity == null)
				return;

			Id = bomMailUsersEntity.Id;
			SiteId = bomMailUsersEntity.SiteId;
			SiteCode = bomMailUsersEntity.SiteCode;
			SiteName = bomMailUsersEntity.SiteName;
			UserId = bomMailUsersEntity.UserId;
			UserEmail = bomMailUsersEntity.UserEmail;
			UserName = bomMailUsersEntity.UserName;
			UserFullName = bomMailUsersEntity.UserfullName;
		}

		public Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity
			{
				Id = Id,
				SiteId = SiteId,
				SiteCode = SiteCode,
				SiteName = SiteName,
				UserId = UserId,
				UserEmail = UserEmail,
				UserName = UserName,
				UserfullName = UserFullName
			};
		}
	}
}
