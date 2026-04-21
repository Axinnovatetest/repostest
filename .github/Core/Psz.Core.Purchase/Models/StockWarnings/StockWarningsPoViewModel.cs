using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StockWarningsPoViewModel
	{
		public int? Nr { get; set; }
		public int? BestellungNr { get; set; }
		public decimal? Quantity { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? ConfirmedDate { get; set; }
		public DateTime? CreationDate { get; set; }

		public StockWarningsPoViewModel()
		{

		}
		public StockWarningsPoViewModel(Infrastructure.Data.Entities.Joins.PRS.StockWarningsPoViewEntity entity)
		{
			Nr = entity.Nr;
			BestellungNr = entity.BestellungNr;
			Quantity = entity.Quantity;
			DeliveryDate = entity.DeliveryDate;
			ConfirmedDate = entity.ConfirmedDate;
			CreationDate = entity.CreationDate;
		}
	}
}