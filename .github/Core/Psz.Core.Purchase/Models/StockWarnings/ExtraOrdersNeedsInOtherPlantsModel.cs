using iText.Kernel.XMP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class ExtraOrdersNeedsInOtherPlantsModel
	{
		public int Bestellung_Nr { get; set; }
		public int Nr { get; set; }
		public string Lieferant { get; set; }
		public decimal Anzahl { get; set; }
		public string Lagerort { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public ExtraOrdersNeedsInOtherPlantsModel()
		{

		}
		public ExtraOrdersNeedsInOtherPlantsModel(Infrastructure.Data.Entities.Joins.PRS.ExtraOrdersNeedsInOtherPlantsEntity entity)
		{
			Bestatigter_Termin = entity.Bestatigter_Termin;
			Bestellung_Nr = entity.Bestellung_Nr;
			Nr = entity.Nr;
			Lieferant = entity.Lieferant;
			Anzahl = entity.Anzahl ?? 0m;
			Wunschtermin = entity.Wunschtermin;
			Lagerort = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetById(entity.Lagerort_id ?? -1)?.Lagerort;
		}
	}

	public class ExtraOrdersNeedsInOtherPlantsRequestModel
	{
		public int ArtikelNr { get; set; }
		public int Lager { get; set; }
		public bool? ProjectOrders { get; set; }
		public bool? OnlyUnconfirmed { get; set; }
		public DateTime? DateConfirmationBefore { get; set; }
	}
}