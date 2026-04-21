using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.Forecasts
{
	public class CRPExcelRequestModel
	{
		public int? KundenNr { get; set; }
		public int? TypeId { get; set; }
		public bool OnlyLastVersion { get; set; }
	}
}
