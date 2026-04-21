using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class CapacityRequestModel
	{
		public int Horizon { get; set; } = 1;
		public bool? Cumulative { get; set; } = false;
	}
	public class CapacityResponseModel
	{
		public int Artikel_Nr { get; set; }
		public decimal abAnzahl { get; set; }
		public int abPosCount { get; set; }
		public string Artikelnummer { get; set; }
		public decimal Bestand { get; set; }
		public int faAnzahl { get; set; }
		public int faCount { get; set; }
		public int lpAnzahl { get; set; }
		public int lpCount { get; set; }
		public int? frcAnzahl { get; set; }
		public int? frcCount { get; set; }
		public decimal Mindestbestand { get; set; }
		public decimal Summe { get; set; }

		// - 
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public string KWH { get; set; }
		public CapacityResponseModel(Infrastructure.Data.Entities.Joins.CTS.CapacityEntity entity,
			DateTime? dateFrom, DateTime? dateTo, string kWH)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Artikel_Nr = entity.Artikel_Nr;
			abAnzahl = entity.abAnzahl ?? 0;
			abPosCount = entity.abPosCount ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Bestand = entity.Bestand ?? 0;
			faAnzahl = entity.faAnzahl ?? 0;
			faCount = entity.faCount ?? 0;
			lpAnzahl = entity.lpAnzahl ?? 0;
			lpCount = entity.lpCount ?? 0;
			frcAnzahl = entity.frcAnzahl ?? 0;
			frcCount = entity.frcCount ?? 0;
			Mindestbestand = entity.Mindestbestand ?? 0;
			Summe = entity.Summe ?? 0;



			DateFrom = dateFrom;
			DateTo = dateTo;
			KWH = kWH;
		}
	}
	public class CapacityAbFaRequestModel
	{
		public int ArticleId { get; set; }
		public int Horizon { get; set; }
		public bool? Cumulative { get; set; } = false;
	}
	public class CapacityAbFaResponseModel
	{
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }

		public int AbId { get; set; }
		public string AbBezug { get; set; }
		public string AbNummer { get; set; }
		public string AbCustomer { get; set; }
		public string AbPosition { get; set; }
		public decimal AbAnzahl { get; set; }

		public int FaId { get; set; }
		public string FaNummer { get; set; }
		public string FaStatus { get; set; }
		public string FaLager { get; set; }
		public int FaAnzahl { get; set; }
		public CapacityAbFaResponseModel(Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Artikel_Nr = entity?.Artikel_Nr ?? 0;
			Artikelnummer = entity?.Artikelnummer;
			AbId = entity?.AbId ?? 0;
			AbBezug = entity?.AbBezug;
			AbNummer = entity?.AbNummer;
			AbCustomer = entity?.AbCustomer;
			AbPosition = entity?.AbPosition;
			AbAnzahl = entity?.AbAnzahl ?? 0;
			FaId = entity?.FaId ?? 0;
			FaNummer = entity?.FaNummer;
			FaStatus = entity?.FaStatus;
			FaLager = entity?.FaLager;
			FaAnzahl = entity?.FaAnzahl ?? 0;
		}
	}
	public class CapacityLpResponseModel
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }


		public int? HeaderId { get; set; }
		public int? LineItemId { get; set; }
		public int? LineItemPlanId { get; set; }
		public string Nummer { get; set; }
		public string Customer { get; set; }
		public string Position { get; set; }
		public decimal? Anzahl { get; set; }
		public bool IsManual { get; set; } = false;
		public int PSZCustomernumber { get; set; }
		public CapacityLpResponseModel(Infrastructure.Data.Entities.Joins.CTS.CapacityLPEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Artikel_Nr = entity?.Artikel_Nr ?? 0;
			Artikelnummer = entity?.Artikelnummer;
			HeaderId = entity.HeaderId ?? 0;
			LineItemId = entity.LineItemId ?? 0;
			LineItemPlanId = entity.LineItemPlanId ?? 0;
			Nummer = entity.Nummer;
			Customer = entity.Customer;
			Position = entity.Position;
			Anzahl = entity.Anzahl ?? 0;
			IsManual = entity.IsManual ?? false;
			PSZCustomernumber = entity.PSZCustomernumber ?? 0;
		}
	}
	public class CapacityFcResponseModel
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }


		public int? Id { get; set; }
		public int? PositionId { get; set; }
		public string CustomerName { get; set; }
		public DateTime? PositionDate { get; set; }
		public decimal? Anzahl { get; set; }
		public CapacityFcResponseModel(Infrastructure.Data.Entities.Joins.CTS.CapacityFCEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Artikel_Nr = entity?.Artikel_Nr;
			Artikelnummer = entity?.Artikelnummer;
			Id = entity?.Id;
			PositionId = entity?.PositionId;
			CustomerName = entity?.CustomerName;
			PositionDate = entity?.PositionDate;
			Anzahl = entity?.Anzahl;
		}
	}
}
