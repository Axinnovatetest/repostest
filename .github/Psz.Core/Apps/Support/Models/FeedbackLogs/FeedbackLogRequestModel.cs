using Psz.Core.Common.Models;
using System;

namespace Psz.Core.Apps.Support.Models.Logs
{
	public class FeedbackLogRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
		public DateTime? SearchDate { get; set; }
		public string Level { get; set; }
		public bool? IsTreated { get; set; }
	}
}
