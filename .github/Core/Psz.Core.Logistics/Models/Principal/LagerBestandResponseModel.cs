using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Principal
{
	public class LagerBestandResponseModel
	{
		public List<LagerBestandModel> ListArtikel { get; set; } = new List<LagerBestandModel>();

		public int AllCount { get; set; }
		public decimal gesammtZeit { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
