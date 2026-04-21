using System.Collections.Generic;

namespace Psz.Core.CRP.Models.FA.Update
{
	public class UpdateFAByArticleNonBETNModel
	{
		public bool Stucklisten { get; set; }
		public bool KundenIndex { get; set; }
		public List<OpenFAForUpdateModel> FAs { get; set; }

		public UpdateFAByArticleNonBETNModel()
		{

		}
	}
}
