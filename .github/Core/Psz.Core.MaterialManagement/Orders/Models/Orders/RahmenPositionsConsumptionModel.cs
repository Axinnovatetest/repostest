using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class RahmenPositionsConsumptionModel
	{
		public int Nr { get; set; }
		public int? Position { get; set; }
		public int? VorfallNr { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Designation { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? OrderdQuantity { get; set; }
		public decimal? RestQuantity { get; set; }
		public decimal? Consumption { get; set; }
		public decimal? TotalPrice { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public bool Expired { get; set; }
		public string StatusName { get; set; }
		public int? StatusId { get; set; }
		public string Projekt_Nr { get; set; }
		public decimal? NeededInBOM { get; set; }
		public decimal? SumNeeded { get; set; }
		public decimal? Total { get; set; }
		public string Supplier { get; set; }
		public int? SupplierId { get; set; }
		public string DocumentNumber { get; set; }

		public RahmenPositionsConsumptionModel(Infrastructure.Data.Entities.Joins.MTM.Order.RahmensPositionsConsumptionEntity entity)
		{
			Nr = entity.Nr;
			Position = entity.Position;
			VorfallNr = entity.Angebot_Nr;
			ArtikelNr = entity.Artikel_Nr;
			Artikelnummer = entity.Artikelnummer;
			Designation = entity.Bezeichnung1;
			Quantity = entity.OriginalAnzahl;
			OrderdQuantity = entity.Geliefert;
			RestQuantity = entity.Anzahl;
			Consumption = entity.Consumption;
			TotalPrice = entity.GesamtpreisDefault;
			ExpiryDate = entity.GultigBis;
			ExtensionDate = entity.ExtensionDate;
			Expired = (entity.ExtensionDate < DateTime.Now) && entity.StatusId == 2;
			StatusName = entity.StatusName;
			StatusId = entity.StatusId;
			Projekt_Nr = entity.Projekt_Nr;
			NeededInBOM = entity.NeededInBOM;
			SumNeeded = entity.SumNeeded;
			Total = entity.Total;
			Supplier = entity.Supplier;
			SupplierId = entity.SupplierId;
			DocumentNumber = entity.DocumentNumber;
		}
	}

	public class RahmenPositionsConsumptionRequestModel: IPaginatedRequestModel
	{
		public string SearchText { get; set; }
	}
	public class RahmenPositionsConsumptionResponseModel: IPaginatedResponseModel<RahmenPositionsConsumptionModel> { }
}