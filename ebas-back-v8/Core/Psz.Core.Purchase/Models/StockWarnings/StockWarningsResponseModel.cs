using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StockWarningsResponseModel
	{
		public List<WeekValueModel> Orders { get; set; }
		public List<WeekValueModel> Needs { get; set; }
		public List<WeekValueModel> Quantities { get; set; }
		public List<WeekValueModel> Corrections { get; set; }
		public List<WeekValueModel> SuggestedOrdes { get; set; }
		public string CurrentWeek { get; set; }
		public List<WeekValueModel> BacklogNeeds { get; set; }
		public List<WeekValueModel> BacklogOrders { get; set; }
		public List<WeekValueModel> CurrentStock { get; set; }
		public List<WeekValueModel> MinimumStock { get; set; }
	}
	public class StockWarningsRequesteModel
	{
		public int ArtikelNr { get; set; }
		public int CountryId { get; set; }
		public int UnitId { get; set; }
		public int? SupplierNr { get; set; }
	}

	public class WeekValueModel
	{
		public string Week { get; set; }
		public decimal Value { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
