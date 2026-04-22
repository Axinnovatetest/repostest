namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetBudgetUsersModel
	{

		public string departement_user { get; set; }
		public int ID { get; set; }
		public string land { get; set; }
		public string username { get; set; }
		public int? U_year { get; set; }
		public decimal? TotalSpent { get; set; }
		public int? LandId { get; set; }
		public int? DepartmentId { get; set; }
		public int? UserId { get; set; }
		public double? budget_month { get; set; }
		public double? budget_order { get; set; }
		public double? budget_year { get; set; }


		public GetBudgetUsersModel() { }

		public GetBudgetUsersModel(Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity budget_UsersEntity)
		{
			//ID = budget_UsersEntity.Id;
			//username = budget_UsersEntity.UserName;
			//land = budget_UsersEntity.Com;
			//departement_user = budget_UsersEntity.departement_user;
			//budget_year = budget_UsersEntity.budget_year;
			//budget_month = budget_UsersEntity.budget_month;
			//budget_order = budget_UsersEntity.budget_order;
			//U_year = budget_UsersEntity.U_year;
			//TotalSpent = budget_UsersEntity.TotalSpent;
			//UserId = budget_UsersEntity.UserId;
			//LandId = budget_UsersEntity.LandId;
			//DepartmentId = budget_UsersEntity.DepartmentId;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity ToBudgetUsers()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity
			{
				//ID = ID,
				//username = username,
				//land = land,
				//departement_user= departement_user,
				//budget_year=budget_year,
				//budget_month=budget_month,
				//budget_order=budget_order,
				//U_year=U_year,
				//TotalSpent = TotalSpent,
				//UserId = UserId,
				//LandId = LandId,
				//DepartmentId = DepartmentId,
			};
		}
	}
}
