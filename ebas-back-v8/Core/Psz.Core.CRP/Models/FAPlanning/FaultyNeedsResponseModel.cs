
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.FAPlanning
{
	public class FaultyNeeds
	{
		public int Nr { get; set; }
		public int? VorfallNr { get; set; }
		public string DockNummer { get; set; }
		public DateTime? Liefertermin_RSD { get; set; }
		public decimal? MengeOffen { get; set; }
		public int CustomerNumber { get; set; }
		public string DocumentType { get; set; }
		public bool? IsDelforManual { get; set; } = null;
		public FaultyNeeds(Infrastructure.Data.Entities.Joins.CRP.FaultyNeedsEntity entity)
		{
			Nr = entity.Id;
			VorfallNr = entity.vorfallNr;
			DockNummer = entity.DocumentNumber;
			Liefertermin_RSD = entity.DeliveryDate;
			MengeOffen = entity.Quantity;
			CustomerNumber = entity.CustomerNumber ?? 0;
			DocumentType = entity.Type;
			IsDelforManual = entity.IsManual;
		}
	}
	public class FaultyNeedsResponseModel: IPaginatedResponseModel<FaultyNeeds> { }
	public class FaultyNeedsResquestModel: IPaginatedRequestModel
	{
		public string SearchTerms { get; set; } = "";
		public bool? Ubg { get; set; } = false;
	}
}