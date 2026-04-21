using System;


namespace Psz.Core.CustomerService.Models.InsideSalesWerksterminUpdates
{
	public class UpdateWerksterminRequestModel
	{
		public int FertigungNumber { get; set; }
		public bool? InsConfirmation { get; set; }
		public DateTime? OldWorkDate { get; set; }
		public DateTime? NewWorkDate { get; set; }
		public bool? ReasonCapacity { get; set; }
		public string? ReasonCapacityComments { get; set; }
		public bool? ReasonClarification { get; set; }
		public string? ReasonClarificationComments { get; set; }
		public bool? ReasonDefective { get; set; }
		public string? ReasonDefectiveComments { get; set; }
		public bool? ReasonMaterial { get; set; }
		public string? ReasonMaterialComments { get; set; }
		public bool? ReasonQuality { get; set; }
		public string? ReasonQualityComments { get; set; }
		public bool? ReasonStatusP { get; set; }
	}

	public class InsideSalesWerkserminUpdatesModel
	{
		public int Id { get; set; }

		public int? FertigungId { get; set; }
		public int? FertigungNumber { get; set; }
		public bool? InsConfirmation { get; set; }
		public DateTime? NewWorkDate { get; set; }
		public DateTime? OldWorkDate { get; set; }
		public DateTime? EditDate { get; set; }
		public bool? ReasonCapacity { get; set; }
		public string ReasonCapacityComments { get; set; }
		public bool? ReasonClarification { get; set; }
		public string ReasonClarificationComments { get; set; }
		public bool? ReasonDefective { get; set; }
		public string ReasonDefectiveComments { get; set; }
		public bool? ReasonMaterial { get; set; }
		public string ReasonMaterialComments { get; set; }
		public bool? ReasonQuality { get; set; }
		public string ReasonQualityComments { get; set; }
		public bool? ReasonStatusP { get; set; }
		public string? CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public int? CustomerOrderNumber { get; set; }
		public string? ArtikelNummer { get; set; }
		public int? ArticleId { get; set; }
	}

	public class InsConfirmationWerkterminModel
	{
		public int WerkterminId { get; set; }
		public bool InsConfirmation { get; set; }
	}
}
