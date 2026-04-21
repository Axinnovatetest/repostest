using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class ReportTwoRequestTblModel: IPaginatedRequestModel
	{
		public string? SearchValue { get; set; }
		public int? LagerId { get; set; }

	}
	public class ReportTwo_Tabl_Model
	{
		public string Artikelnummer { get; set; }
		public decimal GefundeneMengeInProduktion { get; set; }
		public decimal Lagerbestand { get; set; }
		public decimal MengeInProduktion { get; set; }
		public string RueckbuchungBestaetigt { get; set; }
		public int ArtikelNr { get; set; }
		public ReportTwo_Tabl_Model() { }
		public ReportTwo_Tabl_Model(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity entity)
		{
			if(entity == null)
				return;
			Artikelnummer = entity.Artikelnummer;
			MengeInProduktion = entity.MengeInProduktion;
			Lagerbestand = entity.Lagerbestand;
			GefundeneMengeInProduktion = entity.GefundeneMengeInProduktion;
			RueckbuchungBestaetigt = entity.RueckbuchungBestaetigt;
			ArtikelNr=entity.ArtikelNr;
		}

	}
	public class ReportTwo_tbl_Reponse_Model: IPaginatedResponseModel<ReportTwo_Tabl_Model>
	{
	}
}
