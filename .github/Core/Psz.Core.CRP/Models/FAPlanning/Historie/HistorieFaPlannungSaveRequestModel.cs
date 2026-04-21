using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning.Historie
{
	public class HistorieFaPlannungSaveRequestModel
	{
		public DateTime HistoryDate { get; set; }
		public List<HistorieFaPlannungDetailsModel> Details { get; set; }
	}
}