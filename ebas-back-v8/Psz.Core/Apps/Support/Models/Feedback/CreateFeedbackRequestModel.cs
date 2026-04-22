namespace Psz.Core.Apps.Support.Models.Feedback
{
	public class CreateFeedbackRequestModel
	{
		public int Rating { get; set; }
		public string FeedbackType { get; set; }
		public string Comment { get; set; }
		public byte[] Image { get; set; }
		public string Module { get; set; }
		public string? Submodule { get; set; }
		public string PageUrl { get; set; }
		public string? Priority { get; set; }


		public Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity
			{
				Rating = Rating,
				Image = Image,
				FeedbackType = FeedbackType,
				Comment = Comment,
				Module = Module,
				Submodule = Submodule,
				PageUrl = PageUrl,
				priority = Priority
			};
		}

	}
}
