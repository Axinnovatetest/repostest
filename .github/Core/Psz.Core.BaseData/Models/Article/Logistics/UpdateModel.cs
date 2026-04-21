using System;

namespace Psz.Core.BaseData.Models.Article.Logistics
{
	public class UpdateStockRequestModel
	{
		public int ID { get; set; }
		public string LagerName { get; set; }
		public int LagerId { get; set; }
		public int ArticleId { get; set; }
		public string KundenIndex { get; set; }
		public decimal NewStockQuantity { get; set; }
	}

	public class UpdateCCIDRequestModel
	{
		public int ID { get; set; }
		public string LagerName { get; set; }
		public bool NewCCID { get; set; }
		public DateTime? NewCCIDDate { get; set; }
		public int LagerId { get; set; }
		public string Kunde_Index { get; set; }
		public int ArticleId { get; set; }
	}
	public class UpdateProposalRequestModel
	{
		public int ID { get; set; }
		public string LagerName { get; set; }
		public bool NewProposal { get; set; }
		public int LagerId { get; set; }
		public string Kunde_Index { get; set; }
		public int ArticleId { get; set; }
	}
}
