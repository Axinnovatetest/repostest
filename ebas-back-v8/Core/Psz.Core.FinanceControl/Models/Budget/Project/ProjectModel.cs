using Infrastructure.Data.Access.Tables.FNC;
using Infrastructure.Data.Entities.Tables.FNC;
using Infrastructure.Data.Entities.Tables.STG;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq.Extensions;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Budget.Project
{
	public class ProjectModel
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public decimal ProjectBudget { get; set; }
		public int? CustomerId { get; set; }
		public string CustomerName { get; set; }
		public int? Kundennummer { get; set; }
		public int? Nr { get; set; }
		public string Ort { get; set; }
		public string CustomerNr { get; set; }
		public int ResponsableId { get; set; }
		public string ResponsableName { get; set; }
		public string ResponsableEmail { get; set; }
		public int StateId { get; set; }
		public string State { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int TypeId { get; set; }
		public string ProjectType { get; set; }
		public string Description { get; set; }
		public DateTime? CreationDate { get; set; }
		public List<Validator.ValidatorModel> Validators { get; set; }
		public decimal? TotalSpent { get; set; }
		public int OrderCount { get; set; }
		public int BudgetYear { get; set; }
		public decimal PSZOffer { get; set; }
		public decimal OrderAmount { get; set; }
		// -
		public int ApprovalUserId { get; set; }
		public string ApprovalUserName { get; set; }
		public string ApprovalUserEmail { get; set; }
		public DateTime? ApprovalTime { get; set; }
		// - 
		public int Status { get; set; }
		public string StatusName { get; set; }
		public int StatusChangeUserId { get; set; }
		public string StatusChangeUserName { get; set; }
		public DateTime? StatusChangeTime { get; set; }
		public List<FileModel> Files { get; set; }

		//-
		public bool? Closed { get; set; }
		public int? ClosedUserId { get; set; }
		public string ClosedUserName { get; set; }
		public DateTime? ClosedTime { get; set; }

		// - 2024-02-17 - Kechiche - Projects are by default visible at department level only 
		public bool SiteLevelVisibility { get; set; } = false;
		public ProjectModel() { }

		public ProjectModel(int status)
		{
			this.Status = status;
		}


		public ProjectModel(
			 List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> fileEntities,
			Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity project_BudgetEntity,
			Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> validators,
			List<Infrastructure.Data.Entities.Tables.COR.UserEntity> validatorNameEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> bestellungenExtensionEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> bestellteArtikelExtensionEntities)
			: this(fileEntities, project_BudgetEntity, adressenEntity, validators, bestellungenExtensionEntities, bestellteArtikelExtensionEntities)
		{
			Validators = validators?.Select(x => new Validator.ValidatorModel(x, validatorNameEntity))?.ToList();
		}

		public ProjectModel(
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> projectFileEntities,
			Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity project_BudgetEntity,
			Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> validators,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> bestellungenExtensionEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> bestellteArtikelExtensionEntities)
		{
			Id = project_BudgetEntity.Id;
			ProjectName = project_BudgetEntity.ProjectName;
			ProjectBudget = project_BudgetEntity.ProjectBudget;
			CustomerId = project_BudgetEntity.CustomerId == null ? -1 : project_BudgetEntity.CustomerId;
			CustomerName = project_BudgetEntity?.CustomerName;
			Kundennummer = adressenEntity?.Kundennummer;
			TotalSpent = project_BudgetEntity.TotalSpent ?? 0;
			//Nr = project_BudgetEntity?.CustomerNr;
			//Ort = adressenEntity?.Ort;
			CustomerNr = project_BudgetEntity?.CustomerNr;
			ResponsableId = project_BudgetEntity.ResponsableId;
			ResponsableName = project_BudgetEntity?.ResponsableName;
			ResponsableEmail = project_BudgetEntity?.ResponsableEmail;
			StateId = project_BudgetEntity.Id_State;
			State = ((Enums.BudgetEnums.ProjectApprovalStatuses)project_BudgetEntity.Id_State).GetDescription(); // project_BudgetEntity?.sta;
			CurrencyId = project_BudgetEntity?.CurrencyId;
			CurrencyName = project_BudgetEntity?.CurrencyName;
			CompanyId = project_BudgetEntity.CompanyId;
			CompanyName = project_BudgetEntity?.CompanyName;
			DepartmentId = project_BudgetEntity.DepartmentId == null ? -1 : project_BudgetEntity.DepartmentId;
			DepartmentName = project_BudgetEntity?.DepartmentName;
			TypeId = project_BudgetEntity.Id_Type;
			ProjectType = project_BudgetEntity?.Type;
			Description = project_BudgetEntity?.Description;
			CreationDate = project_BudgetEntity?.CreationDate;
			OrderCount = project_BudgetEntity.OrderCount ?? 0;
			BudgetYear = project_BudgetEntity?.BudgetYear ?? DateTime.Today.Year;
			PSZOffer = project_BudgetEntity?.PSZOffer ?? 0;
			// - 
			ApprovalTime = project_BudgetEntity?.ApprovalTime;
			ApprovalUserId = project_BudgetEntity?.ApprovalUserId ?? -1;
			ApprovalUserName = project_BudgetEntity?.ApprovalUserName;
			ApprovalUserEmail = project_BudgetEntity?.ApprovalUserEmail;
			// - 
			Status = project_BudgetEntity?.ProjectStatus ?? 0;
			StatusName = project_BudgetEntity?.ProjectStatusName;
			StatusChangeTime = project_BudgetEntity?.ProjectStatusChangeTime;
			StatusChangeUserId = project_BudgetEntity?.ProjectStatusChangeUserId ?? -1;
			StatusChangeUserName = project_BudgetEntity?.ProjectStatusChangeUserName;

			OrderAmount = bestellteArtikelExtensionEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0;

			Validators = validators?.Select(x => new Validator.ValidatorModel(x))?.ToList();

			Files = projectFileEntities == null || projectFileEntities.Count <= 0
				? null
				: projectFileEntities.Select(x => new FileModel(x))?.ToList();

			// -
			Closed = project_BudgetEntity?.Closed;
			ClosedTime = project_BudgetEntity?.ClosedTime;
			ClosedUserId = project_BudgetEntity?.ClosedUserId;
			ClosedUserName = project_BudgetEntity?.ClosedUserName;
			SiteLevelVisibility = project_BudgetEntity?.SiteLevelVisibility ?? false;

		}
		public Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity ToBudgetProject()
		{
			var entity = ToBudgetProjectExceptDept();
			entity.DepartmentId = DepartmentId;
			entity.DepartmentName = DepartmentName;
			return entity;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity ToBudgetProjectExceptDept()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity
			{
				Id = Id,
				ProjectName = ProjectName,
				ProjectBudget = ProjectBudget,
				CustomerId = CustomerId,
				CustomerNr = CustomerNr,
				ResponsableId = ResponsableId,
				Id_State = StateId,
				CurrencyId = CurrencyId,
				CompanyId = CompanyId,
				Id_Type = TypeId,
				Description = Description,
				CreationDate = CreationDate,
				TotalSpent = TotalSpent,
				CustomerName = CustomerName,
				CompanyName = CompanyName,
				CurrencyName = CurrencyName,
				DepartmentName = DepartmentName,
				ResponsableEmail = ResponsableEmail,
				ResponsableName = ResponsableName,
				Type = ProjectType,
				OrderCount = OrderCount,
				BudgetYear = BudgetYear,
				PSZOffer = PSZOffer,
				// - 
				ApprovalTime = ApprovalTime,
				ApprovalUserId = ApprovalUserId,
				ApprovalUserName = ApprovalUserName,
				ApprovalUserEmail = ApprovalUserEmail,
				// - 
				ProjectStatus = Status,
				ProjectStatusName = StatusName,
				ProjectStatusChangeTime = StatusChangeTime,
				ProjectStatusChangeUserId = StatusChangeUserId,
				ProjectStatusChangeUserName = StatusChangeUserName,
				//-
				Closed = Closed,
				ClosedUserName = ClosedUserName,
				ClosedUserId = ClosedUserId,
				ClosedTime = ClosedTime,
				SiteLevelVisibility = SiteLevelVisibility,
			};
		}
	}

	public class FileModel
	{
		public int Id { get; set; }
		public int? ProjectId { get; set; }
		public int FileId { get; set; }
		public string FileName { get; set; }
		public byte[] FileData { get; set; }
		public string FileExtension { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public DateTime? CreationTime { get; set; }


		public FileModel()
		{
		}
		public FileModel(Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity fileEntity)
		{
			if(fileEntity == null)
				return;

			this.Id = fileEntity.Id;
			this.ProjectId = fileEntity.ProjectId;
			this.FileId = fileEntity.FileId;
			this.FileName = fileEntity.FileName;
			this.FileExtension = fileEntity.FileExtension;
			this.CreationUserId = fileEntity.CreationUserId;
			this.CreationUserName = fileEntity.CreationUserName;
			this.CreationTime = fileEntity.CreationTime;
		}

		public Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity ToFileEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity
			{
				Id = Id,
				ProjectId = ProjectId ?? -1,
				FileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(FileId, FileData, FileExtension),
				FileName = FileName,
				FileExtension = FileExtension,
				CreationUserId = CreationUserId,
				CreationTime = CreationTime ?? DateTime.MinValue,
				CreationUserName = CreationUserName
			};
		}
		public async Task<ProjectFileEntity> ToFileEntityAsync(int UserId)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity
			{
				Id = Id,
				ProjectId = ProjectId ?? -1,
				FileId = await Psz.Core.Common.Helpers.ImageFileHelper.updateImageAsync(UserId, FileId, FileData, FileExtension),
				FileName = FileName,
				FileExtension = FileExtension,
				CreationUserId = CreationUserId,
				CreationTime = CreationTime ?? DateTime.MinValue,
				CreationUserName = CreationUserName
			};
		}

	}


	// Create a custom Model
	public class StatusResponseModel
	{
		public int Id { get; set; }
		public int StatusId { get; set; }
		public string Status { get; set; }
		public string ProjectName { get; set; }

		//--
		public string ProjectTypeName { get; set; }
		public int ProjectTypeId { get; set; }
		public StatusResponseModel(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity, int type, string name)
		{
			if(projectEntity is null)
			{
				return;
			}

			Id = projectEntity.Id;
			StatusId = type;
			Status = name;
			ProjectName = projectEntity.ProjectName;

		}
	}

	// Create a custom Model version 2.
	//remarks : for back-end Pascal case notation is used.
	public class TypeResponseModel
	{


		//-- look here
		public string ProjectTypeName { get; set; }
		public int ProjectTypeId { get; set; }
		public TypeResponseModel(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity)
		{
			if(projectEntity is null)
			{
				return;
			}

			ProjectTypeName = projectEntity.Type;
			ProjectTypeId = projectEntity.Id_Type;
		}
	}

	public class StatisticAmounts
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string InitialAmount { get; set; }

		public decimal AssociatingAmount { get; set; }

		public int CountOrder { get; set; }

		public int CountLeasingOrder { get; set; }

		public int CountPurchaseOrder { get; set; }

		public decimal PurchaseOnlyAmount { get; set; }

		public decimal LeasingOnlyAmount { get; set; }

		public StatisticAmounts(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity, decimal initial_amount, decimal associating_amount, int countOrder, int countLeasing, int countPurchase, decimal purchaseOnlyAmount, decimal leasingOnlyAmount)
		{
			if(projectEntity is null)
			{
				return;
			}
			Id = projectEntity.Id;
			Name = projectEntity.ProjectName;
			InitialAmount = initial_amount + "" + projectEntity.CurrencyName;
			AssociatingAmount = associating_amount;
			CountOrder = countOrder;
			CountLeasingOrder = countLeasing;
			CountPurchaseOrder = countPurchase;
			PurchaseOnlyAmount = purchaseOnlyAmount;
			LeasingOnlyAmount = leasingOnlyAmount;
		}
	}

	public class AmountOfOrderAssociatedWithProjectResponseModel
	{
		public string Name { get; set; }
		public string InitialAmount { get; set; }
		public int? CountOrder { get; set; }   // you must check it
		public decimal AmountOfOrderAssociated { get; set; }

		public string FormattedAmount { get; set; }

		public int Nr { get; set; }

		//OrderValidatedByProjectEntity



		public AmountOfOrderAssociatedWithProjectResponseModel(Infrastructure.Data.Entities.Tables.FNC.OrderValidatedByProjectEntity projectEntity, decimal? orderAmount)
		{
			if(projectEntity is null)
			{
				return;
			}

			Name = projectEntity.ProjectName;

			Nr = projectEntity.Id;


			//InitialAmount = projectEntity.BudgetYear + "" + projectEntity.CurrencyName; // use c# syntax for more fludity

			InitialAmount = $"{projectEntity.ProjectBudget}-{projectEntity.CurrencyName}";
			CountOrder = projectEntity.OrderCount;
			AmountOfOrderAssociated = orderAmount ?? 0;
			//FormattedAmount = $"{(AmountOfOrderAssociated / 1000).ToString("#,##0.##")}K {projectEntity.CurrencyName}";

			FormattedAmount = FormatAmountWithCurrency(orderAmount ?? 0, projectEntity.CurrencyName);
		}


		private string FormatAmountWithCurrency(decimal amount, string currencyName)
		{
			// Utilisation de la méthode de formatage pour éviter les problèmes avec ToString()
			string formattedAmount = $"{(amount / 1000):N2}K {currencyName}";
			return formattedAmount;
		}

	}


}


//

#region >>> Order type || Status || PayementType 
public class OrderPayementsResponseModel
{

	//
	public int PO_Order { get; set; }

	//
	public string PO_OrderName { get; set; }

	public int CountPOOrder { get; set; }

	public OrderPayementsResponseModel() { }

	public OrderPayementsResponseModel(int PoPaymentType, int CountOrder, string PoPaymentTypeName)
	{

		PO_Order = PoPaymentType;
		CountPOOrder = CountOrder;
		PO_OrderName = PoPaymentTypeName;
	}
}
#endregion

#region  >>> Order Type Model for Analystic Statistic
public class OrderTypesResponseModel
{

	//
	public int OrderType { get; set; }

	//
	public string OrderTypeName { get; set; }

	public int CountOrderType { get; set; }

	public OrderTypesResponseModel() { }

	public OrderTypesResponseModel(int orderType, int countOrderType, string orderTypeName)
	{

		OrderType = orderType;
		OrderTypeName = orderTypeName;
		CountOrderType = countOrderType;
	}
	}
#endregion


#region  >>> Order Type Model for Analystic Statistic
public class OrderExpenseResponseModel
{


	public int OrderValidationLevelId { get; set; }
	public string OrderValidationLevelName { get; set; }

	public int CountOrder { get; set; }

	public OrderExpenseResponseModel() { }

	public OrderExpenseResponseModel(int orderValidationLevelId, int countOrder, string orderValidationLevelName)
	{

		OrderValidationLevelId = orderValidationLevelId;
		OrderValidationLevelName = orderValidationLevelName;
		CountOrder = countOrder;
	}
}
#endregion


//

#region  >>>  Allocation vs Expenses (show Internal orders vs  External orders, direct vs leasing) 

//-- Purchase Or Leasing
public class MonthlyOrderExpenseResponseModel
{


	public int Id { get; set; }
	//
	public int PO_Order { get; set; }

	public string PO_OrderName { get; set; }

	public string ExpensesAmount { get; set; }



	public MonthlyOrderExpenseResponseModel() { }

	public MonthlyOrderExpenseResponseModel(int pId, int pPO_Order, string pPO_OrderName, string pExpensesAmount)
	{

		Id = pId;

		PO_Order = pPO_Order;

		PO_OrderName = pPO_OrderName;

		ExpensesAmount = pExpensesAmount;


	}
}
#endregion

//

#region >> <<Model>> Monthly view: foreach month: Bar chart 

//-- added by Salaou chaibou 
public class ProjectTypeByMonthResponseModel
{
	public List<ProjectTypeByMonthEntity> Items { get; set; }
	public ProjectTypeByMonthResponseModel(List<ProjectTypeByMonthEntity> entities)
	{
		Items = entities?.Select(x => new ProjectTypeByMonthEntity
		{
			Month = x?.Month,
			MonthNumber = x?.MonthNumber ?? 0,
			ProjectTypeName = x?.ProjectTypeName,
			Count = x?.Count ?? 0,
		})?.ToList();
	}
}

//-- added by Salaou chaibou 
public class ProjectByMonthResponseModel
{
	public int MonthNumber { get; set; }
	public int Count { get; set; }
	public ProjectByMonthResponseModel(KeyValuePair<int, int> entity)
	{
		MonthNumber = entity.Key;
		Count = entity.Value;
	}
}

//--added by adda issa abdoul razak
public class AllProjectStatusesDistinctResponseModel
{
	public List<ProjectMonthlyStatus> projectMonthlyApprovalStatusActiveResponseModel = new List<ProjectMonthlyStatus>();
	public List<ProjectMonthlyStatus> projectMonthlyApprovalStatusInactiveResponseModel = new List<ProjectMonthlyStatus>();
	public List<ProjectMonthlyStatus> projectMonthlyApprovalStatusRejectResponseModel = new List<ProjectMonthlyStatus>();

	public List<ProjectMonthlyStatus> ProjectMonthlyActiveResponseModel = new List<ProjectMonthlyStatus>();
	public List<ProjectMonthlyStatus> ProjectMonthlySuspendedResponseModel = new List<ProjectMonthlyStatus>();
	public List<ProjectMonthlyStatus> ProjectMonthlyClosedResponseModel = new List<ProjectMonthlyStatus>();

}
public class ProjectByMonthAllViewResponseModel
{
	// public List<ProjectTypeByStatusApprobationAndMonthEntity> ProjectApprobation { get; set; }
	public List<ProjectMonthlyStatus> ProjectMonthlyApprovalStatusActive { get; set; }
	public List<ProjectMonthlyStatus> ProjectMonthlyApprovalStatusInactive { get; set; }
	public List<ProjectMonthlyStatus> ProjectMonthlyApprovalStatusReject { get; set; }
	public List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>> CountOrdersPerProjectType { get; set; }
	public List<ProjectByMonth> TotalProjectByMonths { get; set; }
	public List<List<ProjectTypeByMonthEntity>> ProjectTypeByMonths { get; set; }
	public List<ProjectMonthlyStatus> ProjectMonthlyActive { get; set; }
	public List<ProjectMonthlyStatus> ProjectMonthlySuspended { get; set; }
	public List<ProjectMonthlyStatus> ProjectMonthlyClosed { get; set; }

	//public List<ProjectTypeByStatusApprobationAndMonthEntity> ProjectStatues { get; set; }
	public ProjectByMonthAllViewResponseModel(Infrastructure.Data.Entities.Tables.FNC.AdressenEntity entity)
	{

	}
}

#endregion

#region >> order according Project 
public class ProjectTypeOrderCount
{
	public string Month { get; set; }
	public int MonthNumber { get; set; }
	public string ProjectTypeName { get; set; }
	public int NumberOfOrders { get; set; }
}

#endregion

#region
public class ProjectByMonth
{
	public int MonthNumber { get; set; }
	public int Count { get; set; }

}

#endregion

#region
public class ProjectTypeByMonth
{
	public string Month { get; set; }
	public string ProjectTypeName { get; set; }
	public int Count { get; set; }
}
#endregion



#region  >> personalize Class

//Approbation Model >> DTO 
public class ProjectTypeByStatusApprobationAndMonth
{
	public string Month { get; set; }
	public string ProjectTypeName { get; set; }

	public string Status { get; set; }
	public int Count { get; set; }
	public ProjectTypeByStatusApprobationAndMonth()
	{

	}
}
#endregion

#region
public class ProjectMonthlyStatus
{
	public int MonthNumber { get; set; }

	// public int ProjectStatus { get; set; }
	public int Count { get; set; }
	public ProjectMonthlyStatus()
	{

	}
}
#endregion


#region <<< 13-20-2024  >>> Biggest Allocation
public class BiggestAllocationModel
{
	public int? ProjectId { get; set; }

	public string ProjectName { get; set; }

	public decimal BudgetAllocatedForProject { get; set; }

	public string FormattedBudgetAllocatedForProject { get; set; }

	public string CurrencyName { get; set; }


}


#endregion

#region <<< 13-20-2024  >>> Most expensive 2 - biggest sum of orders
public class ProjectBiggestSumOfOrderModel
{
	public int? ProjectId { get; set; }

	public string ProjectName { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }

	public int? NumberOfOrder { get; set; }

	public decimal TotalAmount { get; set; }
}


#endregion

#region <<< 13-20-2024  >>> Oldest - oldest first approval time for currently open projects
public class OldestFirstApprovalModel
{
	public int ProjectId { get; set; }

	public string ProjectName { get; set; }

	public TimeSpan Date { get; set; }

	public string Date_Time { get; set; }
}


#endregion


#region <<< 13-20-2024  >>> Oldest - oldest first approval time for currently open projects
public class BiggestOfferModel
{
	public int ProjectId { get; set; }

	public string ProjectName { get; set; }

	public string CustomerNr { get; set; }

	public string CustomerName { get; set; }

	public decimal? PSZOffer { get; set; }

	public string FormattedPSZOffer { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }
	public decimal Profit { get; set; }
	public string FormattedProfit { get; set; }
}
#endregion



#region <<< 13-20-2024  >>> Overbudgeted - Total orders amount bigger than projectBu
public class OverbudgetedModel
{
	public int ProjectId { get; set; }

	public string ProjectName { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }

	public int? NumberOfProject { get; set; }

	public decimal SumOfOrder { get; set; }



}
#endregion

#region <<< 13-20-2024  >>> BudgetLeak
public class BudgetLeakModel
{
	public int ProjectId { get; set; }

	public string Status { get; set; }

	public string ProjectName { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }
}
#endregion


#region <<< 14-20-2024 >>> OverView P<roject
public class ProjectOverViewModel
{
	public List<BiggestAllocationModel> BiggestAllocation { get; set; }

	public List<ProjectBiggestSumOfOrderModel> ProjectBiggestWithSumOfOrder { get; set; }

	public List<OldestFirstApprovalModel> OldestFirstApproval { get; set; }

	public List<BiggestOfferModel> BiggestOffer { get; set; }  //--biggest Profit

	public List<OverbudgetedModel> Overbudgeted { get; set; }  //-- Total orders amount bigger than projectBudget

	public List<BudgetLeakModel> ClosedProjectWithRemainingBudget { get; set; }

	// - add 15-02-2024 >> Best Custommer
	public List<BiggestOfferModel> BestCustommer { get; set; }

	public List<BiggestOfferModel> WorstCustommer { get; set; }

	public List<BiggestOfferModel> BiggestCustomer { get; set; }

	public List<BiggestOfferModel> SmallestCustomer { get; set; }

}
#endregion

public class BiggestAllocationResponseModel
{
	public int? ProjectId { get; set; }

	public string ProjectName { get; set; }

	public decimal BudgetAllocatedForProject { get; set; }

	public string FormattedBudgetAllocatedForProject { get; set; }

	public string CurrencyName { get; set; }
}

public class ProjectBiggestSumOfOrderResponseModel
{
	public int? ProjectId { get; set; }

	public string ProjectName { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }

	public int? NumberOfOrder { get; set; }
	public decimal TotalAmount { get; set; }

	public string FormattedTotalAmount { get; set; }
}


public class OldestFirstApprovalResponseModel
{
	public int ProjectId { get; set; }

	public string ProjectName { get; set; }

	public TimeSpan Date { get; set; }

	public string Date_Time { get; set; }

	public DateTime? CreationDate { get; set; }

	public DateTime? ApprovalTime { get; set; }
}

public class BiggestOfferResponseModel
{
	public int ProjectId { get; set; }

	public string ProjectName { get; set; }

	public string CustomerNr { get; set; }

	public string CustomerName { get; set; }

	public decimal? PSZOffer { get; set; }

	public string FormattedPSZOffer { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }
	public decimal Profit { get; set; }
	public string FormattedProfit { get; set; }

}

public class OverbudgetedResponseModel
{
	public int ProjectId { get; set; }

	public string ProjectName { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }

	public int? NumberOfProject { get; set; }

	public decimal SumOfOrder { get; set; }

	public string FormattedSumOfOrder { get; set; }

}

#region <<< 13-20-2024  >>> BudgetLeak
public class BudgetLeakResponseModel
{
	public int ProjectId { get; set; }

	public string Status { get; set; }

	public string ProjectName { get; set; }

	public decimal ProjectBudget { get; set; }

	public string FormattedProjectBudget { get; set; }

	public string CurrencyName { get; set; }
}
#endregion


//--
public class DepartmentassociatedWithSite
{
	public long CompanyId { get; set; }
	public string CompanyName { get; set; }
	public List<DepartmentEntity> Departments { get; set; }

}


public class AllocationAndExpenseResponseModel
{
	public string CompanyAllocation { get; set; }

	public string CompanyExpenses { get; set; }

	public decimal Allocation { get; set; }

	public decimal Expenses { get; set; }

	//public string CurrencyName { get; set; }
	public AllocationAndExpenseResponseModel(string allocation, string expense, decimal _allocation, decimal _expenses)
	{
		this.CompanyAllocation = allocation;
		this.CompanyExpenses = expense;

		this.Allocation = _allocation;
		this.Expenses = _expenses;
	}

}








