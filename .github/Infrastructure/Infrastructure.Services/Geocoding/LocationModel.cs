namespace Infrastructure.Services.Geocoding
{
	public class LocationModel
	{
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public int Confidence { get; set; }


		public LocationModel()
		{

		}
	}
}
