using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class ProjectsTop5StatsModel
	{
		public IEnumerable<Top5Model> BiggectAllocations { get; set; }
		public IEnumerable<Top5Model> BiggectOrdersAmount { get; set; }
		public IEnumerable<Top5Model2> OldestApproval { get; set; }
		public IEnumerable<Top5Model> MostProfitable { get; set; }
		public IEnumerable<OverOrUnderBudgetModel> Overbudgeted { get; set; }
		public IEnumerable<OverOrUnderBudgetModel> BudgetLeaks { get; set; }
	}

	public class Top5Model
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public decimal Amount { get; set; }
		public Top5Model(Tuple<int, string, decimal> entity)
		{
			Id = entity.Item1;
			ProjectName = entity.Item2;
			Amount = entity.Item3;
		}
	}
	public class Top5Model2
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public DateTime OldestApproval { get; set; }
		public Top5Model2(Tuple<int, string, DateTime> entity)
		{
			Id = entity.Item1;
			ProjectName = entity.Item2;
			OldestApproval = entity.Item3;
		}
	}
	public class OverOrUnderBudgetModel
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public decimal? ProjectBudget { get; set; }
		public decimal? OrdersAmount { get; set; }
		public decimal? Diffrence { get; set; }
		public OverOrUnderBudgetModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.OverbudgetedProjectsEntity entity)
		{
			Id = entity.Id;
			ProjectName = entity.ProjectName;
			ProjectBudget = entity.ProjectBudget;
			OrdersAmount = entity.OrdersAmount;
			Diffrence = entity.Diffrence;
		}
	}
}
