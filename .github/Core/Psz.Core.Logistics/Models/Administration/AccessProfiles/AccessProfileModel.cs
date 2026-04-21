using System;

namespace Psz.Core.Logistics.Models.Administration.AccessProfiles
{
	public class AccessProfileModel
	{
		public string AccessProfileName { get; set; }
		public int Id { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? Administration { get; set; }
		//-------------------------------
		//-------------------------------
		public bool? Shipping { get; set; }
		public bool? ShippingCreate { get; set; }
		public bool? Statistics { get; set; }
		public bool? StatiticsDeleteROHScanned { get; set; }
		public bool? StatiticsArticleCustomsNumber { get; set; }
		//--------------------------------------------------------
		public bool? Materialswirtschaft { get; set; }
		public bool? MaterialswirtschaftUmbuchung { get; set; }
		public bool? MaterialswirtschaftEntnahme { get; set; }
		public bool? MaterialswirtschaftZugang { get; set; }
		public bool? MaterialswirtschaftTranfer { get; set; }
		public bool? Customs { get; set; }
		//------------------------------------
		public bool? IsDefault { get; set; }
		//----------------------------------
		public bool? Inventur { get; set; }
		public bool? InventurReset { get; set; }
		public bool? InventurFG { get; set; }
		public bool? InventurROH { get; set; }
		//----------------------------------
		public bool? PlantBookings { get; set; }
		public bool? PlantBookingCreateCopy { get; set; }
		public bool? PlantBookingDelete { get; set; }
		public bool? PlantBookingEdit { get; set; }
		public bool? PlantBookingReprint { get; set; }
		public bool? PlantBookingView { get; set; }
		public bool? ViewLagerList { get; set; }
		public bool? ViewLogs { get; set; }
		public bool? TransferUmbuchung2 { get; set; }
		public bool? Xls { get; set; }
		public bool? AddArtikelVOH { get; set; }


        public AccessProfileModel(Infrastructure.Data.Entities.Tables.Logistics.AccessProfileEntity accessProfileEntity)
		{
			if(accessProfileEntity == null)
				return;
			// -
			AccessProfileName = accessProfileEntity.AccessProfileName;
			Id = accessProfileEntity.Id;
			CreationTime = accessProfileEntity.CreationTime;
			CreationUserId = accessProfileEntity.CreationUserId;
			ModuleActivated = accessProfileEntity.ModuleActivated;
			Administration = accessProfileEntity.Administration;
			Shipping = accessProfileEntity.Shipping;
			ShippingCreate = accessProfileEntity.ShippingCreate;
			Statistics = accessProfileEntity.StatisticsLGT;
			StatiticsDeleteROHScanned = accessProfileEntity.StatiticsDeleteROHScanned;
			StatiticsArticleCustomsNumber = accessProfileEntity.StatiticsArticleCustomsNumber;
			Materialswirtschaft = accessProfileEntity.Materialswirtschaft;
			MaterialswirtschaftUmbuchung = accessProfileEntity.MaterialswirtschaftUmbuchung;
			MaterialswirtschaftEntnahme = accessProfileEntity.MaterialswirtschaftEntnahme;
			MaterialswirtschaftZugang = accessProfileEntity.MaterialswirtschaftZugang;
			MaterialswirtschaftTranfer = accessProfileEntity.MaterialswirtschaftTranfer;
			Customs = accessProfileEntity.Customs;
			IsDefault = accessProfileEntity.IsDefault;
			Inventur = accessProfileEntity.Inventur;
			InventurReset = accessProfileEntity.InventurReset;
			InventurFG = accessProfileEntity.InventurFG;
			InventurROH = accessProfileEntity.InventurROH;
			PlantBookings = accessProfileEntity.PlantBookings;
			PlantBookingCreateCopy = accessProfileEntity.PlantBookingCreateCopy;
			PlantBookingDelete = accessProfileEntity.PlantBookingDelete;
			PlantBookingEdit = accessProfileEntity.PlantBookingEdit;
			PlantBookingReprint = accessProfileEntity.PlantBookingReprint;
			PlantBookingView = accessProfileEntity.PlantBookingView;
			ViewLagerList = accessProfileEntity.ViewLagerList;
			ViewLogs = accessProfileEntity.ViewLogs;
			TransferUmbuchung2 = accessProfileEntity.TransferUmbuchung2;
			Xls = accessProfileEntity.Xls;
			AddArtikelVOH = accessProfileEntity.AddArtikelVOH;
		}
		public Infrastructure.Data.Entities.Tables.Logistics.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.Logistics.AccessProfileEntity
			{
				Id = Id,
				ModuleActivated = ModuleActivated,
				AccessProfileName = AccessProfileName,
				CreationUserId = CreationUserId,
				CreationTime = CreationTime,
				Administration = Administration,
				//-------------------------------------
				Shipping = Shipping,
				ShippingCreate = ShippingCreate,
				StatisticsLGT = Statistics,
				StatiticsDeleteROHScanned = StatiticsDeleteROHScanned,
				StatiticsArticleCustomsNumber = StatiticsArticleCustomsNumber,
				//------------------------------------------------
				Materialswirtschaft = Materialswirtschaft,
				MaterialswirtschaftUmbuchung = MaterialswirtschaftUmbuchung,
				MaterialswirtschaftEntnahme = MaterialswirtschaftEntnahme,
				MaterialswirtschaftZugang = MaterialswirtschaftZugang,
				MaterialswirtschaftTranfer = MaterialswirtschaftTranfer,
				Customs = Customs,
				//-------------------------------
				IsDefault = IsDefault,
				//----------------------------
				Inventur = Inventur,
				InventurReset = InventurReset,
				InventurFG = InventurFG,
				InventurROH = InventurROH,
				//----------------------------
				PlantBookings= PlantBookings,
				PlantBookingCreateCopy = PlantBookingCreateCopy,
				PlantBookingDelete = PlantBookingDelete,
				PlantBookingEdit = PlantBookingEdit,
				PlantBookingReprint = PlantBookingReprint,
				PlantBookingView = PlantBookingView,
				ViewLagerList = ViewLagerList,
				ViewLogs = ViewLogs,
				TransferUmbuchung2 = TransferUmbuchung2,
				Xls = Xls,
				AddArtikelVOH = AddArtikelVOH,
			};
		}
	}
}
