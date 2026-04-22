using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FA
{
	public class FACRPUpdateListModel
	{
		public int Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Kennzeichen { get; set; }
		public bool? gedruckt { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public string KundenIndex { get; set; }
		public decimal? Zeit { get; set; }
		public DateTime? Datum { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public bool InH1 { get; set; }
		public bool CanUpdate { get; set; } = false;
		public bool InFrozenZone { get; set; } = false;
		public bool IsStarted { get; set; } = false;
		public int? OrderNumber { get; set; }
		public int? OrderId { get; set; }
		public string OrderType { get; set; }
		public decimal? Anzahl { get; set; }
		public FACRPUpdateListModel()
		{

		}
		public FACRPUpdateListModel(Infrastructure.Data.Entities.Joins.FAUpdate.FACRPUpdateEntity entity)
		{
			Fertigungsnummer = entity.Fertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			Kennzeichen = entity.Kennzeichen;
			gedruckt = entity.gedruckt;
			FA_Druckdatum = entity.FA_Druckdatum;
			KundenIndex = entity.KundenIndex;
			Zeit = entity.Zeit;
			Datum = entity.Datum;
			Termin_Bestatigt1 = entity.Termin_Bestatigt1;
			InH1 = entity.InFrozenZone;
			OrderNumber = entity.OrderNumber;
			OrderType = entity.OrderType;
			CanUpdate = entity.CanUpdate;
			IsStarted = entity.IsStarted;
			OrderId = entity.OrderId;
			Anzahl = entity.Anzahl;
		}
	}
	public class FACRPUpdateRequestModel
	{
		public int ArtikelNr { get; set; }
		public List<int> FaList { get; set; }
	}

	public class FACRPUpdateResponseModel
	{
		public int Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Produktionstermin { get; set; }
	}

	public class FACRPNotRequiredROHResponseModel
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal NeededInOtherOrders { get; set; }
		public decimal NeededInOrderToUpdate { get; set; }
		public decimal Diffrence { get; set; }
	}

	public class FACRPNotRequiredROHRequestModel
	{
		public int ArtikelNr { get; set; }
		public List<int> FaList { get; set; }
	}
}