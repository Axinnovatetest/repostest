using Psz.Core.Common.Models;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class GetPlantBookingsRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
		public int LagerId { get; set; }
	}
	public class GetPlantBookingsResponseModel
	{
		public int? Aktiv { get; set; }
		public int? PackagingNr { get; set; }
		public string ArtikelNummer { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? TotalQuantity { get; set; }
		public decimal? RestQuantity { get; set; }
		//Add 04-04-2025 By Ridha
		public int? Status { get; set; }
		public string Inspector { get; set; }
		public string Remarque { get; set; }
		public DateTime? Datum { get; set; }

		public GetPlantBookingsResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNMinimalEntity eignanskontrolleEntity)
		{
			if(eignanskontrolleEntity == null)
				return;
			Aktiv = eignanskontrolleEntity.Aktiv;
			PackagingNr = eignanskontrolleEntity.Nummer_Verpackung;
			ArtikelNummer = eignanskontrolleEntity.Artikelnummer;
			Quantity = eignanskontrolleEntity.Menge;
			TotalQuantity = eignanskontrolleEntity.Gesamtmenge;
			RestQuantity = eignanskontrolleEntity.Restmenge_Rolle_PPS;
			Status = eignanskontrolleEntity.Status_Rolle;
			Inspector = eignanskontrolleEntity.Inspector;
			Remarque = eignanskontrolleEntity.Remarque;
			Datum = eignanskontrolleEntity.Datum;
		}

		public GetPlantBookingsResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity eignanskontrolleEntity)
		{
			if(eignanskontrolleEntity == null)
				return;
			PackagingNr = eignanskontrolleEntity.Nummer_Verpackung;
			ArtikelNummer = eignanskontrolleEntity.Artikelnummer;
			Quantity = eignanskontrolleEntity.Menge;
			TotalQuantity = eignanskontrolleEntity.Gesamtmenge;
			RestQuantity = eignanskontrolleEntity.Restmenge_Rolle_PPS;
			Status = eignanskontrolleEntity.Status_Rolle;
		}
		public GetPlantBookingsResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity eignanskontrolleEntity)
		{
			if(eignanskontrolleEntity == null)
				return;
			PackagingNr = eignanskontrolleEntity.Anzahl_Verpackungen;
			ArtikelNummer = eignanskontrolleEntity.Artikelnummer;
			Quantity = eignanskontrolleEntity.Menge;
			TotalQuantity = eignanskontrolleEntity.Gesamtmenge;
			RestQuantity = eignanskontrolleEntity.Restmenge_Rolle_PPS;
			Status = eignanskontrolleEntity.Status_Rolle;
		}
		public GetPlantBookingsResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_ALEntity eignanskontrolleEntity)
		{
			if(eignanskontrolleEntity == null)
				return;
			PackagingNr = eignanskontrolleEntity.Anzahl_Verpackungen;
			ArtikelNummer = eignanskontrolleEntity.Artikelnummer;
			Quantity = eignanskontrolleEntity.Menge;
			TotalQuantity = eignanskontrolleEntity.Gesamtmenge;
			RestQuantity = eignanskontrolleEntity.Restmenge_Rolle_PPS;
			Status = eignanskontrolleEntity.Status_Rolle;
		}
	}



	public class GetPlantResponseModel: IPaginatedResponseModel<GetPlantBookingsResponseModel>
	{
	}
}





