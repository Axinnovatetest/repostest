using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FA
{
	public class ArticleProductionCostForFACreationRequestModel
	{
		public int ArtikelNr { get; set; }
		public int TypeId { get; set; }
		public bool? FromAB { get; set; }
	}
}