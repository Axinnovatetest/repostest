namespace Psz.Core.BaseData.Models.SuppliersGroup
{
	public class SuppliersGroupModel
	{
		public int Id { get; set; }//ID
		public string GroupName { get; set; } // Lieferantengruppe
		public SuppliersGroupModel()
		{

		}
		public SuppliersGroupModel(Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity pszLieferantengruppenEntity)
		{
			Id = pszLieferantengruppenEntity.ID;
			GroupName = pszLieferantengruppenEntity.Lieferantengruppe;
		}

		public Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity
			{
				ID = Id,
				Lieferantengruppe = GroupName,
			};
		}
	}
}
