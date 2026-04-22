using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FAStapelFinalReportModel
	{
		public int Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Termin_bestatigt { get; set; }
		public FAStapelFinalReportModel()
		{

		}
		public FAStapelFinalReportModel(int fa, string article, DateTime? date)
		{
			Fertigungsnummer = fa;
			Artikelnummer = article;
			Termin_bestatigt = date.HasValue ? date.Value.ToString("dd.MM.yyyy") : "";
		}
	}
}