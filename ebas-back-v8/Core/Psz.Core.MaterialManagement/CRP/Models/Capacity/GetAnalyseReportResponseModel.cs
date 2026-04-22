namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class GetAnalyseReportResponseModel
	{
		public List<Item> Capacities { get; set; } = new List<Item>();
		public List<Item> RequestedCapacities { get; set; } = new List<Item>();
		public List<FaItem> FaultyFAs { get; set; } = new List<FaItem>();
		public class Item
		{
			public int WeekNumber { get; set; }
			public decimal Attendance { get; set; }
			public decimal PlanCapacity { get; set; }
			public decimal RequiredEmployees { get; set; }
			public decimal FaultyFARequiredCapacities { get; set; }
		}
		public class FaItem
		{
			public int FaNumber { get; set; }
			public DateTime? FaDate { get; set; }
			public string FaArticle { get; set; }
			public decimal FaQuantity { get; set; }
			public decimal FaUnitTime { get; set; }
			public decimal FaTotalTime { get; set; }
			public int WPL_ArticleId { get; set; }
			public int WorkScheduleId { get; set; }

			public int WeekNumber { get; set; }
			public FaItem()
			{

			}
			public FaItem(int weekNumber, Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity faultyTimeEntity)
			{
				WeekNumber = weekNumber;
				if(faultyTimeEntity == null)
					return;

				FaNumber = faultyTimeEntity.FaNumber;
				FaDate = faultyTimeEntity.FaDate;
				FaArticle = faultyTimeEntity.FaArticle;
				FaQuantity = faultyTimeEntity.FaQuantity;
				FaUnitTime = faultyTimeEntity.FaUnitTime;
				FaTotalTime = Common.Helpers.MathHelper.RoundDecimal(faultyTimeEntity.FaTotalTime, Helpers.Config.DecimalPart);
				WPL_ArticleId = faultyTimeEntity.WPL_ArticleId;
				WorkScheduleId = faultyTimeEntity.WorkScheduleId;

			}
		}
	}
}
