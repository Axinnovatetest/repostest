namespace Psz.Core.BaseData.Models.Customer
{
	public class GetGeoLocationModel
	{
		public int Nr { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public int Confidence { get; set; }

		public GetGeoLocationModel() { }
		public GetGeoLocationModel(Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity adressenGeocodingExtension)
		{
			if(adressenGeocodingExtension == null)
				return;

			this.Nr = adressenGeocodingExtension.Nr;
			this.Longitude = (double)adressenGeocodingExtension.Longitude;
			this.Latitude = (double)adressenGeocodingExtension.Latitude;
			this.Confidence = (int)adressenGeocodingExtension.Confidence;
		}
	}
}
