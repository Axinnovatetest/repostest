using System;

namespace Psz.Core.Apps.Settings.Models.Bearbeiter
{
	public class BearbeiterEditRequestModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public string Country { get; set; }
		public string Email { get; set; }
		public int Nummer { get; set; }
		public BearbeiterEditRequestModel() { }
		public Infrastructure.Data.Entities.Tables.MTM.PSZ_BearbeiterEntity ToEntity()
		{

			var entity = Infrastructure.Data.Access.Tables.MTM.PSZ_BearbeiterAccess.Get(Id);

			if(entity == null)
				return null;

			entity.Name = Name;
			entity.AccessName = Username;
			entity.LandDisponent = Country;
			entity.Nummer = Nummer;
			entity.Email = Email;
			return entity;
		}
		public BearbeiterEditRequestModel(Infrastructure.Data.Entities.Tables.MTM.PSZ_BearbeiterEntity pSZ_BearbeiterEntity)
		{

		}

	}


	public class BearbeiterEditResponseModel
	{
		public Boolean Success { get; set; }
	}
}
