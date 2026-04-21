namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class ArticleContactCSModel
	{
		public int Id { get; set; }
		public string Username { get; set; }

		public string Email { get; set; }
		public string Name { get; set; }
		public int UserId { get; set; }
		public ArticleContactCSModel(Infrastructure.Data.Entities.Tables.BSD.ArticleContactCSEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
			Username = entity.Username;
			Email = entity.Email;
			UserId = entity.UserId ?? -1;
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArticleContactCSEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleContactCSEntity
			{
				Id = Id,
				Email = Email,
				Name = Name,
				UserId = UserId,
				Username = Username
			};
		}
	}
}
