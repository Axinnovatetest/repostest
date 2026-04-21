namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetBudgetTestModel
	{
		public int ID { get; set; }
		public string value_1 { get; set; }
		public string value_2 { get; set; }
		public int? value_3 { get; set; }
		public bool? value_4 { get; set; }

		public GetBudgetTestModel()
		{

		}
		public GetBudgetTestModel(Infrastructure.Data.Entities.Tables.FNC.Budget_testEntity budget_TestEntity)
		{
			ID = budget_TestEntity.ID;
			value_1 = budget_TestEntity.value_1;
			value_2 = budget_TestEntity.value_2;
			value_3 = budget_TestEntity.value_3;
			value_4 = budget_TestEntity.value_4;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_testEntity ToBudgetTest()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_testEntity
			{
				ID = ID,
				value_1 = value_1,
				value_2 = value_2,
				value_3 = value_3,
				value_4 = value_4
			};
		}
	}
}
