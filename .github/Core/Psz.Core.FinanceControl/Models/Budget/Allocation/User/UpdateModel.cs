using System;

namespace Psz.Core.FinanceControl.Models.Budget.Allocation.User
{
	public class UpdateModel
	{
		public decimal AmountMonth { get; set; }
		public decimal AmountOrder { get; set; }
		public decimal AmountSpent { get; set; }
		public decimal AmountYear { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int Year { get; set; }


		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }

		public decimal AmountNotificationThreshold { get; set; }

		public decimal AmountFix { get; set; }
		public decimal AmountInvest { get; set; }
		public bool IsFreezed { get; set; }
		public UpdateModel(Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity entity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity)
		{
			if(entity == null)
				return;

			AmountMonth = entity.AmountMonth;
			AmountOrder = entity.AmountOrder;
			AmountSpent = entity.AmountSpent;
			AmountYear = entity.AmountYear;
			AmountNotificationThreshold = entity.AmountNotificationThreshold ?? 0;
			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId;
			Id = entity.Id;
			LastEditTime = entity.LastEditTime;
			LastEditUserId = entity.LastEditUserId;
			UserId = entity.UserId;
			UserName = entity.UserName;
			Year = entity.Year;

			CompanyId = companyEntity?.Id ?? -1;
			CompanyName = companyEntity?.Name;
			DepartmentId = ((int?)departmentEntity?.Id) ?? -1;
			DepartmentName = departmentEntity?.Name;

			AmountFix = entity.AmountFix;
			AmountInvest = entity.AmountInvest;
			IsFreezed = entity.LastFreezeTime.HasValue
				? true
				: false;
		}
		public Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity
			{
				AmountMonth = AmountMonth,
				AmountOrder = AmountOrder,
				AmountSpent = AmountSpent,
				AmountYear = AmountYear,
				AmountNotificationThreshold = AmountNotificationThreshold,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				UserId = UserId,
				UserName = UserName,
				Year = Year,
				AmountFix = AmountFix,
				AmountInvest = AmountInvest,
			};
		}
	}
}
