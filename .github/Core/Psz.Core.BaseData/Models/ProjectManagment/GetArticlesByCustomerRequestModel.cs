using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class GetArticlesByCustomerRequestModel
	{
		public int CustomerNumber { get; set; }
		public string SearchText { get; set; }
	}
}
