using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Models.Budget.Allocation.Company
{
	public class UpdateModel
	{
		public decimal AmountLeasing { get; set; }
		public int NbLeasing { get; set; }
		public decimal AmountAllocatedToDepartments { get; set; }
		public decimal AmountAllocatedToProjects { get; set; }
		public decimal AmountSupplements { get; set; }
		public decimal AmountInitial { get; set; }
		public decimal AmountSpent { get; set; }
		public string ComapnyName { get; set; }
		public int CompanyId { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int Year { get; set; }
		public decimal AmountNotificationThreshold { get; set; }
		public decimal AmountFix { get; set; }
		public decimal AmountInvest { get; set; }
		//souilmi 11/06/2024
		public decimal AmountNotificationSuperValidationThreshold { get; set; }
		public bool IsFreezed { get; set; }

		public UpdateModel(
			Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationCompanyEntity budgetAllocationCompanyEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> leasingEntities)
		{
			if(budgetAllocationCompanyEntity == null)
				return;

			AmountAllocatedToDepartments = budgetAllocationCompanyEntity.AmountAllocatedToDepartments;
			AmountAllocatedToProjects = budgetAllocationCompanyEntity.AmountAllocatedToProjects ?? 0;
			AmountSupplements = budgetAllocationCompanyEntity.AmountSupplements ?? 0;
			AmountNotificationThreshold = budgetAllocationCompanyEntity.AmountNotificationThreshold ?? 0;
			AmountInitial = budgetAllocationCompanyEntity.AmountInitial;
			AmountSpent = budgetAllocationCompanyEntity.AmountSpent;
			ComapnyName = budgetAllocationCompanyEntity.ComapnyName;
			CompanyId = budgetAllocationCompanyEntity.CompanyId;
			CreationTime = budgetAllocationCompanyEntity.CreationTime;
			CreationUserId = budgetAllocationCompanyEntity.CreationUserId;
			Id = budgetAllocationCompanyEntity.Id;
			LastEditTime = budgetAllocationCompanyEntity.LastEditTime;
			LastEditUserId = budgetAllocationCompanyEntity.LastEditUserId;
			Year = budgetAllocationCompanyEntity.Year;
			AmountFix = budgetAllocationCompanyEntity.AmountFix;
			AmountInvest = budgetAllocationCompanyEntity.AmountInvest;
			// - 
			NbLeasing = leasingEntities?.Count ?? 0;
			AmountLeasing = leasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, budgetAllocationCompanyEntity.Year)).Sum() ?? 0;
			//
			AmountNotificationSuperValidationThreshold = budgetAllocationCompanyEntity.AmountNotificationSuperValidationThreshold ?? 0;
			IsFreezed = budgetAllocationCompanyEntity.LastFreezeTime.HasValue
				? true
				: false;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationCompanyEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationCompanyEntity
			{
				Id = Id,
				AmountAllocatedToDepartments = AmountAllocatedToDepartments,
				AmountAllocatedToProjects = AmountAllocatedToProjects,
				AmountSupplements = AmountSupplements,
				AmountNotificationThreshold = AmountNotificationThreshold,
				AmountInitial = AmountInitial,
				AmountSpent = AmountSpent,
				ComapnyName = ComapnyName,
				CompanyId = CompanyId,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				Year = Year,
				AmountFix = AmountFix,
				AmountInvest = AmountInvest,
				//
				AmountNotificationSuperValidationThreshold = AmountNotificationSuperValidationThreshold,
			};
		}
	}
}
