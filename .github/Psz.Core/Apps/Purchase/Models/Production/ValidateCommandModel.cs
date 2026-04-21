using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Production
{
	public class ValidateCommandModel
	{
		public int PositionId { get; set; }
		public int ManufacturingFacilityId { get; set; }
		public int OriginalArticleId { get; set; }
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
		public string Customer { get; set; }
		// - 2022-10-24 
		public int HBGFAPositionId { get; set; }
		public ValidateCommandModel()
		{

		}
	}
	public class ValidateCommandWUbgFaRequestModel
	{
		public int PositionId { get; set; }
		public int? ManufacturingFacilityId { get; set; }
		public int OriginalArticleId { get; set; }
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
		public string Customer { get; set; }
		// - 2022-10-24 
		public int HBGFAPositionId { get; set; }
		public bool CreateUBGFas { get; set; } = false;
		public List<UbgFaItem> UbgFaItems { get; set; }
		public string Remarks { get; set; }
		public decimal Surchagre { get; set; }
	}
	public class UbgFaItem
	{
		public int ArticleId { get; set; }
		public DateTime ProdDate { get; set; }
		public int? ProdLager { get; set; }
		public int DestLager { get; set; }
		public decimal ProdQuantity { get; set; }
		public bool ProdUBG { get; set; }
		public bool Checked { get; set; } = false;

	}
}
