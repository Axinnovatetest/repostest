using System;

namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class QuickSearchResponseModel
	{
		public int Nr { get; set; }
		public string DocNumber { get; set; }
		public string Type { get; set; }
		public string Customer { get; set; }
		public string ProjectNr { get; set; }
		public string VorfailNr { get; set; }
		public DateTime? DueDate { get; set; }

		public QuickSearchResponseModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity)
		{
			Nr = entity.Nr;
			DocNumber = entity.Bezug;
			Type = entity.Typ;
			Customer = entity.Vorname_NameFirma;
			ProjectNr = entity.Projekt_Nr;
			VorfailNr = entity.Angebot_Nr.HasValue ? entity.Angebot_Nr.ToString() : "";
			DueDate = entity.Falligkeit;
		}
	}
}
