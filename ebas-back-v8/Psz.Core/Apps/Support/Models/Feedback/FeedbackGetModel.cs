using System;

namespace Psz.Core.Apps.Support.Models.Feedback
{
	public class FeedbackGetModel
	{
		public string comment { get; set; }
		public DateTime? date { get; set; }
		public int Id { get; set; }
		public byte[] image { get; set; }
		public string module { get; set; }
		public string pageUrl { get; set; }
		public int? rating { get; set; }
		public string submodule { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }
		public string FeedbackType { get; set; }
		public bool? Treated { get; set; }
		public string TreatedUser { get; set; }
		public DateTime? TreatedDate { get; set; }

		public FeedbackGetModel(Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity feedbackEntity)
		{
			if(feedbackEntity == null)
				return;

			Id = feedbackEntity?.Id ?? -1;
			UserId = feedbackEntity?.UserId ?? -1;
			Username = feedbackEntity?.Username;
			module = feedbackEntity?.Module;
			submodule = feedbackEntity?.Submodule;
			//date = feedbackEntity?.date ?? DateTime.MinValue;
			date = feedbackEntity?.CreationDate ?? DateTime.Now;
			pageUrl = feedbackEntity?.PageUrl;
			comment = feedbackEntity?.Comment;
			rating = feedbackEntity?.Rating;
			image = feedbackEntity?.Image;
			FeedbackType = feedbackEntity.FeedbackType;
			Treated = feedbackEntity.Treated;
			TreatedUser = feedbackEntity.TreatedUser;
			TreatedDate = feedbackEntity.TreatedDate;

		}
		public Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity
			{
				Id = Id,
				UserId = UserId,
				Username = Username,
				CreationDate = date,
				Module = module,
				Submodule = submodule,
				PageUrl = pageUrl,
				Comment = comment,
				Rating = rating,
				Image = image,
				FeedbackType = FeedbackType,
				Treated = Treated,
				TreatedUser = TreatedUser,
				TreatedDate = TreatedDate,
			};
		}
	}
}
