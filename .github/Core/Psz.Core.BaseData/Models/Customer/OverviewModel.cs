namespace Psz.Core.BaseData.Models.Customer
{
	public class OverviewModel
	{
		public int Id { get; set; }//kunden Nr
		public int Number { get; set; }// kunden nummer
		public int AdressId { get; set; }// adressen Nr
		public bool? Isarchived { get; set; }
		public string Name { get; set; }//adressen name1
		public string Industry { get; set; }//kunden branche
		public string Adress { get; set; }//adressen Ort
		public string Street { get; set; }//adressen StraBe
		public string Costumer_group { get; set; }//kundengrouppen
		public int ImageID { get; set; }
		public bool IsBoth { get; set; }
		public int Deals { get; set; }
		public GetGeoLocationModel GeoLocation { get; set; }
		// - 2023-04-04
		public int? SupplierNumber { get; set; }
		public int? SupplierId { get; set; }

		// - 2023-04-17
		public int? PreviousNr { get; set; }
		public int? NextNr { get; set; }
		public bool? DELFixiert { get; set; }
		public int? DEL { get; set; }

		public OverviewModel(Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity, Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity,
			Infrastructure.Data.Entities.Tables.BSD.KundenExtensionEntity kundenExtensionEntity, Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity geolocationEntity,
			Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity lieferantenEntity
			)
		{
			string adrStreet = !string.IsNullOrEmpty(adressenEntity.StraBe) && !string.IsNullOrWhiteSpace(adressenEntity.StraBe) ? adressenEntity.StraBe + "," : "";
			string adrZipCode = !string.IsNullOrEmpty(adressenEntity.PLZ_StraBe) && !string.IsNullOrWhiteSpace(adressenEntity.PLZ_StraBe) ? adressenEntity.PLZ_StraBe + "," : "";
			string adrOrt = !string.IsNullOrEmpty(adressenEntity.Ort) && !string.IsNullOrWhiteSpace(adressenEntity.Ort) ? adressenEntity.Ort + "," : "";
			string adrLand = !string.IsNullOrEmpty(adressenEntity.Land) && !string.IsNullOrWhiteSpace(adressenEntity.Land) ? adressenEntity.Land + "." : "";


			var newLoc = Infrastructure.Services.Geocoding.Converter.LocationFromAddress($"");
			Id = kundenEntity.Nr;
			Isarchived = (Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id).IsArchived : false;
			Number = adressenEntity != null && int.TryParse(adressenEntity.Kundennummer.ToString(), out int nummer) ? nummer : -1;
			AdressId = (int)kundenEntity.Nummer;
			Name = adressenEntity?.Name1;
			Industry = kundenEntity.Branche;
			//Adress = adressenEntity?.Land + " " + adressenEntity?.Ort + " " + adressenEntity?.StraBe + " " + adressenEntity?.PLZ_StraBe;
			Adress = $"{adrStreet} {adrZipCode} {adrOrt} {(adrLand.ToLower().Trim() == "d" ? "Deutschland" : adrLand)}".Replace(", ,", ",").Replace(",,", ",").TrimEnd(',');
			Street = adressenEntity.StraBe;
			Costumer_group = kundenEntity.Kundengruppe;
			ImageID = (kundenExtensionEntity != null) ? kundenExtensionEntity.ImageId : 0;
			IsBoth = (adressenEntity.Lieferantennummer.HasValue) ? true : false;
			if(geolocationEntity != null)
			{
				GeoLocation = new GetGeoLocationModel(geolocationEntity);
			}
			SupplierNumber = adressenEntity?.Lieferantennummer;
			SupplierId = lieferantenEntity?.Nr;
			//2024-03-06 00024 PM - FG1(back)

			DELFixiert = kundenEntity.DELFixiert;
			DEL = kundenEntity.DEL;
		}
	}
}
