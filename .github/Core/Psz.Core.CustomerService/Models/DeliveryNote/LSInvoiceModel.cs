using System;

namespace Psz.Core.CustomerService.Models.DeliveryNote
{
	public class LSInvoiceResponseModel
	{
		public int Nr { get; set; }
		public string DocumentNumber { get; set; }
		public DateTime? Date { get; set; }
		public int? AngebotNumber { get; set; }
		public string ProjectNumber { get; set; }
		public LSInvoiceResponseModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity)
		{
			if(entity == null)
				return;

			Nr = entity.Nr;
			DocumentNumber = entity.Bezug;
			Date = entity.Datum;
			AngebotNumber = entity.Angebot_Nr;
			ProjectNumber = entity.Projekt_Nr;
		}
	}
}
