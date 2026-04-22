using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ROH.OfferRequests
{
	public class VerifySupplierInformationModel
	{
		public string Message { get; set; }
		public bool IsValid { get; set; } = false;
	}
}
