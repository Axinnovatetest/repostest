namespace Psz.Core.FinanceControl.Models.Budget
{
	public class Land_Responsable_JointModel
	{
		public int ID { get; set; }
		public int? ID_Land { get; set; }
		public int? ID_user { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }

		public Land_Responsable_JointModel() { }
		public Land_Responsable_JointModel(Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity budget_SuppLandsEntity)
		{
			ID = budget_SuppLandsEntity.ID;
			ID_Land = budget_SuppLandsEntity.ID_Land;
			ID_user = budget_SuppLandsEntity.ID_user;
			Username = budget_SuppLandsEntity.Username;
			Name = budget_SuppLandsEntity.Name;

		}
		public Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity ToBudgetSuppLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity
			{
				ID = ID,
				ID_Land = ID_Land,
				ID_user = ID_user,
				Username = Username,
				Name = Name,

			};
		}
	}
}
