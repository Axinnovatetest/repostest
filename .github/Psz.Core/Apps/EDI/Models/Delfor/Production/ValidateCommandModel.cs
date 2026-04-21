using System;

namespace Psz.Core.Apps.EDI.Models.Delfor.Production
{
	public class ValidateCommandModel
	{
		public int PositionId { get; set; }
		public int ManufacturingFacilityId { get; set; }
		public int? OriginalArticleId { get; set; }
		public DateTime ProductionDate { get; set; }
		public string TypeRemarks { get; set; }
		public bool TechnicalCommand { get; set; }
		public bool FirstSample { get; set; }
		public decimal Price { get; set; }
		public int StorageId { get; set; }
		public string User { get; set; }
		public bool Storage_Subassembly { get; set; }
		public int TechnicianId { get; set; }
		public string TechnicianName { get; set; }
		// - 2022-10-24
		public int HBGFAPositionId { get; set; }
	}
}
