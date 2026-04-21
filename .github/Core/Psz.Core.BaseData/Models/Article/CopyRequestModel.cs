using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class CopyRequestModel
	{
		public string ArticleNumber { get; set; }
		public int ProductionCountryId { get; set; }
		public int ProductionSiteId { get; set; }
		// -
		public int CopyFromArticleId { get; set; }
		public bool WithBOM { get; set; } = true;
		public bool IsArticleNumberCustom { get; set; } = false;
		public string ArticleNumberCustom { get; set; }
		// -
		public string CustomerItemNumber { get; set; }
		public string ProductionCountryCode { get; set; }
		public string ProductionCountryName { get; set; }
		public string ProductionSiteCode { get; set; }
		public string ProductionSiteName { get; set; }
		public string CustomerItemNumberSequence { get; set; }
		public string Designation { get; set; }
		public string CustomerItemIndex { get; set; }
		public DateTime? CustomerItemIndexDate { get; set; }
		public string CustomerItemIndexSequence { get; set; }

		// - 2023-01-22 - Default Edi
		public bool IsEdiDefault { get; set; } = false;
		// - 2023-08-24
		public bool? CocActive { get; set; }
		public string CocVersion { get; set; }
		// - 2024-02-28 - Capital // E-Drawing
		public bool IsEDrawing { get; set; } = false;
	}
}
