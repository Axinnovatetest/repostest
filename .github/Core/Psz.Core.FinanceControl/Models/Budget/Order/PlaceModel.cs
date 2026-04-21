using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class PlaceModel
	{
		public int OrderId { get; set; }
		public string EmailTitle { get; set; }
		public string EmailBody { get; set; }
		public string SupplierEmail { get; set; } // semi-column separated emails
		public string IssuerEmail { get; set; }
		public bool? CcIssuer { get; set; }
		public string OrderPlacementCCEmail { get; set; } // semi-column separated emails
		public List<Files.FilesModel> Files { get; set; }
	}
}
