using System;

namespace Psz.Core.FinanceControl.Models.Budget.Project
{
	public class HistoryModel
	{
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity GetHistory(
			Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
			Identity.Models.UserModel userModel)
		{
			if(projectEntity == null)
				return null;

			return new Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity
			{
				ApprovalTime = projectEntity.ApprovalTime,
				ApprovalUserEmail = projectEntity.ApprovalUserEmail,
				ApprovalUserId = projectEntity.ApprovalUserId,
				ApprovalUserName = projectEntity.ApprovalUserName,
				Archived = projectEntity.Archived,
				ArchiveTime = projectEntity.ArchiveTime,
				ArchiveUserId = projectEntity.ArchiveUserId,
				BudgetYear = projectEntity.BudgetYear,
				CompanyId = projectEntity.CompanyId,
				CompanyName = projectEntity.CompanyName,
				CreationDate = projectEntity.CreationDate,
				CurrencyId = projectEntity.CurrencyId,
				CurrencyName = projectEntity.CurrencyName,
				CustomerId = projectEntity.CustomerId,
				CustomerName = projectEntity.CustomerName,
				CustomerNr = projectEntity.CustomerNr,
				Deleted = projectEntity.Deleted,
				DeleteTime = projectEntity.DeleteTime,
				DeleteUserId = projectEntity.DeleteUserId,
				DepartmentId = projectEntity.DepartmentId,
				DepartmentName = projectEntity.DepartmentName,
				Description = projectEntity.Description,
				HistoryTime = DateTime.Now,
				HistoryUserEmail = userModel?.Email,
				HistoryUserId = userModel?.Id ?? -1,
				HistoryUserName = userModel?.Username,
				ProjectId = projectEntity.Id,
				Id_State = projectEntity.Id_State,
				Id_Type = projectEntity.Id_Type,
				OrderCount = projectEntity.OrderCount,
				ProjectBudget = projectEntity.ProjectBudget,
				ProjectName = projectEntity.ProjectName,
				ProjectStatus = projectEntity.ProjectStatus,
				ProjectStatusChangeTime = projectEntity.ProjectStatusChangeTime,
				ProjectStatusChangeUserId = projectEntity.ProjectStatusChangeUserId,
				ProjectStatusChangeUserName = projectEntity.ProjectStatusChangeUserName,
				ProjectStatusName = projectEntity.ProjectStatusName,
				PSZOffer = projectEntity.PSZOffer,
				ResponsableEmail = projectEntity.ResponsableEmail,
				ResponsableId = projectEntity.ResponsableId,
				ResponsableName = projectEntity.ResponsableName,
				TotalSpent = projectEntity.TotalSpent,
				Type = projectEntity.Type,
				//-
				Closed = projectEntity.Closed,
				ClosedTime = projectEntity.ClosedTime,
				ClosedUserEmail = projectEntity.ClosedUserEmail,
				ClosedUserId = projectEntity.ClosedUserId,
				ClosedUserName = projectEntity.ClosedUserName
			};
		}
	}
}
