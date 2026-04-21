using static Psz.Core.MaterialManagement.Enums.OrderEnums;

namespace Psz.Core.MaterialManagement.Orders.Models.Wareneingang
{
	public class CreateReportRequestModel
	{
		public int WareneingangId { get; set; }
		public int? WareneingangNr { get; set; }
		public int LagerortId { get; set; }
		public WareneingangReportTypes ReportType { get; set; }
		public int BestellteArtikelId { get; set; }
		public string MhdDate { get; set; }
		public bool Watermark { get; set; } = false;
	}
}
