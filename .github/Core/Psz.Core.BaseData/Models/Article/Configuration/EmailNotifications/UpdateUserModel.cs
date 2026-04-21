namespace Psz.Core.BaseData.Models.Article.Configuration.EmailNotifications
{
	public class UpdateUserModel
	{
		public long UserId { get; set; }
		public string UserName { get; set; }

		public Enums.ArticleEnums.NotificationOptions NotificationOption { get; set; }
	}
}
