namespace Psz.Core.BaseData.Models.Supplier
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
		public string Supplier_group { get; set; }//kundengrouppen
		public int ImageID { get; set; }
		public bool IsBoth { get; set; }
		public int Deals { get; set; }
		public string CustomerPszNumber { get; set; }
		public GetGeoLocationModel GeoLocation { get; set; }
		// - 2023-04-04
		public int? CustomerNumber { get; set; }
		public int? CustomerId { get; set; }
		// - 2023-04-19 
		public int? PreviousNr { get; set; }
		public int? NextNr { get; set; }
		public OverviewModel()
		{

		}

		public OverviewModel(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity lieferentenEntity, Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity,
			Infrastructure.Data.Entities.Tables.BSD.LieferantenExtensionEntity lieferantenExtensionEntity, Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity geolocationEntity,
			Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity
			)
		{
			string adrStreet = !string.IsNullOrEmpty(adressenEntity.StraBe) && !string.IsNullOrWhiteSpace(adressenEntity.StraBe) ? adressenEntity.StraBe + "," : "";
			string adrZipcode = !string.IsNullOrEmpty(adressenEntity.PLZ_StraBe) && !string.IsNullOrWhiteSpace(adressenEntity.PLZ_StraBe) ? adressenEntity.PLZ_StraBe + "," : "";
			string adrOrt = !string.IsNullOrEmpty(adressenEntity.Ort) && !string.IsNullOrWhiteSpace(adressenEntity.Ort) ? adressenEntity.Ort + "," : "";
			string adrLand = !string.IsNullOrEmpty(adressenEntity.Land) && !string.IsNullOrWhiteSpace(adressenEntity.Land) ? adressenEntity.Land + "." : "";

			Id = lieferentenEntity.Nr;
			Isarchived = (Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Id).IsArchived : false;
			Number = adressenEntity != null && int.TryParse(adressenEntity.Lieferantennummer.ToString(), out int nummer) ? nummer : -1;
			AdressId = (int)lieferentenEntity.Nummer;
			Name = !string.IsNullOrEmpty(adressenEntity?.Name1)
					? adressenEntity?.Name1
					: !string.IsNullOrEmpty(adressenEntity?.Name2)
						? adressenEntity?.Name2
						: adressenEntity?.Name3;
			Industry = lieferentenEntity.Branche;
			//Adress = adressenEntity?.Land + " " + adressenEntity?.Ort + " " + adressenEntity?.StraBe + " " + adressenEntity?.PLZ_StraBe;
			Adress = $"{adrStreet} {adrZipcode} {adrOrt} {(adrLand.ToLower().Trim() == "d" ? "Deutschland" : adrLand)}".Replace(", ,", ",").Replace(",,", ",").TrimEnd(',');

			Street = adressenEntity.StraBe;
			Supplier_group = lieferentenEntity.Lieferantengruppe;
			ImageID = (lieferantenExtensionEntity != null) ? lieferantenExtensionEntity.ImageId : 0;
			IsBoth = (adressenEntity.Kundennummer.HasValue) ? true : false;
			CustomerPszNumber = lieferentenEntity?.Kundennummer_Lieferanten;
			if(geolocationEntity != null)
			{
				GeoLocation = new GetGeoLocationModel(geolocationEntity);
			}
			CustomerNumber = adressenEntity?.Kundennummer;
			CustomerId = kundenEntity?.Nr;
		}
	}
}
