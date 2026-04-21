namespace Psz.Core.BaseData.Models.Article.Configuration.EmailNotifications
{
	public class GetUserModel
	{
		public bool? ArticleBomCpControl_Engineering { get; set; }
		public bool? ArticleBomCpControl_Quality { get; set; }
		public bool? ArticlePurchase { get; set; }
		public bool? ArticleSales { get; set; }
		public long Id { get; set; }
		public string UserEmail { get; set; }
		public long UserId { get; set; }
		public string UserName { get; set; }
		public GetUserModel(Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity emailNotificationUsersEntity)
		{
			if(emailNotificationUsersEntity == null)
				return;

			ArticleBomCpControl_Engineering = emailNotificationUsersEntity.ArticleBomCpControl_Engineering;
			ArticleBomCpControl_Quality = emailNotificationUsersEntity.ArticleBomCpControl_Quality;
			ArticlePurchase = emailNotificationUsersEntity.ArticlePurchase;
			ArticleSales = emailNotificationUsersEntity.ArticleSales;
			Id = emailNotificationUsersEntity.Id;
			UserEmail = emailNotificationUsersEntity.UserEmail;
			UserId = emailNotificationUsersEntity.UserId;
			UserName = emailNotificationUsersEntity.UserName;
		}
		public Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity ToEntity(Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity emailNotificationUsersEntity)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity
			{
				ArticleBomCpControl_Engineering = (ArticleBomCpControl_Engineering ?? false) || (emailNotificationUsersEntity?.ArticleBomCpControl_Engineering ?? false),
				ArticleBomCpControl_Quality = (ArticleBomCpControl_Quality ?? false) || (emailNotificationUsersEntity?.ArticleBomCpControl_Quality ?? false),
				ArticlePurchase = (ArticlePurchase ?? false) || (emailNotificationUsersEntity?.ArticlePurchase ?? false),
				ArticleSales = (ArticleSales ?? false) || (emailNotificationUsersEntity?.ArticleSales ?? false),
				Id = Id,
				UserEmail = UserEmail,
				UserId = UserId,
				UserName = UserName
			};
		}
	}
}
