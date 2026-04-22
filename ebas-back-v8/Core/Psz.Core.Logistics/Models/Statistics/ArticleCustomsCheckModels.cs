using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ArticleCustomsCheckResponseModels
	{
		public int ArticlesTotalCount { get; set; }
		public int ArticlesWithoutNumberCount { get; set; }
		public int ArticlesWithWrongNumberCount { get; set; }
		public int ArticlesWithCorrectNumberCount { get; set; }
		public DateTime? CheckDate { get; set; }
		public int? CheckUser { get; set; }
		public string CheckUserName { get; set; }
		public int Id { get; set; }
		public decimal CheckOkPercentage { get; set; }
		// - 2023-11-21 - temp db table id
		public int TempId { get; set; }
		public ArticleCustomsCheckResponseModels()
		{

		}
		public ArticleCustomsCheckResponseModels(Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			ArticlesTotalCount = entity.ArticlesTotalCount ?? 0;
			ArticlesWithoutNumberCount = entity.ArticlesWithoutNumberCount ?? 0;
			ArticlesWithWrongNumberCount = entity.ArticlesWithWrongNumberCount ?? 0;
			ArticlesWithCorrectNumberCount = ArticlesTotalCount - (ArticlesWithoutNumberCount + ArticlesWithWrongNumberCount);
			CheckDate = entity.CheckDate;
			CheckUser = entity.CheckUser;
			CheckUserName = entity.CheckUserName;
			Id = entity.Id;
			CheckOkPercentage = ((decimal)ArticlesWithCorrectNumberCount) / ArticlesTotalCount * 100;
		}
		public ArticleCustomsCheckResponseModels(Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity entity, int tempId)
			: this(entity)
		{
			TempId = tempId;
		}
	}
}
