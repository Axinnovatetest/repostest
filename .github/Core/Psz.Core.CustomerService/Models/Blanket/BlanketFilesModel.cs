using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class BlanketFilesModel
	{
		public int AngeboteNr { get; set; }
		public List<int> BlanketsFileIds { get; set; }
		public List<FilesModel> Files { get; set; }

		public BlanketFilesModel()
		{ }
	}
}
