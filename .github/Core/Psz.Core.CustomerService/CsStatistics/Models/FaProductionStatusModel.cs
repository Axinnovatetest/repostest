using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class FaProductionStatusRequestModel
	{
		public DateTime? DateFrom { get; set; } = null;
		public DateTime? DateTo { get; set; } = DateTime.Today;
	}
	public class FaProductionStatusResponseModel
	{
		public int Anzahl { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public int Fertigungsnummer { get; set; }
		public int Flag { get; set; }
		public int ID { get; set; }
		public int Lagerort_id { get; set; }
		public DateTime? Produktionstermin { get; set; }
		public DateTime? Werktermin { get; set; }
		public string Status { get; set; }
		public string Bemerkung { get; set; }
		public FaProductionStatusResponseModel(Infrastructure.Data.Entities.Joins.CTS.FaProductionStatusEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Anzahl = entity.Anzahl ?? 0;
			Artikel_Nr = entity.Artikel_Nr;
			Artikelnummer = entity.Artikelnummer;
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Flag = entity.Flag;
			ID = entity.ID;
			Lagerort_id = entity.Lagerort_id ?? 0;
			Produktionstermin = entity.Produktionstermin;
			Werktermin = entity.Werktermin;
			Bemerkung = entity.Bemerkung;
			Status = entity.Status;
		}
	}
	public class FaProductionStatusSearchRequestModel: IPaginatedRequestModel
	{
		public int Horizon { get; set; } = 1;
		public bool? Cumulative { get; set; } = false;
		public string SearchValue { get; set; }
		public string ProductionStatus { get; set; }
	}

	public class FaProductionStatusSearchResponseModel: IPaginatedResponseModel<FaProductionStatusResponseModel>
	{
	}
}
