using System.Collections.Generic;

namespace Infrastructure.Services.Geocoding
{
	using global::Geocoding.Microsoft;
	using System.Linq;
	using System.Threading.Tasks;

	public static class Converter
	{
		public static LocationModel LocationFromAddress(string address)
		{
			try
			{
				if(string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
					return new LocationModel { };

				var geocoder = new BingMapsGeocoder("AvUBspFxQu4NmmotC1itwr6Uw-YgEg7VwX0hnNuZQnOIIKD_XFpyi6EWpUhJUsXC");
				IEnumerable<BingAddress> addresses = Task.Run(async () => await geocoder.GeocodeAsync(address)).Result;

				if(addresses == null || addresses.Count() == 0)
					return new LocationModel { };

				var country = addresses
						// .Where(a => a.Confidence != ConfidenceLevel.Unknown && a.Confidence != ConfidenceLevel.Low)
						?.OrderBy(x => x.Confidence)
						?.First()
						;
				return new LocationModel
				{
					Longitude = country.Coordinates.Longitude,
					Latitude = country.Coordinates.Latitude,
					Confidence = (int)country.Confidence
				};
			} catch(System.Exception)
			{
				return new LocationModel { };
				throw;
			}
		}
		public static List<LocationModel> GetLocations(List<string> addresses)
		{
			return addresses.Select(x => LocationFromAddress(x)).ToList();
		}

	}

}
