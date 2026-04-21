using Psz.Core.CustomerService.Models.Blanket;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Statistics
{
	public class RAPositionsColorsSearchModel
	{
		public int SelectedColor { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string Artikelnummer { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }

		public int TypeId { get; set; }
	}
	public class RAPositionsColorsResponseModel
	{
		public List<BlanketItem> Positions { get; set; } = new List<BlanketItem>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}

	public class RADashboardResponseModel
	{
		public List<BlanketItem> RedPositions { get; set; } = new List<BlanketItem>();
		public List<BlanketItem> OrangePositions { get; set; } = new List<BlanketItem>();
		public List<BlanketItem> GreenPositions { get; set; } = new List<BlanketItem>();
	}
}
