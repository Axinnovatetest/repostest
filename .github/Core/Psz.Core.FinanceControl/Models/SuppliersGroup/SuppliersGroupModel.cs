namespace Psz.Core.FinanceControl.Models.SuppliersGroup
{
	public class SuppliersGroupModel
	{
		public int Id { get; set; }
		public string GroupName { get; set; } // Lieferantengruppe
		public SuppliersGroupModel()
		{

		}
		public SuppliersGroupModel(Infrastructure.Data.Entities.Tables.FNC.PszLieferantengruppenEntity pszLieferantengruppenEntity)
		{
			Id = pszLieferantengruppenEntity.ID;
			GroupName = pszLieferantengruppenEntity.Lieferantengruppe;
		}
	}
}
