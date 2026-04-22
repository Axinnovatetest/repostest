using System;

namespace Psz.Core.BaseData.Models.Article.Overview
{
	public class BlanketResponseModel
	{
		public int RaId { get; set; }
		public string RaNumber { get; set; }
		public string RaProjectNumber { get; set; }
		public string RaStatus { get; set; }
		public DateTime RaCreationTime { get; set; }
		public string RaSupplierName { get; set; }
		public string RaCustomerName { get; set; }
		public string RaType { get; set; }
		public int PosId { get; set; }
		public int PosNumber { get; set; }
		public int PosArticleId { get; set; }
		public string PosArticleNumber { get; set; }
		public decimal PosOriginalQuantity { get; set; }
		public decimal PosOrderedQuantity { get; set; }
		public decimal PosUnitPrice { get; set; }
		public string PosCurrency { get; set; }
		public string PosCurrencySymbol { get; set; }
		public DateTime PosStartTime { get; set; }
		public DateTime PosEndTime { get; set; }

		public BlanketResponseModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity ra,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity raExtension,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity raPos,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity raPosExtension)
		{
			if(ra == null)
			{
				return;
			}

			RaId = ra.Nr;
			RaNumber = ra.Bezug;
			RaProjectNumber = ra.Projekt_Nr;
			RaStatus = raPos?.erledigt_pos == true ? "Closed" : raExtension?.StatusName;
			RaCreationTime = ra.Datum ?? DateTime.MinValue;
			RaSupplierName = raExtension?.SupplierName;
			RaCustomerName = raExtension?.CustomerName;
			RaType = raExtension?.BlanketTypeName;
			PosId = raPos?.Nr ?? -1;
			PosNumber = raPos?.Position ?? 0;
			PosArticleId = raPos?.ArtikelNr ?? -1;
			PosArticleNumber = raPosExtension?.Material;
			PosOriginalQuantity = raPos?.OriginalAnzahl ?? 0;
			PosOrderedQuantity = raPos?.Anzahl ?? 0;
			PosUnitPrice = raPosExtension?.PreisDefault ?? 0;
			PosCurrency = raPosExtension?.WahrungName;
			PosCurrencySymbol = raPosExtension?.WahrungSymbole;
			PosStartTime = raPosExtension?.GultigAb ?? DateTime.MinValue;
			PosEndTime = raPosExtension?.ExtensionDate ?? DateTime.MinValue; // - 2025-08-11 - Fr. Hejdukova
		}
	}
}
