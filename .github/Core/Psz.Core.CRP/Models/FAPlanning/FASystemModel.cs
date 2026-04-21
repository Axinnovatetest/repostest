using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning
{
	public class FASystemModel: ICloneable
	{
		public List<WeekQuantityModel> FAMovement { get; set; }
		public List<WeekQuantityModel> ABMovement { get; set; }
		public List<WeekQuantityModel> LPMovement { get; set; }
		public List<WeekQuantityModel> FCMovement { get; set; }
		public List<WeekQuantityModel> InternBedarfMovement { get; set; }
		public List<WeekQuantityModel> ExternBedarfMovement { get; set; }
		public List<WeekQuantityModel> Bestand { get; set; }
		public List<WeekQuantityModel> Corrections { get; set; }
		public List<WeekQuantityModel> ProposedFA { get; set; }
		public int WeekFrozenZone { get; set; } = 0;
		public int CurrentWeek { get; set; } = 0;
		public decimal CurrentStock { get; set; } = 0;
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public class WeekQuantityModel: ICloneable
	{
		public int Week { get; set; }
		public int Year { get; set; }
		public decimal Quantity { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
