namespace Psz.Core.Apps.EDI.Models.Delfor.Production
{
	public class ManufacturingFacilityModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ManufacturingFacilityModel(Infrastructure.Data.Entities.Tables.INV.LagerorteEntity lagerorteEntity)
		{
			Id = lagerorteEntity.LagerortId;
			Name = lagerorteEntity.Lagerort;
		}
		public Infrastructure.Data.Entities.Tables.INV.LagerorteEntity ToLagerorteEntity()
		{
			return new Infrastructure.Data.Entities.Tables.INV.LagerorteEntity
			{
				LagerortId = Id,
				Lagerort = Name,
				Standard = null
			};
		}
	}
}
