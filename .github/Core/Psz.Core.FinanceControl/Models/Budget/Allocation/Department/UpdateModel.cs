using Infrastructure.Data.Entities.Tables.FNC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Models.Budget.Allocation.Department
{
	public class UpdateModel
	{
		public decimal AmountAllocatedToUsers { get; set; }
		public decimal AmountAllocatedToProjects { get; set; }
		public decimal AmountInitial { get; set; }
		public decimal AmountSpent { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int Year { get; set; }

		public int CompanyId { get; set; }
		public string CompanyName { get; set; }

		public decimal AmountNotificationThreshold { get; set; }
		public int NbLeasing { get; set; }
		public decimal AmountLeasing { get; set; }
		public decimal AmountFix { get; set; }
		public decimal AmountInvest { get; set; }
		public bool IsFreezed { get; set; }
		public UpdateModel(
			Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationDepartmentEntity budgetAllocation,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> leasingEntities)
		{
			if(budgetAllocation == null)
				return;

			AmountAllocatedToUsers = budgetAllocation.AmountAllocatedToUsers;
			AmountAllocatedToProjects = budgetAllocation.AmountAllocatedToProjects ?? 0;
			AmountNotificationThreshold = budgetAllocation.AmountNotificationThreshold ?? 0;
			AmountInitial = budgetAllocation.AmountInitial;
			AmountSpent = budgetAllocation.AmountSpent;
			CreationTime = budgetAllocation.CreationTime;
			CreationUserId = budgetAllocation.CreationUserId;
			DepartmentId = budgetAllocation.DepartmentId;
			DepartmentName = budgetAllocation.DepartmentName;
			Id = budgetAllocation.Id;
			LastEditTime = budgetAllocation.LastEditTime;
			LastEditUserId = budgetAllocation.LastEditUserId;
			Year = budgetAllocation.Year;
			AmountFix = budgetAllocation.AmountFix;
			AmountInvest = budgetAllocation.AmountInvest;

			CompanyId = companyEntity?.Id ?? -1;
			CompanyName = companyEntity?.Name;
			// - 
			NbLeasing = leasingEntities?.Count ?? 0;
			AmountLeasing = leasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, budgetAllocation.Year)).Sum() ?? 0;
			IsFreezed = budgetAllocation.LastFreezeTime.HasValue
				? true
				: false;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationDepartmentEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationDepartmentEntity
			{
				AmountAllocatedToUsers = AmountAllocatedToUsers,
				AmountAllocatedToProjects = AmountAllocatedToProjects,
				AmountInitial = AmountInitial,
				AmountNotificationThreshold = AmountNotificationThreshold,
				AmountSpent = AmountSpent,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				DepartmentId = DepartmentId,
				DepartmentName = DepartmentName,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				Year = Year,
				AmountFix = AmountFix,
				AmountInvest = AmountInvest,
			};
		}
	}
}
