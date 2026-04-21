namespace Psz.Core.BaseData.Models.Article.ManagerUser
{
	public class ManagerUserModel
	{
		public int ArtikelNr { get; set; }
		public int Id { get; set; }
		public string UserFullName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Artikelnummer { get; set; }
		public ManagerUserModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity artikelManagerUserEntity)
		{
			if(artikelManagerUserEntity == null)
				return;

			Id = artikelManagerUserEntity.Id;
			ArtikelNr = artikelManagerUserEntity.ArtikelNr;
			UserId = artikelManagerUserEntity.UserId;
			UserName = artikelManagerUserEntity.UserName;
			UserFullName = artikelManagerUserEntity.UserFullName;
		}

		public Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity
			{
				Id = Id,
				ArtikelNr = ArtikelNr,
				UserId = UserId,
				UserName = UserName,
				UserFullName = UserFullName
			};
		}
	}
}
