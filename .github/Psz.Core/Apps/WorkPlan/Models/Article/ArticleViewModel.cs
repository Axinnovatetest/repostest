using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.Apps.WorkPlan.Models.Article
{
	public class ArticleViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string CreationUserName { get; set; }
		public DateTime CreationTime { get; set; }
		public string LastEditUserName { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int HallId { get; set; }
		public Boolean CanSafeDelete { get; set; }
		public ArticleViewModel() { }
		public ArticleViewModel(Infrastructure.Data.Entities.Tables.WPL.ArticleEntity articleDb)
		{
			this.Id = articleDb.Id;
			this.Name = articleDb.Name;
			this.CreationTime = articleDb.CreationTime;
			this.CreationUserName = Helpers.User.GetUserNameById(articleDb.CreationUserId);
			this.LastEditTime = articleDb.LastEditTime.HasValue ? articleDb.LastEditTime.Value : (DateTime?)null;
			this.LastEditUserName = articleDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(articleDb.LastEditUserId.Value) : "";
			this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteArticle(articleDb.Id);
			this.HallId = articleDb.HallId;
		}
	}
}
