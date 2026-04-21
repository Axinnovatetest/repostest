using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class PositionsByColorsModel
	{
		public List<BlanketItem> RedPositions { get; set; }
		public List<BlanketItem> OrangePositions { get; set; }
		public List<BlanketItem> YellowPositions { get; set; }
		public List<BlanketItem> GreenPositions { get; set; }

		public PositionsByColorsModel()
		{

		}
	}
}
