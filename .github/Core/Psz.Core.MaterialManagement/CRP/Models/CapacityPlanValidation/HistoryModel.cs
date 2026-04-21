using Infrastructure.Data.Entities.Tables.MTM;

namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlanValidation
{
	public class HistoryRequestModel
	{
		public int Year { get; set; }
		public int CountryId { get; set; }
		public int HallId { get; set; }
	}
	public class HistoryResponseModel
	{
		public int Year { get; set; }
		public int CountryId { get; set; }
		public int HallId { get; set; }
		public List<HistoryItem> Items { get; set; }

		public HistoryResponseModel()
		{

		}
		public HistoryResponseModel(CapacityPlanValidationLogEntity logEntity)
		{
			if(logEntity == null)
				return;

			Year = logEntity.Year;
			CountryId = logEntity.CountryId;
			HallId = logEntity.HallId;
		}
		// -
		public class HistoryItem
		{
			public int? UserId { get; set; }
			public string UserName { get; set; }
			public int? Level { get; set; }
			public string LevelDescription { get; set; }
			public string ValidationStatus { get; set; }
			public DateTime? Date { get; set; }
		}
	}
}
