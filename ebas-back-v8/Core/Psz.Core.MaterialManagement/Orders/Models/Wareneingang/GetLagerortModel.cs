namespace Psz.Core.MaterialManagement.Orders.Models.Wareneingang
{
	public class GetLagerortModel
	{
		public string Name { get; set; }
		public int Id { get; set; }

		public GetLagerortModel(Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity lagerorteEntity)
		{
			Name = lagerorteEntity.Lagerort;
			Id = lagerorteEntity.Lagerort_id;
		}
	}
}
