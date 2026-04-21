using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class LogPositionResponseModel
	{
		public decimal? Price { get; set; }
		public decimal? PriceDefault { get; set; }
		public string WarungSymbol { get; set; }
		public DateTime? ValidFrom { get; set; }
		public string User { get; set; }
		public DateTime? DateUpdate { get; set; }
		public decimal BasePrice { get; set; }

		public LogPositionResponseModel(Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity entity)
		{
			Price = entity.Price;
			ValidFrom = entity.ValidFrom;
			User = entity.UserName;
			DateUpdate = entity.DateUpdate;
			//
			PriceDefault = entity.PriceDefault;
			WarungSymbol = entity.WarungSymbol;
			BasePrice = entity.BasePrice ?? 0;
		}
	}
}
