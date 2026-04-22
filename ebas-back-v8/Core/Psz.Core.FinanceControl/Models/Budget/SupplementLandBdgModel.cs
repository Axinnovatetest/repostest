namespace Psz.Core.FinanceControl.Models.Budget
{
	public class SupplementLandBdgModel
	{

		public int ID { get; set; }
		public string Land_Name { get; set; }
		public double? budget { get; set; }
		public double? SOMME_Supplement_Land_Budget { get; set; }
		public int? B_year { get; set; }


		public SupplementLandBdgModel()
		{

		}
		public SupplementLandBdgModel(Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity supplement_LandBdg)
		{
			ID = supplement_LandBdg.ID;
			Land_Name = supplement_LandBdg.Land_Name;
			budget = supplement_LandBdg.budget;
			SOMME_Supplement_Land_Budget = supplement_LandBdg.SOMME_Supplement_Land_Budget;
			B_year = supplement_LandBdg.B_year;

		}

		public Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity ToSupplementCheckTest()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity
			{
				ID = ID,
				Land_Name = Land_Name,
				budget = budget,
				SOMME_Supplement_Land_Budget = SOMME_Supplement_Land_Budget,
				B_year = B_year,

			};
		}
	}
}
