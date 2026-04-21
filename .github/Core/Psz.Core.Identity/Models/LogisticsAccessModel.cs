using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class LogisticsAccessModel
	{

		public bool ModuleActivated;
		public bool Administration;
		//-------------------------------
		public bool Shipping { get; set; }
		public bool ShippingCreate { get; set; }
		public bool Statistics { get; set; }
		public bool StatiticsDeleteROHScanned { get; set; }
		public bool StatiticsArticleCustomsNumber { get; set; }
		//--------------------------------------------------------
		public bool Materialswirtschaft { get; set; }
		public bool MaterialswirtschaftUmbuchung { get; set; }
		public bool MaterialswirtschaftEntnahme { get; set; }
		public bool MaterialswirtschaftZugang { get; set; }
		public bool MaterialswirtschaftTranfer { get; set; }
		public bool Customs { get; set; }
		public bool IsDefault { get; set; }
		//-----------------------------------------
		public bool Inventur { get; set; }
		public bool InventurReset { get; set; }
		public bool InventurFG { get; set; }
		public bool InventurROH { get; set; }
		//-----------------------------------------
		public bool PlantBookings { get; set; }
		public bool PlantBookingCreateCopy { get; set; }
		public bool PlantBookingDelete { get; set; }
		public bool PlantBookingEdit { get; set; }
		public bool PlantBookingReprint { get; set; }
		public bool PlantBookingView { get; set; }
		public bool ViewLagerList { get; set; }
		public bool ViewLogs { get; set; }
		public bool TransferUmbuchung2 { get; set; }
		public bool Xls { get; set; }
		public bool AddArtikelVOH { get; set; }
		public LogisticsAccessModel()
		{

		}
		public LogisticsAccessModel(List<Infrastructure.Data.Entities.Tables.Logistics.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities?.Count <= 0)
				return;

			ModuleActivated = false;
			Shipping = false;
			ShippingCreate = false;
			Statistics = false;
			StatiticsDeleteROHScanned = false;
			StatiticsArticleCustomsNumber = false;
			Administration = false;
			Materialswirtschaft = false;
			MaterialswirtschaftUmbuchung = false;
			MaterialswirtschaftEntnahme = false;
			MaterialswirtschaftZugang = false;
			MaterialswirtschaftTranfer = false;
			Customs = false;
			IsDefault = false;
			Inventur = false;
			InventurReset = false;
			InventurFG = false;
			InventurROH = false;
			PlantBookings = false;
		    PlantBookingCreateCopy = false;
			PlantBookingDelete = false;
			PlantBookingEdit = false;
			PlantBookingReprint = false;
			PlantBookingView = false;
			ViewLagerList = false;
			ViewLogs = false;
			TransferUmbuchung2 = false;
			Xls = false;
			AddArtikelVOH = false;

			foreach(var accessItem in accessProfileEntities)
			{
				// - 
				Shipping = Shipping || (accessItem.Shipping ?? false);
				ShippingCreate = ShippingCreate || (accessItem.ShippingCreate ?? false);
				Statistics = Statistics || (accessItem.StatisticsLGT ?? false);
				StatiticsDeleteROHScanned = StatiticsDeleteROHScanned || (accessItem.StatiticsDeleteROHScanned ?? false);
				StatiticsArticleCustomsNumber = StatiticsArticleCustomsNumber || (accessItem.StatiticsArticleCustomsNumber ?? false);
				Administration = Administration || (accessItem.Administration ?? false);
				Materialswirtschaft = Materialswirtschaft || (accessItem.Materialswirtschaft ?? false);
				MaterialswirtschaftUmbuchung = MaterialswirtschaftUmbuchung || (accessItem.MaterialswirtschaftUmbuchung ?? false);
				MaterialswirtschaftEntnahme = MaterialswirtschaftEntnahme || (accessItem.MaterialswirtschaftEntnahme ?? false);
				MaterialswirtschaftZugang = MaterialswirtschaftZugang || (accessItem.MaterialswirtschaftZugang ?? false);
				MaterialswirtschaftTranfer = MaterialswirtschaftTranfer || (accessItem.MaterialswirtschaftTranfer ?? false);
				Customs = Customs || (accessItem.Customs ?? false);
				IsDefault = IsDefault || (accessItem.IsDefault ?? false);
				Inventur = Inventur || (accessItem.Inventur ?? false);
				InventurReset = InventurReset || (accessItem.InventurReset ?? false);
				InventurFG = InventurFG || (accessItem.InventurFG ?? false);
				InventurROH = InventurROH || (accessItem.InventurROH ?? false);
				PlantBookings = PlantBookings || (accessItem.PlantBookings ?? false);
				PlantBookingCreateCopy = PlantBookingCreateCopy || (accessItem.PlantBookingCreateCopy ?? false);
				PlantBookingDelete = PlantBookingDelete || (accessItem.PlantBookingDelete ?? false);
				PlantBookingEdit = PlantBookingEdit || (accessItem.PlantBookingEdit ?? false);
				PlantBookingReprint = PlantBookingReprint || (accessItem.PlantBookingReprint ?? false);
				PlantBookingView = PlantBookingView || (accessItem.PlantBookingView ?? false);
				ViewLagerList = ViewLagerList || (accessItem.ViewLagerList ?? false);
				ViewLogs = ViewLogs || (accessItem.ViewLogs ?? false);
				TransferUmbuchung2 = TransferUmbuchung2 || (accessItem.TransferUmbuchung2 ?? false);
				Xls = Xls || (accessItem.Xls ?? false);
				AddArtikelVOH = AddArtikelVOH || (accessItem.AddArtikelVOH ?? false);

				// -
				ModuleActivated = ModuleActivated || (accessItem?.Shipping ?? false)
					|| (accessItem?.ShippingCreate ?? false)
					|| (accessItem?.StatisticsLGT ?? false)
					|| (accessItem?.StatiticsDeleteROHScanned ?? false)
					|| (accessItem?.StatiticsArticleCustomsNumber ?? false)
					|| (accessItem?.Administration ?? false)
				|| (accessItem?.Materialswirtschaft ?? false)
				|| (accessItem?.MaterialswirtschaftUmbuchung ?? false)
				|| (accessItem?.MaterialswirtschaftEntnahme ?? false)
				|| (accessItem?.MaterialswirtschaftZugang ?? false)
				|| (accessItem?.MaterialswirtschaftTranfer ?? false)
				|| (accessItem?.Customs ?? false)
				|| (accessItem?.IsDefault ?? false)
				|| (accessItem?.Inventur ?? false)
				|| (accessItem?.InventurReset ?? false)
				|| (accessItem?.InventurFG ?? false)
				|| (accessItem?.InventurROH ?? false)
				|| (accessItem?.PlantBookings ?? false)
				|| (accessItem?.PlantBookingCreateCopy ?? false)
				|| (accessItem?.PlantBookingDelete ?? false)
				|| (accessItem?.PlantBookingEdit ?? false)
				|| (accessItem?.PlantBookingReprint ?? false)
				|| (accessItem?.PlantBookingView ?? false)
				|| (accessItem?.ViewLagerList ?? false)
				|| (accessItem?.ViewLogs ?? false)
				|| (accessItem?.TransferUmbuchung2 ?? false)
				|| (accessItem?.Xls ?? false)
				|| (accessItem?.AddArtikelVOH ?? false);
			}


		}
	}
	public class LogisticsAccessMinimalModel
	{


		public bool ModuleActivated;
		public bool Shipping { get; set; }
		public bool Statistics { get; set; }
		public bool Administration { get; set; }
		public bool Materialswirtschaft { get; set; }
		public bool Inventur { get; set; }
		public LogisticsAccessMinimalModel()
		{

		}
		public LogisticsAccessMinimalModel(LogisticsAccessModel model)
		{
			Shipping = model.Shipping;
			Statistics = model.Statistics;
			Administration = model.Administration;
			Materialswirtschaft = model.Materialswirtschaft;
			Inventur = model.Inventur;
			ModuleActivated = model.ModuleActivated || model.Shipping || model.Statistics || model.Materialswirtschaft || model.Inventur || model.Administration;
		}
	}
}
