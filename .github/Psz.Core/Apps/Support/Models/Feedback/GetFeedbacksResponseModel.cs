using System;

namespace Psz.Core.Apps.Support.Models.Feedback
{
	public class GetFeedbacksResponseModel
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public byte[] Image { get; set; }
		public string Module { get; set; }
		public string PageUrl { get; set; }
		public int? Rating { get; set; }
		public string Submodule { get; set; }
		public string FeedbackType { get; set; }
		public bool? Treated { get; set; }
		public string? TreatedUser { get; set; }
		public DateTime? TreatedDate { get; set; }
		public DateTime? CreationDate { get; set; }
		public string? Username { get; set; }
		public string? Priority { get; set; }

		public int? AssociatedFeedbackCount { get; set; }
		public int? FeedbacksTreatedCount { get; set; }

		public GetFeedbacksResponseModel()
		{

		}

		public GetFeedbacksResponseModel(Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity feedbackEntity)
		{
			if(feedbackEntity == null)
				return;

			Module = feedbackEntity?.Module;
			Submodule = feedbackEntity?.Submodule;
			CreationDate = feedbackEntity?.CreationDate;
			PageUrl = feedbackEntity?.PageUrl;
			Comment = feedbackEntity?.Comment;
			Rating = feedbackEntity?.Rating;
			Image = feedbackEntity?.Image;
			FeedbackType = feedbackEntity.FeedbackType;
			Treated = feedbackEntity.Treated;
			TreatedUser = feedbackEntity.TreatedUser;
			TreatedDate = feedbackEntity.TreatedDate;
			Username = feedbackEntity.Username;
			Id = feedbackEntity.Id;
			Priority = feedbackEntity.priority;

		}
		public Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity
			{
				Module = Module,
				Submodule = Submodule,
				PageUrl = PageUrl,
				Comment = Comment,
				Rating = Rating,
				Image = Image,
				FeedbackType = FeedbackType,
				Treated = Treated,
				TreatedUser = TreatedUser,
				TreatedDate = TreatedDate,
				CreationDate = CreationDate,
				Username = Username,
				priority = Priority
			};
		}
	}
}
