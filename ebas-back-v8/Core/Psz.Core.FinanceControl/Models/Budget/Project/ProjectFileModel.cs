using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Project
{
	public class ProjectFileModel
	{

		public int ProjectId { get; set; }
		public List<int> ProjectFileIds { get; set; }
		public List<FileModel> Files { get; set; }

		public ProjectFileModel()
		{ }
	}
}
