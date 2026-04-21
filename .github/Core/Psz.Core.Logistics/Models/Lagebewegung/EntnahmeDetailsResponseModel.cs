using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class EntnahmeDetailsResponseModel
	{
		public List<EntnahmeWertTreeDetailsModel> listEntnahme { get; set; } = new List<EntnahmeWertTreeDetailsModel>();

		public int AllCount { get; set; }

		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
