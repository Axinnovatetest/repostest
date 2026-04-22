using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.LifeCycle
{
	public class AddArticleLifeCycleRequestModel
	{
		public int Id { get; set; }
		public int ArtileId { get; set; }
		public int LifeCyclePhaseId { get; set; }
		public int PhaseOrderInCycle { get; set; }
	}
}
