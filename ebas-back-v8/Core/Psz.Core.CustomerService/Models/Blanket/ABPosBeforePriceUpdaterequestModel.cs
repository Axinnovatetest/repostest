using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class ABPosBeforePriceUpdaterequestModel
	{
		public int RahmenNr { get; set; }
		public int PositionNr { get; set; }
		public DateTime DateFin { get; set; }

	}
}
