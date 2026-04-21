namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetStateModel
	{
		public int IdS { get; set; }
		public string State { get; set; }


		public GetStateModel() { }

		public GetStateModel(Infrastructure.Data.Entities.Tables.FNC.State_BudgetEntity budget_StatesEntity)


		{
			IdS = budget_StatesEntity.IdS;
			State = budget_StatesEntity.State;


		}
		public Infrastructure.Data.Entities.Tables.FNC.State_BudgetEntity ToBudgetStates()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.State_BudgetEntity
			{
				IdS = IdS,
				State = State

			};
		}
	}
}
