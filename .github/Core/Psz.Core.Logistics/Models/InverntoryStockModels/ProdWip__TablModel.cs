namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class ProdWip_Tabl_RequestModel: IPaginatedRequestModel
	{
		public string? SearchValue { get; set; }
		public int? LagerId { get; set; }
	}

	public class ProdWipResponseModel
	{
		public decimal? Assembled { get; set; }
		public decimal? Crimped { get; set; }
		public decimal? Cut { get; set; }
		public DateTime Due { get; set; }
		public int FA { get; set; }
		public int Id { get; set; }
		public string? Item { get; set; }
		public decimal? OInspected { get; set; }
		public int OpenQty { get; set; }
		public decimal? Picked { get; set; }
		public decimal? Preped { get; set; }
		public decimal? VInspected { get; set; }
		public string? Step { get; set; }
		public int? ComPercent { get; set; }
		public string? ArtikelNr { get; set; }
		public int IdFa { get; set; }
		public DateTime? InventoryYear { get; set; }
		public int LagerId { get; set; }
		public ProdWipResponseModel() { }

		public ProdWipResponseModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			FA = entity.FA;
			Item = entity.Item;
			OpenQty = entity.OpenQty;
			Due = entity.Due;

			// Conversion des int vers decimal
			Picked = entity.UserPicked;
			Cut = entity.UserCut;
			Preped = entity.UserPreped;
			Assembled = entity.UserAssembled;
			Crimped = entity.UserCrimped;
			VInspected = entity.UserElectricalInspected;
			OInspected = entity.UserOpticalInspected;

			ArtikelNr = entity.ArtikelNr;
			IdFa = entity.IdFa;
			LagerId = entity.LagerId;
			InventoryYear = entity.InventoryYear;
		}
	}

	public class ProdWip_Tabl_Reponse_Model: IPaginatedResponseModel<ProdWipResponseModel>
	{
	}
}
