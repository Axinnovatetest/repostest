using System.Collections.Generic;

namespace Psz.Core.CRP.Models.FA
{
	public class CSTLogSearchModel
	{
		public string ProjectNr { get; set; }
		public string VorfallNr { get; set; }
		public string User { get; set; }
		public List<int> Types { get; set; }//typ
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
