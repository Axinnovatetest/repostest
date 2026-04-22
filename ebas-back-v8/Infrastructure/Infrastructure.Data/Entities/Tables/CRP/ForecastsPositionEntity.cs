using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class ForecastsPositionEntity
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Datum { get; set; }
		public decimal? GesamtPreis { get; set; }
		public int Id { get; set; }
		public int? IdForcast { get; set; }
		public bool? IsOrdered { get; set; }
		public int? Jahr { get; set; }
		public int? KW { get; set; }
		public string Material { get; set; }
		public int? Menge { get; set; }
		public decimal? VKE { get; set; }

		public ForecastsPositionEntity() { }

		public ForecastsPositionEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			GesamtPreis = (dataRow["GesamtPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtPreis"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdForcast = (dataRow["IdForcast"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdForcast"]);
			IsOrdered = (dataRow["IsOrdered"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsOrdered"]);
			Jahr = (dataRow["Jahr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Jahr"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
			VKE = (dataRow["VKE"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKE"]);
		}

		public ForecastsPositionEntity ShallowClone()
		{
			return new ForecastsPositionEntity
			{
				ArtikelNr = ArtikelNr,
				Artikelnummer = Artikelnummer,
				Datum = Datum,
				GesamtPreis = GesamtPreis,
				Id = Id,
				IdForcast = IdForcast,
				IsOrdered = IsOrdered,
				Jahr = Jahr,
				KW = KW,
				Material = Material,
				Menge = Menge,
				VKE = VKE
			};
		}
	}
}

