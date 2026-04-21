using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FA
{
	public class FaDataChartResponseModel
	{
		public List<string> x { get; set; }
		public List<string> y { get; set; }
		public List<List<decimal>> z { get; set; }
	}
}
