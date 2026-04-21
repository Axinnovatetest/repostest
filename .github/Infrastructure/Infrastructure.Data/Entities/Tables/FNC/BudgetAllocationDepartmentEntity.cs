using System;
using System.Data;


namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BudgetAllocationDepartmentEntity
	{
		public decimal? AmountAllocatedToProjects { get; set; }
		public decimal AmountAllocatedToUsers { get; set; }
		public decimal AmountFix { get; set; }
		public decimal AmountInitial { get; set; }
		public decimal AmountInvest { get; set; }
		public decimal? AmountNotificationThreshold { get; set; }
		public decimal AmountSpent { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
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

		public BudgetAllocationDepartmentEntity() { }

		public BudgetAllocationDepartmentEntity(DataRow dataRow)
		{
			AmountAllocatedToProjects = (dataRow["AmountAllocatedToProjects"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountAllocatedToProjects"]);
			AmountAllocatedToUsers = Convert.ToDecimal(dataRow["AmountAllocatedToUsers"]);
			AmountFix = Convert.ToDecimal(dataRow["AmountFix"]);
			AmountInitial = Convert.ToDecimal(dataRow["AmountInitial"]);
			AmountInvest = Convert.ToDecimal(dataRow["AmountInvest"]);
			AmountNotificationThreshold = (dataRow["AmountNotificationThreshold"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountNotificationThreshold"]);
			AmountSpent = Convert.ToDecimal(dataRow["AmountSpent"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			DepartmentId = Convert.ToInt32(dataRow["DepartmentId"]);
			DepartmentName = (dataRow["DepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DepartmentName"]);
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

		public BudgetAllocationDepartmentEntity ShallowClone()
		{
			return new BudgetAllocationDepartmentEntity
			{
				AmountAllocatedToProjects = AmountAllocatedToProjects,
				AmountAllocatedToUsers = AmountAllocatedToUsers,
				AmountFix = AmountFix,
				AmountInitial = AmountInitial,
				AmountInvest = AmountInvest,
				AmountNotificationThreshold = AmountNotificationThreshold,
				AmountSpent = AmountSpent,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				DepartmentId = DepartmentId,
				DepartmentName = DepartmentName,
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

