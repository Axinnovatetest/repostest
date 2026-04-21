namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class RohInProductionRequestModel: IPaginatedRequestModel
	{
		public string? SearchValue { get; set; }
		public int? LagerId { get; set; }
		public int WarenType { get; set; }
	}
	public class RohInProductionResponseModel
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal MengeInProduktion { get; set; }
		public int ID { get; set; }
		public string Status { get; set; }
		public RohInProductionResponseModel() { }
		public RohInProductionResponseModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity entity)
		{
			if(entity == null)
				return;

			ArtikelNr=entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			MengeInProduktion = entity.MengeInProduktion;
			Status = entity.StatusSpule;
			ID = entity.IdSpule;
		}
	}
	public class RohInProductionPaginatedReponseModel: IPaginatedResponseModel<RohInProductionResponseModel>
	{
	}
}
