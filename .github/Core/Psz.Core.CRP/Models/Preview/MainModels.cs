using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.Preview
{
	public class PreviewRequestModel
	{
		public int ArticleId1 { get; set; }
		public int ArticleId2 { get; set; }
		public int ArticleId3 { get; set; }
	}
	public class PreviewDataWeekResponseModel
	{
		public int ArticleId { get; set; }
		public decimal SumFa { get; set; }
		public decimal SumAb { get; set; }
		public decimal SumFc { get; set; }
		public decimal SumLp { get; set; }
		public int Year { get; set; }
		public int Week { get; set; }
		public PreviewDataWeekResponseModel()
		{
				
		}
		public PreviewDataWeekResponseModel(Infrastructure.Data.Entities.Joins.CRP.PreviewQuantitiesEntity entity)
		{
			if(entity == null) return;

			// -
			ArticleId = entity.ArticleId;
			SumFa = entity.FAQuantity;
			SumAb = entity.ABQuantity;
			SumFc = entity.FCQuantity;
			SumLp = entity.LPQuantity;
			Year = entity.Year;
			Week = entity.Week;
		}
	}
	public class PreviewArticleResponseModel
	{
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
		public string ArticleStatus { get; set; }
		public decimal Stock { get; set; }
		public decimal MinimumStock { get; set; }
		public decimal Difference { get; set; }
		public decimal SumNeeds { get; set; }
		public decimal SumNeedsAB { get; set; }
		public decimal SumNeedsFC { get; set; }
		public decimal SumNeedsLP { get; set; }
		public decimal SumFa { get; set; }
		public DateTime? SyncDate { get; set; }
		public List<PreviewDataWeekResponseModel> Data { get; set; }
	}
	public class PreviewHeaderWeekResponseModel	
	{
		public int Year { get; set; }
		public string YearName { get; set; }
		public int Month { get; set; }
		public string MonthName { get; set; }
		public string MonthLongName { get; set; }
		public int Week { get; set; }
		public string WeekName { get; set; }
		public int Day { get; set; }
		public string DayName { get; set; }
		public int Horizon { get; set; } = 3;
	}
	public class PreviewResponseModel
	{
		public List<PreviewHeaderWeekResponseModel> Headers { get; set; }
		public PreviewArticleResponseModel Article1 { get; set; }
		public PreviewArticleResponseModel Article2 { get; set; }
		public PreviewArticleResponseModel Article3 { get; set; }
	}
	public class PreviewWeekResponseModel
	{
		public int EntityId { get; set; }
		public int CustomerNumber { get; set; }
		public string EntityNumber { get; set; }
		public bool? IsManual { get; set; }
		public decimal Quantity { get; set; }
		public PreviewWeekResponseModel(Infrastructure.Data.Entities.Joins.CRP.WeekEntitiesEntity x)
		{
			if(x == null)
				return;

			EntityId = x.EntityId;
			CustomerNumber = x.CustomerNumber;
			EntityNumber = x.EntityNumber;
			IsManual = x.IsManual;
			Quantity = x.Quantity;
		}
	}
}
