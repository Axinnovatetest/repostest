using Infrastructure.Services.Reporting.Models.CTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FAUpdateByArticleFinalModel
	{
		public List<FAUpdateByArticleListModel> Updated { get; set; }
		public List<FANotUpdateByArticleListModel> NotUpdated { get; set; }
		public string? Logo { get; set; }
		public FAUpdateByArticleFinalModel()
		{

		}
	}
}
