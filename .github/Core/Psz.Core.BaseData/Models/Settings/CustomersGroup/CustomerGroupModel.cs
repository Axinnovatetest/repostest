namespace Psz.Core.BaseData.Models.CustomersGroup
{
	public class CustomerGroupModel
	{
		public int Id { get; set; }//ID
		public string GroupName { get; set; } // Lieferantengruppe
		public CustomerGroupModel()
		{

		}
		public CustomerGroupModel(Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity pszKundengruppenEntity)
		{
			Id = pszKundengruppenEntity.ID;
			GroupName = pszKundengruppenEntity.Kundengruppe;
		}

		public Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity
			{
				ID = Id,
				Kundengruppe = GroupName,
			};
		}
	}
}
