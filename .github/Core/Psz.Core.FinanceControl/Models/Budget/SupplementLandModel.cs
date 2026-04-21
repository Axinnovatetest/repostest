using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class SupplementLandModel
	{
		public int Id { get; set; }
		public int Id_AL { get; set; }
		public double? Supplement_Budget { get; set; }
		public DateTime Creation_Date { get; set; }

		public SupplementLandModel() { }
		public SupplementLandModel(Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity budget_SuppLandsEntity)
		{
			Id = budget_SuppLandsEntity.Id;
			Id_AL = budget_SuppLandsEntity.Id_AL;
			Supplement_Budget = budget_SuppLandsEntity.Supplement_Budget;
			Creation_Date = budget_SuppLandsEntity.Creation_Date;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity ToBudgetSuppLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity
			{
				Id = Id,
				Id_AL = Id_AL,
				Supplement_Budget = Supplement_Budget,
				Creation_Date = Creation_Date,
			};
		}
	}
}
