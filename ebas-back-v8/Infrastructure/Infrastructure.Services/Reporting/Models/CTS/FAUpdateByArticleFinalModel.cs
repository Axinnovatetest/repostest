using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAUpdateByArticleFinalModel
	{
		public List<FAUpdateByArticleListModel> Updated { get; set; }
		public List<FANotUpdateByArticleListModel> NotUpdated { get; set; }
		public FAUpdateByArticleFinalModel()
		{

		}
	}
}