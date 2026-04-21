namespace Psz.Core.Apps.Settings.Models.Bearbeiter
{
	public class BearbeiterResponseModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public string Country { get; set; }
		public string Email { get; set; }
		public int Nummer { get; set; }
		public BearbeiterResponseModel() { }
		public BearbeiterResponseModel(Infrastructure.Data.Entities.Tables.MTM.PSZ_BearbeiterEntity pSZ_BearbeiterEntity)
		{
			Id = pSZ_BearbeiterEntity.ID;
			Name = pSZ_BearbeiterEntity.Name;
			Username = pSZ_BearbeiterEntity.AccessName;
			Country = pSZ_BearbeiterEntity.LandDisponent;
			Nummer = pSZ_BearbeiterEntity.Nummer.HasValue ? pSZ_BearbeiterEntity.Nummer.Value : 0;
			Email = pSZ_BearbeiterEntity.Email;
		}
	}


	public class BearbeiterRequestModel
	{
		public int Id { get; set; }
	}
}
