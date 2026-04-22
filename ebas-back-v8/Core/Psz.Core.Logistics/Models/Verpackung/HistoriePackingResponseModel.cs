using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class HistoriePackingResponseModel
	{
		public List<HistoriePackingModel> listVerpackung { get; set; } = new List<HistoriePackingModel>();

		public int AllCount { get; set; }

		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
