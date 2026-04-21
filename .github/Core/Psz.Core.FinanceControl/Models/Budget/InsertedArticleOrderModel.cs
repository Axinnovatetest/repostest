using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class InsertedArticleOrderModel
	{

		public string Action_Article { get; set; }
		public DateTime? Article_date { get; set; }
		public string Currency_Article { get; set; }
		public string Dept_name { get; set; }
		public int Id_AO { get; set; }
		public int Id_Article { get; set; }
		public int? Id_Currency_Article { get; set; }
		public int? Id_Dept { get; set; }
		public int? Id_Land { get; set; }
		public int Id_Order { get; set; }
		public int? Id_Project { get; set; }
		public int Id_User { get; set; }
		public string Land_name { get; set; }
		public int? Quantity { get; set; }
		public double? TotalCost_Article { get; set; }
		public double? Unit_Price { get; set; }

		//order vesion	
		public double? TotalCost_Order { get; set; }

		//param Version Order
		public int? Max_VO { get; set; }
		public int? Nr_version_Order_param { get; set; }
		public int? Id_Level_param { get; set; }
		public int? Id_Status_param { get; set; }
		public int? Id_Dept_param { get; set; }
		public int? Id_Land_param { get; set; }
		public string Dept_name_param { get; set; }
		public string Land_name_param { get; set; }
		public int? Id_Currency_Order_param { get; set; }
		public int? Id_Supplier_VersionOrder_param { get; set; }
		public double? TotalCost_Order_param { get; set; }
		public string Step_Order_param { get; set; }
		public int? Id_Project_param { get; set; }
		public int? Number { get; set; }



		public InsertedArticleOrderModel() { }
		public InsertedArticleOrderModel(Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity artikelOrderEntity)
		{
			Action_Article = artikelOrderEntity.Action_Article;
			Id_AO = artikelOrderEntity.Id_AO;
			//this.Id_AO = artikelOrderEntity.Id_AO  -1;
			Number = artikelOrderEntity.Id_Order - 1;
			Id_Order = artikelOrderEntity.Id_Order;
			Id_Article = artikelOrderEntity.Id_Article;
			Id_Currency_Article = artikelOrderEntity.Id_Currency_Article;
			Currency_Article = artikelOrderEntity.Currency_Article;
			Id_Dept = artikelOrderEntity.Id_Dept;
			Dept_name = artikelOrderEntity.Dept_name;
			Id_Land = artikelOrderEntity.Id_Land;
			Land_name = artikelOrderEntity.Land_name;
			Id_Project = artikelOrderEntity.Id_Project;
			Quantity = artikelOrderEntity.Quantity;
			Unit_Price = artikelOrderEntity.Unit_Price;
			TotalCost_Article = artikelOrderEntity.TotalCost_Article;
			Article_date = artikelOrderEntity.Article_date;
			Id_User = artikelOrderEntity.Id_User;

			//order vesion	

			TotalCost_Order = artikelOrderEntity.TotalCost_Order;
			//param Version Order
			Max_VO = artikelOrderEntity.Max_VO;
			Nr_version_Order_param = artikelOrderEntity.Nr_version_Order_param;
			Id_Level_param = artikelOrderEntity.Id_Level_param;
			Id_Status_param = artikelOrderEntity.Id_Status_param;
			Id_Dept_param = artikelOrderEntity.Id_Dept_param;
			Id_Land_param = artikelOrderEntity.Id_Land_param;
			Dept_name_param = artikelOrderEntity.Dept_name_param;
			Land_name_param = artikelOrderEntity.Land_name_param;
			Id_Currency_Order_param = artikelOrderEntity.Id_Currency_Order_param;
			Id_Supplier_VersionOrder_param = artikelOrderEntity.Id_Supplier_VersionOrder_param;
			TotalCost_Order_param = artikelOrderEntity.TotalCost_Order_param;
			Step_Order_param = artikelOrderEntity.Step_Order_param;
			Id_Project_param = artikelOrderEntity.Id_Project_param;

		}

		public Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity ToBudgetListArtikelsOrder(int? Number)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity
			{
				Action_Article = Action_Article,
				Id_AO = Id_AO,
				//Id_Order = Id_Order,
				Id_Order = Number ?? this.Id_AO,
				Id_Article = Id_Article,
				Id_Currency_Article = Id_Currency_Article,
				Currency_Article = Currency_Article,
				Id_Dept = Id_Dept,
				Dept_name = Dept_name,
				Id_Land = Id_Land,
				Land_name = Land_name,
				Id_Project = Id_Project,
				Quantity = Quantity,
				Unit_Price = Unit_Price,
				TotalCost_Article = TotalCost_Article,
				Article_date = Article_date,
				Id_User = Id_User,

				//order vesion	

				//TotalCost_Order = TotalCost_Order,

				//param Version Order
				Max_VO = Max_VO,
				Nr_version_Order_param = Nr_version_Order_param,
				Id_Level_param = Id_Level_param,
				Id_Status_param = Id_Status_param,
				Id_Dept_param = Id_Dept_param,
				Id_Land_param = Id_Land_param,
				Dept_name_param = Dept_name_param,
				Land_name_param = Land_name_param,
				Id_Currency_Order_param = Id_Currency_Order_param,
				Id_Supplier_VersionOrder_param = Id_Supplier_VersionOrder_param,
				TotalCost_Order_param = TotalCost_Order_param,
				Step_Order_param = Step_Order_param,
				Id_Project_param = Id_Project_param,
			};
		}
	}
}
