using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BudgetAllocationUserEntity
	{
		public decimal AmountFix { get; set; }
		public decimal AmountInvest { get; set; }
		public decimal AmountMonth { get; set; }
		public decimal? AmountNotificationThreshold { get; set; }
		public decimal AmountOrder { get; set; }
		public decimal AmountSpent { get; set; }
		public decimal AmountYear { get; set; }
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
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int Year { get; set; }

		public BudgetAllocationUserEntity() { }

		public BudgetAllocationUserEntity(DataRow dataRow)
		{
			AmountFix = Convert.ToDecimal(dataRow["AmountFix"]);
			AmountInvest = Convert.ToDecimal(dataRow["AmountInvest"]);
			AmountMonth = Convert.ToDecimal(dataRow["AmountMonth"]);
			AmountNotificationThreshold = (dataRow["AmountNotificationThreshold"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AmountNotificationThreshold"]);
			AmountOrder = Convert.ToDecimal(dataRow["AmountOrder"]);
			AmountSpent = Convert.ToDecimal(dataRow["AmountSpent"]);
			AmountYear = Convert.ToDecimal(dataRow["AmountYear"]);
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
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
			Year = Convert.ToInt32(dataRow["Year"]);
		}

		public BudgetAllocationUserEntity ShallowClone()
		{
			return new BudgetAllocationUserEntity
			{
				AmountFix = AmountFix,
				AmountInvest = AmountInvest,
				AmountMonth = AmountMonth,
				AmountNotificationThreshold = AmountNotificationThreshold,
				AmountOrder = AmountOrder,
				AmountSpent = AmountSpent,
				AmountYear = AmountYear,
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
				UserId = UserId,
				UserName = UserName,
				Year = Year
			};
		}
	}
}

