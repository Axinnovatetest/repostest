
namespace Psz.Core.CRP.Models.FA.Update
{
	public class AllOpenFAForUpdateModel
	{
		//for non versionning
		public bool? Stucklisten { get; set; }
		public bool? KundenIndex { get; set; }
		//for versionning
		public string? Index { get; set; }
		public int? BomVersion { get; set; }
		public int? CPVersion { get; set; }
		public List<OpenFAForUpdateModel>? FAWithVersionning { get; set; }
		public List<OpenFAForUpdateModel>? FAWithoutVersionning { get; set; }
		public AllOpenFAForUpdateModel()
		{

		}
	}
}
