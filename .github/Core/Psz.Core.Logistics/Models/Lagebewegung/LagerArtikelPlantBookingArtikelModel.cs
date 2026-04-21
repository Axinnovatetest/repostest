using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class LagerArtikelPlantBookingArtikelModel
	{
		public int artikelNr { get; set; }
		public string? artikelnummer { get; set; }
		public decimal bestand { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? wereingangId { get; set; }

		//------------------Pour Transfer---------------
		public decimal bestandOriginal { get; set; }
		public int Nr { get; set; }
		public decimal? UbertrageneMenge { get; set; }
		public bool CanTransfer { get; set; }
		public bool? FromRealOrder { get; set; }
		public decimal? TransferableQuantity { get; set; }
		public LagerArtikelPlantBookingArtikelModel()
		{
		}
		public LagerArtikelPlantBookingArtikelModel(Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity lagerArtikelEntity, string Artikelnummer)
		{
			if(lagerArtikelEntity == null)
				return;
			artikelNr = lagerArtikelEntity.artikelNr;
			artikelnummer = Artikelnummer;
			Anzahl = lagerArtikelEntity.Anzahl;
			wereingangId = lagerArtikelEntity.wereingangId;
			FromRealOrder = lagerArtikelEntity.FromRealOrder;
			Nr = lagerArtikelEntity.Nr;
			UbertrageneMenge = lagerArtikelEntity.UbertrageneMenge;

			TransferableQuantity = lagerArtikelEntity.Anzahl - lagerArtikelEntity.UbertrageneMenge;
		}

		public LagerArtikelPlantBookingArtikelModel(Infrastructure.Data.Entities.Tables.Logistics.LagerPlantBookingArtikelEntity lagerArtikelEntity)
		{
			if(lagerArtikelEntity == null)
				return;
			artikelNr = lagerArtikelEntity.artikelNr;
			artikelnummer = lagerArtikelEntity.artikelnummer;
			Anzahl = lagerArtikelEntity.Anzahl;
			wereingangId = lagerArtikelEntity.wereingangId;
			Nr = lagerArtikelEntity.Nr;
			FromRealOrder = lagerArtikelEntity.FromRealOrder;
			
			if(lagerArtikelEntity.transfered ?? false)
			{
				TransferableQuantity = lagerArtikelEntity.UbertrageneMenge ;
				UbertrageneMenge = lagerArtikelEntity.Anzahl - lagerArtikelEntity.UbertrageneMenge;
			}
			else
			{
				TransferableQuantity = lagerArtikelEntity.Anzahl - lagerArtikelEntity.UbertrageneMenge;
				UbertrageneMenge = lagerArtikelEntity.UbertrageneMenge;
			}
		}
	}
}
