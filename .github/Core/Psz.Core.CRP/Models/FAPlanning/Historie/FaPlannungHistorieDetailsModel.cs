using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning.Historie
{
	public class FaPlannungHistorieDetailsModel
	{
		public int FAId { get; set; }
		public int Werk { get; set; }
		public string Customer { get; set; }
		public int FANumber { get; set; }
		public int OpenQty { get; set; }
		public string PNPSZ { get; set; }
		public int ArtikelNr { get; set; }
		public string Freigabestatus { get; set; }
		public decimal OrderTime { get; set; }
		public DateTime? AckDate { get; set; }
		public DateTime? FADruckdatum { get; set; }
		public FaPlannungHistorieDetailsModel()
		{

		}
		public FaPlannungHistorieDetailsModel(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity entity)
		{
			FAId = entity.FAId;
			Werk = entity.Werk ?? 0;
			Customer = entity.Customer;
			FANumber = entity.FA_Number ?? 0;
			OpenQty = entity.Open_Qty ?? 0;
			PNPSZ = entity.PN_PSZ;
			ArtikelNr = entity.ArtikelNr;
			Freigabestatus = entity.Freigabestatus;
			OrderTime = entity.Order_Time ?? 0m;
			AckDate = entity.Ack_Date;
			FADruckdatum = entity.FA_Druckdatum;
		}
	}
	public class FaPlannungHistorieDetailsRequestModel: IPaginatedRequestModel
	{
		public int IdHeader { get; set; }
		public string SearchText { get; set; }
	}
	public class FaPlannungHistorieDetailsResponsetModel: IPaginatedResponseModel<FaPlannungHistorieDetailsModel> { }
}