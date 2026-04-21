using System.Collections.Generic;
using Psz.Core.Logistics.Models.PlantBookings;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class ArticleSearchResponseModel
	{
		public List<ArtikelMinimalLagerbewegungModel> Articles { get; set; } = new List<ArtikelMinimalLagerbewegungModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}

	public class ArticleSearchMHDResponseModel
	{
		public List<ArtikelWithMhModel> Articles { get; set; } = new List<ArtikelWithMhModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
