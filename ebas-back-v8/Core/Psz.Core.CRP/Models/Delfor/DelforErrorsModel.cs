
namespace Psz.Core.CRP.Models.Delfor
{
	public class DelforErrorsModel
	{
		public int Id { get; set; }
		public DateTime? Date { get; set; }
		public int? Customer { get; set; }
		public string Sender { get; set; }
		public string DocumentNumber { get; set; }
		public string ErrorMessage { get; set; }
		public string File { get; set; }
	}

	public class DelforValidatedErrorsModel: DelforErrorsModel
	{
		public string ProcessedBy { get; set; }
		public DateTime? ProcessedOn { get; set; }
	}

	public class DelforErrorsRequestModel
	{
		public string Duns { get; set; }
	}
}
