using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Reporting.Models
{
	public class ABNotAvailable
	{
		public string Name1 { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public List<ReportItem> Items { get; set; }

		public List<ABNotAvailable> GetData(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> view_PrioeinkaufEntity)
		{
			var response = new List<ABNotAvailable>();
			foreach(var item in view_PrioeinkaufEntity)
			{
				var supplier = item.Name1;
				if(response.Find(x => x.Name1.ToLower() == supplier.ToLower()) is null)
				{
					var supplierData = view_PrioeinkaufEntity.FindAll(x => x.Name1 == supplier);

					var ABNotAvailable = new ABNotAvailable();
					ABNotAvailable.Name1 = item.Name1;
					ABNotAvailable.Telefon = item.Telefon;
					ABNotAvailable.Fax = item.Fax;

					ABNotAvailable.Items = supplierData.Select(x => new ReportItem
					{
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						BestellungNr = x.Bestellung_Nr,
						Bezeichnung_1 = x.Bezeichnung_1,
						Datum = x.Datum,
						Lagerort_id = x.Lagerort_id,
						Liefertermin = x.Liefertermin
					}).ToList();

					response.Add(ABNotAvailable);
				}
			}
			return response;
		}
	}
	public class DispositionDateDifference
	{
		public string Name1 { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public List<ReortItemDateDifference> Items { get; set; }

		public List<DispositionDateDifference> GetData(List<Infrastructure.Data.Entities.Views.MTM.View_PSZ_Disposition_Ab_Termin_zu_Spat_sqlEntity> view_PSZ_Disposition_Ab_Termin_Zu_Spat_SqlEntities)
		{
			var response = new List<DispositionDateDifference>();
			foreach(var item in view_PSZ_Disposition_Ab_Termin_Zu_Spat_SqlEntities)
			{
				var supplier = item.Name1;
				if(response.Find(x => x.Name1.ToLower() == supplier.ToLower()) is null)
				{
					var supplierData = view_PSZ_Disposition_Ab_Termin_Zu_Spat_SqlEntities.FindAll(x => x.Name1 == supplier);

					var ABNotAvailable = new DispositionDateDifference();
					ABNotAvailable.Name1 = item.Name1;
					ABNotAvailable.Telefon = item.Telefon;
					ABNotAvailable.Fax = item.Fax;

					ABNotAvailable.Items = supplierData.Select(x => new ReortItemDateDifference
					{
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						BestellungNr = x.Bestellung_Nr,
						Bezeichnung_1 = x.Bezeichnung_1,
						Datum = x.Datum,
						Lagerort_id = x.Lagerort_id,
						Liefertermin = x.Liefertermin,
						Bestatigter_Termin = x.Bestatigter_Termin
					}).ToList();

					response.Add(ABNotAvailable);
				}
			}
			return response;
		}

	}
	public class ReportItem
	{
		public DateTime? Datum { get; set; }
		public int? BestellungNr { get; set; }
		public int? Lagerort_id { get; set; }
		public decimal? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public DateTime? Liefertermin { get; set; }
	}
	public class ReortItemDateDifference: ReportItem
	{
		public DateTime? Bestatigter_Termin { get; set; }
	}
}
