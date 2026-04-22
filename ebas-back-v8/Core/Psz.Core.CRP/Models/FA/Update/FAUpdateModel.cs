namespace Psz.Core.CRP.Models.FA
{
	public class FAUpdateModel
	{
		//for with versionning
		public string Index { get; set; }
		public int? BOM_version { get; set; }
		public int? CP_version { get; set; }
		//commun
		public int Fertigungsnummer { get; set; }
		//for without versionning
		public bool? stucklisten { get; set; }
		public bool? kundenIndex { get; set; }
		public FAUpdateModel()
		{

		}
	}
}