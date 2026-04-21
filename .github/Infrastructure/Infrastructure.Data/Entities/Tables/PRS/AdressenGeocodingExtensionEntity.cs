using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class AdressenGeocodingExtensionEntity
	{
		public int? Confidence { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		public int Nr { get; set; }
		public DateTime? UpdateDate { get; set; }

		public AdressenGeocodingExtensionEntity() { }

		public AdressenGeocodingExtensionEntity(DataRow dataRow)
		{
			Confidence = (dataRow["Confidence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Confidence"]);
			Latitude = (dataRow["Latitude"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Latitude"]);
			Longitude = (dataRow["Longitude"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Longitude"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			UpdateDate = (dataRow["UpdateDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateDate"]);
		}
	}
}

