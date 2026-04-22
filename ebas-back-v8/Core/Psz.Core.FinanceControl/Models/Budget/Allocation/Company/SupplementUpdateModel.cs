using System;

namespace Psz.Core.FinanceControl.Models.Budget.Allocation.Company
{
	public class SupplementUpdateModel
	{
		public decimal AmountInitial { get; set; }
		public string ComapnyName { get; set; }
		public int CompanyId { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int Year { get; set; }
		public SupplementUpdateModel(Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity entity)
		{
			if(entity == null)
				return;

			AmountInitial = entity.AmountInitial;
			ComapnyName = entity.ComapnyName;
			CompanyId = entity.CompanyId;
			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId;
			Id = entity.Id;
			LastEditTime = entity.LastEditTime;
			LastEditUserId = entity.LastEditUserId;
			Year = entity.Year;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity
			{
				AmountInitial = AmountInitial,
				ComapnyName = ComapnyName,
				CompanyId = CompanyId,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				Year = Year,
			};
		}
	}
}
