using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BudgetAllocationCompanyEntity
	{
		public decimal AmountAllocatedToDepartments { get; set; }
		public decimal? AmountAllocatedToProjects { get; set; }
		public decimal AmountFix { get; set; }
		public decimal AmountInitial { get; set; }
		public decimal AmountInvest { get; set; }
		public decimal? AmountNotificationSuperValidationThreshold { get; set; }
		public decimal? AmountNotificationThreshold { get; set; }
		public decimal AmountSpent { get; set; }
		public decimal? AmountSupplements { get; set; }
		public string ComapnyName { get; set; }
		public int CompanyId { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? LastFreezeTime { get; set; }
		public int? LastFreezeUserId { get; set; }
		public DateTime? LastResetTime { get; set; }
		public int? LastResetUserId { get; set; }
		public DateTime? LastUnFreezeTime { get; set; }
		public int? LastUnFreezeUserId { get; set; }
		public DateTime? LastUnResetTime { get; set; }
		public int? LastUnResetUserId { get; set; }
		public int Year { get; set; }

		public BudgetAllocationCompanyEntity() { }

		public BudgetAllocationCompanyEntity(DataRow dataRow)
		{
			AmountAllocatedToDepartments = Convert.ToDecimal(dataRow["AmountAllocatedToDepartments"]);
			AmountAllocatedToProjects = (dataRow["AmountAllocatedToProjects"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountAllocatedToProjects"]);
			AmountFix = Convert.ToDecimal(dataRow["AmountFix"]);
			AmountInitial = Convert.ToDecimal(dataRow["AmountInitial"]);
			AmountInvest = Convert.ToDecimal(dataRow["AmountInvest"]);
			AmountNotificationSuperValidationThreshold = (dataRow["AmountNotificationSuperValidationThreshold"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountNotificationSuperValidationThreshold"]);
			AmountNotificationThreshold = (dataRow["AmountNotificationThreshold"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountNotificationThreshold"]);
			AmountSpent = Convert.ToDecimal(dataRow["AmountSpent"]);
			AmountSupplements = (dataRow["AmountSupplements"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountSupplements"]);
			ComapnyName = (dataRow["ComapnyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ComapnyName"]);
			CompanyId = Convert.ToInt32(dataRow["CompanyId"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			LastFreezeTime = (dataRow["LastFreezeTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastFreezeTime"]);
			LastFreezeUserId = (dataRow["LastFreezeUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastFreezeUserId"]);
			LastResetTime = (dataRow["LastResetTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastResetTime"]);
			LastResetUserId = (dataRow["LastResetUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastResetUserId"]);
			LastUnFreezeTime = (dataRow["LastUnFreezeTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUnFreezeTime"]);
			LastUnFreezeUserId = (dataRow["LastUnFreezeUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUnFreezeUserId"]);
			LastUnResetTime = (dataRow["LastUnResetTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUnResetTime"]);
			LastUnResetUserId = (dataRow["LastUnResetUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUnResetUserId"]);
			Year = Convert.ToInt32(dataRow["Year"]);
		}

		public BudgetAllocationCompanyEntity ShallowClone()
		{
			return new BudgetAllocationCompanyEntity
			{
				AmountAllocatedToDepartments = AmountAllocatedToDepartments,
				AmountAllocatedToProjects = AmountAllocatedToProjects,
				AmountFix = AmountFix,
				AmountInitial = AmountInitial,
				AmountInvest = AmountInvest,
				AmountNotificationSuperValidationThreshold = AmountNotificationSuperValidationThreshold,
				AmountNotificationThreshold = AmountNotificationThreshold,
				AmountSpent = AmountSpent,
				AmountSupplements = AmountSupplements,
				ComapnyName = ComapnyName,
				CompanyId = CompanyId,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				LastFreezeTime = LastFreezeTime,
				LastFreezeUserId = LastFreezeUserId,
				LastResetTime = LastResetTime,
				LastResetUserId = LastResetUserId,
				LastUnFreezeTime = LastUnFreezeTime,
				LastUnFreezeUserId = LastUnFreezeUserId,
				LastUnResetTime = LastUnResetTime,
				LastUnResetUserId = LastUnResetUserId,
				Year = Year
			};
		}
	}
}

