using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class ReportOneRequestTblModel: IPaginatedRequestModel
	{
		public string? SearchValue { get; set; }
		public int? LagerId { get; set; }
	}
	public class ReportOne_Tabl_Model
	{
		public string Artikelnummer { get; set; }
		public string FaGeschnitten { get; set; }
		public string FaKommisioniert { get; set; }
		public DateTime? FaTermin { get; set; }
		public string Fertigungsnummer { get; set; }
		public decimal OffeneMenge { get; set; }
		public int ArtikelNr { get; set; }
		public int FertigungId { get; set; }

		public ReportOne_Tabl_Model() { }
		public ReportOne_Tabl_Model(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity entity)
		{
			if(entity == null)
				return;

			Fertigungsnummer = entity.Fertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			OffeneMenge = entity.OffeneMenge;
			FaTermin = entity.FaTermin;
			FaGeschnitten = entity.FaGeschnitten;
			FaKommisioniert = entity.FaKommisioniert;
			ArtikelNr=entity.ArtikelNr;
			FertigungId=entity.FertigungId;
		}

	}
	public class ReportOne_tbl_Reponse_Model: IPaginatedResponseModel<ReportOne_Tabl_Model>
	{
	}
	public class ReportRequestModel
	{
		public string? SearchValue { get; set; }
		public int? LagerId { get; set; }
		public int? ReportId { get; set; }
	}
}
