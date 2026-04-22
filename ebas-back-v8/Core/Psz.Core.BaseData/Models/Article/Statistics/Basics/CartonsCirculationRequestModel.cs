using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class CartonsCirculationRequestModel
	{
		public Enums.ArticleEnums.StatsCartonsCirculationSites Site { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTill { get; set; }
	}
}
