using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning
{
	public class FASystemRequestModel
	{
		public int ArticleNr { get; set; }
		public int Period { get; set; }
		public int? Unit { get; set; }
		public bool? Mindesbestand { get; set; }
	}
}
