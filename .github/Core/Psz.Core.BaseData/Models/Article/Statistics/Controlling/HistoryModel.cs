using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Controlling
{
	public class HistoryModel
	{
		public string Id { get; set; }
		public string ArticleId { get; set; }
		public DateTime? EditTime { get; set; }
		public string EditAction { get; set; }
		public string EditDescription { get; set; }
		public string EditUser { get; set; }
		public HistoryModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingHistory controllingHistory)
		{
			if(controllingHistory == null)
				return;

			Id = controllingHistory.Id;
			ArticleId = controllingHistory.ArticleId;
			EditTime = DateTime.TryParse(controllingHistory.EditTime, out var v) ? v : null;
			EditAction = controllingHistory.EditAction;
			EditDescription = controllingHistory.EditDescription;
			EditUser = controllingHistory.EditUser;
		}
	}
}
