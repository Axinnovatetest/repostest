using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class InsertedDataOrderModel
	{

		public int Id_Order { get; set; }
		public string Order_Number { get; set; }
		public string order_index => Id_Order + "_" + Type_Order;
		public string Type_Order { get; set; }
		public int? Id_Project { get; set; }
		public int? Id_Dept { get; set; }
		public string Dept_name { get; set; }
		public int? Id_Land { get; set; }
		public string Land_name { get; set; }
		public int? Id_Currency_Order { get; set; }
		public int? Id_Supplier { get; set; }
		public int Id_User { get; set; }
		public DateTime? Order_date { get; set; }
		public int Id_VO { get; set; }
		public int? Nr_version_Order { get; set; }
		public int? Id_Level { get; set; }
		public int? Id_Status { get; set; }
		public string Step_Order { get; set; }
		public int? Id_Supplier_VersionOrder { get; set; }
		public double? TotalCost_Order { get; set; }
		public DateTime? Version_Order_date { get; set; }


		public int Id_Diverse_Supplier_Order { get; set; }
		public int? Id_Order_Diverse { get; set; }
		public int? Id_Supplier_Order_Diverse { get; set; }
		public int? Lieferantennummer_Order_Diverse { get; set; }
		public string Ort_Order_Supplier_Diverse { get; set; }
		public string Supplier_Contact_Description_Order_Diverse { get; set; }
		public string Supplier_Contact_Order_Diverse { get; set; }
		public string Supplier_Name_Order_Diverse { get; set; }




		public InsertedDataOrderModel() { }

		public InsertedDataOrderModel(Infrastructure.Data.Entities.Tables.FNC.Budget_OrderInsertedEntity allDataOrderEntity)


		{
			Id_Order = allDataOrderEntity.Id_Order;
			//Order_Number = allDataOrderEntity.Order_Number;
			Order_Number = order_index;
			Type_Order = allDataOrderEntity.Type_Order;
			Id_Project = allDataOrderEntity.Id_Project;
			Id_Dept = allDataOrderEntity.Id_Dept;
			Dept_name = allDataOrderEntity.Dept_name;
			Id_Land = allDataOrderEntity.Id_Land;
			Land_name = allDataOrderEntity.Land_name;
			Id_Currency_Order = allDataOrderEntity.Id_Currency_Order;
			Id_Supplier = allDataOrderEntity.Id_Supplier;
			Id_User = allDataOrderEntity.Id_User;
			Order_date = allDataOrderEntity.Order_date;
			Id_VO = allDataOrderEntity.Id_VO;
			Nr_version_Order = allDataOrderEntity.Nr_version_Order;
			Id_Level = allDataOrderEntity.Id_Level;
			Id_Status = allDataOrderEntity.Id_Status;
			Step_Order = allDataOrderEntity.Step_Order;
			Id_Supplier_VersionOrder = allDataOrderEntity.Id_Supplier_VersionOrder;
			TotalCost_Order = allDataOrderEntity.TotalCost_Order;
			Version_Order_date = allDataOrderEntity.Version_Order_date;
			Id_Diverse_Supplier_Order = allDataOrderEntity.Id_Diverse_Supplier_Order;
			Id_Order_Diverse = allDataOrderEntity.Id_Order_Diverse;
			Id_Supplier_Order_Diverse = allDataOrderEntity.Id_Supplier_Order_Diverse;
			Lieferantennummer_Order_Diverse = allDataOrderEntity.Lieferantennummer_Order_Diverse;
			Ort_Order_Supplier_Diverse = allDataOrderEntity.Ort_Order_Supplier_Diverse;
			Supplier_Contact_Description_Order_Diverse = allDataOrderEntity.Supplier_Contact_Description_Order_Diverse;
			Supplier_Contact_Order_Diverse = allDataOrderEntity.Supplier_Contact_Order_Diverse;
			Supplier_Name_Order_Diverse = allDataOrderEntity.Supplier_Name_Order_Diverse;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_OrderInsertedEntity ToBudgetallDtaOrders()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderInsertedEntity
			{
				Id_Order = Id_Order,
				//Order_Number = Order_Number,
				Order_Number = order_index,
				Type_Order = Type_Order,
				Id_Project = Id_Project,
				Id_Dept = Id_Dept,
				Dept_name = Dept_name,
				Id_Land = Id_Land,
				Land_name = Land_name,
				Id_Currency_Order = Id_Currency_Order,
				Id_Supplier = Id_Supplier,
				Id_User = Id_User,
				Order_date = Order_date,
				Id_VO = Id_VO,
				Nr_version_Order = Nr_version_Order,
				Id_Level = Id_Level,
				Id_Status = Id_Status,
				Step_Order = Step_Order,
				Id_Supplier_VersionOrder = Id_Supplier_VersionOrder,
				TotalCost_Order = TotalCost_Order,
				Version_Order_date = Version_Order_date,
				Id_Diverse_Supplier_Order = Id_Diverse_Supplier_Order,
				Id_Order_Diverse = Id_Order_Diverse,
				Id_Supplier_Order_Diverse = Id_Supplier_Order_Diverse,
				Lieferantennummer_Order_Diverse = Lieferantennummer_Order_Diverse,
				Ort_Order_Supplier_Diverse = Ort_Order_Supplier_Diverse,
				Supplier_Contact_Description_Order_Diverse = Supplier_Contact_Description_Order_Diverse,
				Supplier_Contact_Order_Diverse = Supplier_Contact_Order_Diverse,
				Supplier_Name_Order_Diverse = Supplier_Name_Order_Diverse,

			};
		}
	}
}
