using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class PositionsColorsResponseModel
	{
		public int RAWithRedPositions { get; set; }
		public int RAWithYellowPositions { get; set; }
		public int RAWithOrangePositions { get; set; }
		public int RAWithGreenPositions { get; set; }
		//

		public int RedPositions { get; set; }
		public int YellowPositions { get; set; }
		public int OrangePositions { get; set; }
		public int GreenPositions { get; set; }

		public PositionsColorsResponseModel()
		{

		}

		public PositionsColorsResponseModel(List<Tuple<int, int, int>> PositionsColors)
		{
			if(PositionsColors == null || PositionsColors.Count == 0)
				return;
			RAWithRedPositions = PositionsColors.Where(x => x.Item3 == 3).Select(y => y.Item1).Distinct().ToList().Count;
			RAWithYellowPositions = PositionsColors.Where(x => x.Item3 == 2).Select(y => y.Item1).Distinct().ToList().Count;
			RAWithOrangePositions = PositionsColors.Where(x => x.Item3 == 1).Select(y => y.Item1).Distinct().ToList().Count;
			RAWithGreenPositions = PositionsColors.Where(x => x.Item3 == 0).Select(y => y.Item1).Distinct().ToList().Count;
			//
			RedPositions = PositionsColors.Where(x => x.Item3 == 3).ToList().Count;
			YellowPositions = PositionsColors.Where(x => x.Item3 == 2).ToList().Count;
			OrangePositions = PositionsColors.Where(x => x.Item3 == 1).ToList().Count;
			GreenPositions = PositionsColors.Where(x => x.Item3 == 0).ToList().Count;
		}
	}
}
