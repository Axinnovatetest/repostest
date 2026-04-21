using System.Collections.Generic;

namespace Psz.Core.CRP.Models.FA
{
	public class UpdateFAByArticleBETNModel
	{
		public string? Index { get; set; }
		public int? BomVersion { get; set; }
		public int? CPVersion { get; set; }
		public List<OpenFAForUpdateModel> FAs { get; set; }

		public UpdateFAByArticleBETNModel()
		{

		}
	}
}
