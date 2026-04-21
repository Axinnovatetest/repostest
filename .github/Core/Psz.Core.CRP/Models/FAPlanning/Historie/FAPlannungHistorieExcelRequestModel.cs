using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning.Historie
{
	public class FAPlannungHistorieExcelRequestModel
	{
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public int? Kundennummer { get; set; }
	}
}