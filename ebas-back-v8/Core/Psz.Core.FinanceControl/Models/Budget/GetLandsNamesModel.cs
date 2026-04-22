namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetLandsNamesModel
	{
		public int ID { get; set; }
		public string Land_name { get; set; }

		public GetLandsNamesModel() { }
		public GetLandsNamesModel(Infrastructure.Data.Entities.Tables.FNC.Budget_landsNamesEntity budget_LandsEntity)
		{
			Land_name = budget_LandsEntity.Land_name;
			ID = budget_LandsEntity.ID;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_landsNamesEntity ToBudgetLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_landsNamesEntity
			{
				Land_name = Land_name,
				ID = ID,
			};
		}
	}
}
