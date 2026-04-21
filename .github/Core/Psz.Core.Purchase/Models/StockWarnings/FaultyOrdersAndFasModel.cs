using Psz.Core.Common.Models;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class FaultyRequestModel: IPaginatedRequestModel
	{
		public int? Unit { get; set; }
		public int? ArtiekNr { get; set; }
		public string? SearchTerms { get; set; }
	}

	public class FaultyOrdersModel
	{
		public int Nr { get; set; }
		public int Bestellung_Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string Supplier { get; set; }
		public FaultyOrdersModel(Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity entity)
		{
			Nr = entity.Nr;
			Bestellung_Nr = entity.Bestellung_Nr;
			Liefertermin = entity.Bestatigter_Termin ?? default(DateTime);
			Supplier = entity.Supplier;
			Bestellung_Nr = entity.Bestellung_Nr;
		}
	}
	public class FaultyOrdersResponseModel: IPaginatedResponseModel<FaultyOrdersModel> { }

	public class FaultyFasModel
	{
		public int Fertigungsnummer { get; set; }
		public int ID { get; set; }
		public DateTime? Termin_Bestatigt { get; set; }
		public FaultyFasModel(Infrastructure.Data.Entities.Joins.MTM.Order.GetFaultyFasEntity entity)
		{
			Fertigungsnummer = entity.Fertigungsnummer;
			ID = entity.ID;
			Termin_Bestatigt = entity.Termin_Bestätigt ?? default(DateTime);
		}
	}

	public class FaultyFasResponseModel: IPaginatedResponseModel<FaultyFasModel> { }
}
