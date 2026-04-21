using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderFileModel
	{

		public int Id_Order { get; set; }
		public List<int> OrderFileIds { get; set; }
		public List<Files.FilesModel> Files { get; set; }

		public OrderFileModel()
		{ }
	}
}
