using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.Overview
{
	public class UpdateManufacturerArticleRequestModel
	{
		public int ArticleId { get; set; }
		public int ManufacturerArticleId { get; set; }
		public string ManufacturerArticle { get; set; }
	}
}
