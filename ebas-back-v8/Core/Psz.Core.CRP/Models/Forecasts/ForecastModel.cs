using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.Forecasts
{
	public class ForecastModel
	{
		public int? IdNextVersion { get; set; }
		public int? IdPreviousVersion { get; set; }
		public int ActualVersion { get; set; }
		public ForecastHeaderModel Header { get; set; }
		public IEnumerable<ForecastPositionModel> Positions { get; set; }
	}

	public class ForecastHeaderModel
	{
		public int Id { get; set; }
		public int KundenNr { get; set; }
		public int KundenNummer { get; set; }
		public string? Kunden { get; set; }
		public string? Type { get; set; }
		public int ForcastTypeId { get; set; }
		public DateTime Datum { get; set; }
		public int? Versions { get; set; }
		public int IdLastVersion { get; set; }
		public int? IdNextVersion { get; set; }
		public int? IdPreviousVersion { get; set; }
		public ForecastHeaderModel()
		{

		}
		public ForecastHeaderModel(Infrastructure.Data.Entities.Tables.CRP.ForecastsEntity entity)
		{
			Id = entity.Id;
			Type = entity.Type;
			Kunden = entity.kunden;
			KundenNummer = entity.kundennummer ?? -1;
			Datum = entity.Datum ?? DateTime.Today;
			Versions = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetVersionsNumber(entity.kundennummer ?? -1, entity.TypeId ?? -1);
			IdLastVersion = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetIdByKundenAndTypeAndMaxVesrion(entity.kundennummer ?? -1, entity.TypeId ?? -1);
		}
	}
	public class ForecastPositionModel
	{
		public int Id { get; set; }
		public int IdForcast { get; set; }
		public int ArtikelNr { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Material { get; set; }
		public DateTime? Datum { get; set; }
		public int? Menge { get; set; }
		public int? Jahr { get; set; }
		public int? KW { get; set; }
		public decimal? VKE { get; set; }
		public decimal? Gesampreis { get; set; }
		public bool IsOrdered { get; set; }
		public ForecastPositionModel()
		{

		}
		public ForecastPositionModel(Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity entity)
		{
			Id = entity.Id;
			IdForcast = entity.IdForcast ?? -1;
			ArtikelNr = entity.ArtikelNr ?? -1;
			Artikelnummer = entity.Artikelnummer;
			Material = entity.Material;
			Datum = entity.Datum;
			Menge = entity.Menge;
			Jahr = entity.Jahr;
			KW = entity.KW;
			VKE = entity.VKE;
			Gesampreis = entity.GesamtPreis;
			IsOrdered = entity.IsOrdered ?? false;
		}
	}
	public class ForecastPositonsRequestModel: IPaginatedRequestModel
	{
		public int Id { get; set; }
		public string SearchText { get; set; }
	}
	public class ForecastPositionsResponseModel: IPaginatedResponseModel<ForecastPositionModel> { }
}
