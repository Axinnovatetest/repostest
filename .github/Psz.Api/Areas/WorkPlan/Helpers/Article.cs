using System;

namespace Psz.Api.Areas.WorkPlan.Helpers
{
	public static class Article
	{
		public static bool EditArticle(Core.Identity.Models.UserModel user, int articleId)
		{
			var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(articleId);
			if(articleDb == null)
			{
				return true;
			}
			else
			{
				articleDb.LastEditTime = DateTime.Now;
				articleDb.LastEditUserId = user.Id;

				Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Update(articleDb);

				return true;
			}
		}
	}
}
