using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.Statistics
{
	public class CRPAuswertungRahmenFGArtikelModel
	{
		public int? Rahmen { get; set; }
		public string Type { get; set; }
		public string Kunde { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Originalmenge { get; set; }
		public decimal? Restmenge { get; set; }
		public decimal? Einzelpreis { get; set; }
		public decimal? PreisRestmenge { get; set; }
		public DateTime? Enddatum { get; set; }
		public string Status { get; set; }
		public CRPAuswertungRahmenFGArtikelModel()
		{

		}
		public CRPAuswertungRahmenFGArtikelModel(Infrastructure.Data.Entities.Joins.CRP.CRPAuswertungRahmenFGArtikelEntity entity)
		{
			Rahmen = entity.Rahmen;
			Type = entity.Type;
			Kunde = entity.Kunde;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung1 = entity.Bezeichnung1;
			Originalmenge = entity.Originalmenge;
			Restmenge = entity.Restmenge;
			Einzelpreis = entity.Einzelpreis;
			PreisRestmenge = entity.PreisRestmenge;
			Enddatum = entity.Enddatum;
			Status = entity.Status;
		}
	}
}