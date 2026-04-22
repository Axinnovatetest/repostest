using System;
using System.Collections.Generic;


namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class PorjectsMonthlyViewStats
	{
		public IEnumerable<PorjectsMonthlyStats> ProjectsMonthlyAll { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByStatus> ProjectsMonthlyPending { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByStatus> ProjectsMonthlyApproved { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByStatus> ProjectsMonthlyRejected { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByApprovalStatus> ProjectsMonthlySuspended { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByApprovalStatus> ProjectsMonthlyActive { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByApprovalStatus> ProjectsMonthlyClosed { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByType> ProjectsMonthlyInternal { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByType> ProjectsMonthlyExternal { get; set; }
		public IEnumerable<PorjectsMonthlyStatsByType> ProjectsMonthlyFinance { get; set; }

	}
	public class PorjectsMonthlyStats
	{
		public string Month { get; set; }
		public int Count { get; set; }
		public PorjectsMonthlyStats()
		{

		}
		public PorjectsMonthlyStats(KeyValuePair<string, int> entity)
		{
			Month = entity.Key;
			Count = entity.Value;
		}
	}
	public class PorjectsMonthlyStatsByStatus
	{
		public string Month { get; set; }
		public string Status { get; set; }
		public int Count { get; set; }
		public PorjectsMonthlyStatsByStatus()
		{

		}
		public PorjectsMonthlyStatsByStatus(Tuple<string, int, int> entity)
		{
			Month = entity.Item1;
			Status = ((Enums.BudgetEnums.ProjectApprovalStatuses)entity.Item2).GetDescription();
			Count = entity.Item3;
		}
	}
	public class PorjectsMonthlyStatsByApprovalStatus
	{
		public string Month { get; set; }
		public string ApprovalStatus { get; set; }
		public int Count { get; set; }
		public PorjectsMonthlyStatsByApprovalStatus()
		{

		}
		public PorjectsMonthlyStatsByApprovalStatus(Tuple<string, string, int> entity)
		{
			Month = entity.Item1;
			ApprovalStatus = entity.Item2;
			Count = entity.Item3;
		}
	}
	public class PorjectsMonthlyStatsByType
	{
		public string Month { get; set; }
		public string Type { get; set; }
		public int Count { get; set; }
		public PorjectsMonthlyStatsByType()
		{

		}
		public PorjectsMonthlyStatsByType(Tuple<string, string, int> entity)
		{
			Month = entity.Item1;
			Type = entity.Item2;
			Count = entity.Item3;
		}
	}
}