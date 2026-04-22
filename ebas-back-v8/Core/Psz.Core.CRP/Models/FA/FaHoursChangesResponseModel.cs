using Infrastructure.Data.Entities.Joins.CTS;
using Psz.Core.Common.Models;


namespace Psz.Core.CRP.Models.FA
{
	public class FaHoursChangesResponseModel
	{
		public int Week { get; set; }
		public int Year { get; set; }
		public string Lager { get; set; }
		public decimal? Hours { get; set; }
		public int? FaPositionZone { get; set; }
		public int? LagerId { get; set; }
		public FaHoursChangesResponseModel(FAHoursChangesEntity entity)
		{
			Week = entity.Week;
			Year = entity.Year;
			Lager = entity.Lager ?? "";
			Hours = entity.Hours;
			FaPositionZone = entity.FaPositionZone ?? 0;
		}		
	}
	public class GetFaHoursChangesResponseModel: IPaginatedResponseModel<FaHoursChangesResponseModel>
	{

	}
}
