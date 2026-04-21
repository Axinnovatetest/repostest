using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FANotUpdatedwunshUserModel
	{
		public string Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string ProduKtionstermin { get; set; }
		public string Gedruckt { get; set; }
		public string Begonnen { get; set; }
		public string Geschnitten { get; set; }
		public string Termin_error { get; set; }
		public string Gestartet { get; set; }
		public string NotOpen { get; set; }
		public FANotUpdatedwunshUserModel()
		{

		}
		public FANotUpdatedwunshUserModel(Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity entity)
		{
			if(entity == null)
				return;
			Fertigungsnummer = entity.Fertigungsnummer.HasValue ? entity.Fertigungsnummer.Value.ToString() : "";
			Artikelnummer = entity.Artikelnummer;
			ProduKtionstermin = entity.Termin_Bestätigt1.HasValue ? entity.Termin_Bestätigt1.Value.ToString("dd/MM/yyyy") : "";
			Gedruckt = entity.gedruckt.HasValue && entity.gedruckt.Value ? "X" : "";
			Begonnen = entity.Begonnen.HasValue && entity.Begonnen.Value ? "X" : "";
			Geschnitten = (entity.Geschnitten.HasValue && entity.Geschnitten.Value
				|| (entity.G1.HasValue && entity.G1.Value)
				|| (entity.G2.HasValue && entity.G2.Value)
				|| (entity.G3.HasValue && entity.G3.Value)
				|| (entity.GT1.HasValue && entity.GT1.Value)
				|| (entity.GT2.HasValue && entity.GT2.Value)
				|| (entity.GT3.HasValue && entity.GT3.Value)
				) ? "X" : "";
			Termin_error = (entity.Termin.HasValue && entity.Termin.Value < DateTime.Now.AddDays(21)) ? "X" : "";
			Gestartet = entity.FA_Gestartet.HasValue && entity.FA_Gestartet.Value ? "X" : "";
			NotOpen = entity.Kennzeichen;
		}
	}
}