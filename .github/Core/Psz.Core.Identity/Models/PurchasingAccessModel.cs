using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class PurchasingAccessModel
	{
		public bool ModuleActivated { get; set; } = false;

		public bool Order { get; set; }
		public bool Dashboard { get; set; }
		public bool OrderAdd { get; set; }
		public bool OrderEdit { get; set; }
		public bool OrderValidate { get; set; }
		public bool OrderDelete { get; set; }
		public bool OrderUnValidate { get; set; }
		public bool OrderQuickPO { get; set; }
		public bool STAT_Dashboard { get; set; }
		public bool DISPO_Dashboard { get; set; }
		//souilmi 24-07-2023
		public bool Rahmen { get; set; }
		public bool RahmenClosure { get; set; }
		public bool RahmenCancelation { get; set; }
		public bool RahmenValdation { get; set; }
		public bool RahmenDeletePositions { get; set; }
		public bool RahmenAddPositions { get; set; }
		public bool RahmenEditPositions { get; set; }
		public bool RahmenEditHeader { get; set; }
		public bool RahmenDocumentFlow { get; set; }
		public bool RahmenHistory { get; set; }
		public bool RahmenDelete { get; set; }
		public bool RahmenAdd { get; set; }

		public bool WE_Create { get; set; }
		public bool WE { get; set; }
		// - 2024-04-25
		public bool ProjectPurchaseSetOrder { get; set; }
		public bool ProjectPurchaseDeleteOrder { get; set; }
		public PurchasingAccessModel()
		{

		}
		public PurchasingAccessModel(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;

			Order = false;
			OrderEdit = false;
			OrderValidate = false;
			OrderDelete = false;
			OrderUnValidate = false;
			Dashboard = false;
			OrderAdd = false;
			OrderQuickPO = false;
			STAT_Dashboard = false;
			DISPO_Dashboard = false;
			WE_Create = false;
			WE = false;
			//souilmi 24-07-2023
			Rahmen = false;
			RahmenClosure = false;
			RahmenCancelation = false;
			RahmenValdation = false;
			RahmenDeletePositions = false;
			RahmenAddPositions = false;
			RahmenEditPositions = false;
			RahmenEditHeader = false;
			RahmenDocumentFlow = false;
			RahmenHistory = false;
			RahmenDelete = false;
			RahmenAdd = false;
			ProjectPurchaseDeleteOrder = false;
			ProjectPurchaseSetOrder = false;
			foreach(var accessItem in accessProfileEntities)
			{
				Order = Order || (accessItem?.ORD_Order ?? false);
				OrderEdit = OrderEdit || (accessItem?.ORD_OrderEdit ?? false);
				OrderValidate = OrderValidate || (accessItem?.ORD_OrderValidate ?? false);
				OrderUnValidate = OrderUnValidate || (accessItem?.ORD_OrderUnValidate ?? false);
				OrderAdd = OrderAdd || (accessItem?.ORD_OrderAdd ?? false);
				Dashboard = Dashboard || (accessItem?.ORD_Dashboard ?? false);
				OrderDelete = OrderDelete || (accessItem?.ORD_OrderDelete ?? false);
				OrderQuickPO = OrderQuickPO || (accessItem?.ORD_OrderQuickPO ?? false);
				STAT_Dashboard = STAT_Dashboard || (accessItem?.STAT_Dashboard ?? false);
				DISPO_Dashboard = DISPO_Dashboard || (accessItem?.DISPO_Dashboard ?? false);
				WE = WE || (accessItem?.WE ?? false);
				WE_Create = WE_Create || (accessItem?.WE_Create ?? false);
				Rahmen = Rahmen || (accessItem?.Rahmen ?? false);
				RahmenClosure = RahmenClosure || (accessItem?.RahmenClosure ?? false);
				RahmenCancelation = RahmenCancelation || (accessItem?.RahmenCancelation ?? false);
				RahmenValdation = RahmenValdation || (accessItem?.RahmenValdation ?? false);
				RahmenDeletePositions = RahmenDeletePositions || (accessItem?.RahmenDeletePositions ?? false);
				RahmenAddPositions = RahmenAddPositions || (accessItem?.RahmenAddPositions ?? false);
				RahmenEditPositions = RahmenEditPositions || (accessItem?.RahmenEditPositions ?? false);
				RahmenEditHeader = RahmenEditHeader || (accessItem?.RahmenEditHeader ?? false);
				RahmenDocumentFlow = RahmenDocumentFlow || (accessItem?.RahmenDocumentFlow ?? false);
				RahmenHistory = RahmenHistory || (accessItem?.RahmenHistory ?? false);
				RahmenDelete = RahmenDelete || (accessItem?.RahmenDelete ?? false);
				RahmenAdd = RahmenAdd || (accessItem?.RahmenAdd ?? false);
				ProjectPurchaseDeleteOrder = ProjectPurchaseDeleteOrder || (accessItem?.ORD_ProjectPurchaseDeleteOrder ?? false);
				ProjectPurchaseSetOrder = ProjectPurchaseSetOrder || (accessItem?.ORD_ProjectPurchaseSetOrder ?? false);
			}

			ModuleActivated = Order || Dashboard || OrderAdd || OrderDelete || OrderEdit || OrderValidate || OrderUnValidate || STAT_Dashboard || OrderQuickPO || DISPO_Dashboard
		   || Rahmen || RahmenClosure || RahmenCancelation || RahmenValdation || RahmenDeletePositions || RahmenAddPositions || RahmenEditPositions || RahmenEditHeader ||
		   RahmenDocumentFlow || RahmenHistory || RahmenDelete || RahmenAdd || WE || WE_Create || ProjectPurchaseDeleteOrder || ProjectPurchaseSetOrder;
		}

		internal Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity ToDbEntity(int id)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity()
				{
					Id = id,
					ModuleActivated = false,
					ORD_Order = false,
					ORD_OrderEdit = false,
					ORD_OrderValidate = false,
					ORD_OrderUnValidate = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity()
			{
				Id = id,
				ModuleActivated = true,
				ORD_Order = this.Order,
				ORD_OrderEdit = this.OrderEdit,
				ORD_OrderValidate = this.OrderValidate,
				ORD_OrderUnValidate = this.OrderUnValidate,
			};
		}
	}
	public class PurchasingAccessMinimalModel
	{
		public bool ModuleActivated { get; set; }

		public bool Order { get; set; }

		public PurchasingAccessMinimalModel()
		{

		}
		public PurchasingAccessMinimalModel(PurchasingAccessModel model)
		{
			ModuleActivated = model.ModuleActivated;


			Order = model.Order;

		}

		public Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity ToDbEntity(int mainId, int id)
		{
			return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity
			{
				Id = id,
				ModuleActivated = this.ModuleActivated,
				ORD_Order = this.Order,
			};
		}


	}
}
