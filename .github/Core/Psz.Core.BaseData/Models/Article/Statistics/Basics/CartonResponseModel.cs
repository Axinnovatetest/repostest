using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class CartonResponseModel
	{
		public string Artikelnummer_Umlauf { get; set; }
		public decimal? Bestand { get; set; }
		public int? SummevonBedarf { get; set; }
		public decimal? Transfer_Bestand { get; set; }
		public List<OrderItem> OrderItems { get; set; }

		public class OrderItem
		{
			public int? Bestellung_Nr { get; set; }
			public DateTime? Bestatigter_Termin { get; set; }
			public decimal? Anzahl { get; set; }
		}
	}
}
