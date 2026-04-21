

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class ProjectsOverviewModel
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public string ProjectStatus { get; set; }
		public string State { get; set; }
		public decimal? ProjectBudget { get; set; }
		public string Type { get; set; }
		public ProjectsOverviewModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.ProjectsOverviewEntity entity)
		{
			Id = entity.Id;
			ProjectName = entity.ProjectName;
			State = ((Enums.BudgetEnums.ProjectApprovalStatuses)entity.StatusId).GetDescription();
			ProjectBudget = entity.ProjectBudget;
			ProjectStatus = entity.ProjectStatus;
			Type = entity.Type;
		}
	}
}