namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class ArticleContactAVModel
	{
		public int Id { get; set; }
		public string Username { get; set; }

		public string Email { get; set; }
		public string Name { get; set; }
		public int UserId { get; set; }
		public ArticleContactAVModel()
		{

		}
		public ArticleContactAVModel(Infrastructure.Data.Entities.Tables.BSD.ArticleContactAVEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
			Username = entity.Username;
			Email = entity.Email;
			UserId = entity.UserId ?? -1;
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArticleContactAVEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleContactAVEntity
			{
				Id = Id,
				Email = Email,
				Name = Name,
				UserId = UserId,
				Username = Username
			};
		}
		// - 
		public ArticleContactAVModel(Infrastructure.Data.Entities.Tables.CTS.AV_Abteilung_MitarbeiterEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.ID;
			Name = entity.AV_Mitarbeiter;
			Username = "";
			Email = entity.EMAIL;
			UserId = -1;
		}
	}
}
