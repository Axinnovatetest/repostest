using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class LagerbewegungCompletModel
	{
		public long id { get; set; }
		public string typ { get; set; }
		public DateTime? datum { get; set; }
		public bool gebucht { get; set; }
		public string gebuchtVon { get; set; }
		public List<LagerbewegungDetailsModel> listPosition { get; set; }
	}
}
