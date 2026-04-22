using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class ArtikelOrderAllModel
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
		//Article Version
		public string Action_Version_Article { get; set; }
		public string Currency_Version_Article { get; set; }
		public string Dept_name_VersionArticle { get; set; }
		public int Id_AOV { get; set; }
		public int? Id_Currency_Version_Article { get; set; }
		public int? Id_Dept_VersionArticle { get; set; }
		public int? Id_Land_VersionArticle { get; set; }
		public int? Id_Level_VersionArticle { get; set; }
		public int Id_Order_Version { get; set; }
		public int? Id_Project_VersionArticle { get; set; }
		public int? Id_Status_VersionArticle { get; set; }
		public int? Id_Supplier_VersionArticle { get; set; }
		public int Id_User_VersionArticle { get; set; }
		public string Land_name_VersionArticle { get; set; }
		public int? Quantity_VersionArticle { get; set; }
		public double? TotalCost__VersionArticle { get; set; }
		public double? Unit_Price_VersionArticle { get; set; }
		public DateTime? Version_Article_date { get; set; }
		//order vesion	
		public int? Id_Currency_Order { get; set; }
		public int? Id_Level { get; set; }
		public int? Id_Status { get; set; }
		public int? Id_Supplier_VersionOrder { get; set; }
		public int Id_VO { get; set; }
		public int? Nr_version_Order { get; set; }
		public string Step_Order { get; set; }
		public double? TotalCost_Order { get; set; }
		public DateTime? Version_Order_date { get; set; }
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


		public ArtikelOrderAllModel() { }
		public ArtikelOrderAllModel(Infrastructure.Data.Entities.Tables.FNC.Article_OrderAllEntity artikelOrderEntity)
		{
			Action_Article = artikelOrderEntity.Action_Article;
			Id_AO = artikelOrderEntity.Id_AO;
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
			//Article Version
			Action_Version_Article = artikelOrderEntity.Action_Version_Article;
			Currency_Version_Article = artikelOrderEntity.Currency_Version_Article;
			Dept_name_VersionArticle = artikelOrderEntity.Dept_name_VersionArticle;
			Id_AOV = artikelOrderEntity.Id_AOV;
			Id_Currency_Version_Article = artikelOrderEntity.Id_Currency_Version_Article;
			Id_Dept_VersionArticle = artikelOrderEntity.Id_Dept_VersionArticle;
			Id_Land_VersionArticle = artikelOrderEntity.Id_Land_VersionArticle;
			Id_Level_VersionArticle = artikelOrderEntity.Id_Level_VersionArticle;
			Id_Order_Version = artikelOrderEntity.Id_Order_Version;
			Id_Project_VersionArticle = artikelOrderEntity.Id_Project_VersionArticle;
			Id_Status_VersionArticle = artikelOrderEntity.Id_Status_VersionArticle;
			Id_Supplier_VersionArticle = artikelOrderEntity.Id_Supplier_VersionArticle;
			Id_User_VersionArticle = artikelOrderEntity.Id_User_VersionArticle;
			Land_name_VersionArticle = artikelOrderEntity.Land_name_VersionArticle;
			Quantity_VersionArticle = artikelOrderEntity.Quantity_VersionArticle;
			TotalCost__VersionArticle = artikelOrderEntity.TotalCost__VersionArticle;
			Unit_Price_VersionArticle = artikelOrderEntity.Unit_Price_VersionArticle;
			Version_Article_date = artikelOrderEntity.Version_Article_date;
			//order vesion	
			Id_Currency_Order = artikelOrderEntity.Id_Currency_Order;
			Id_Level = artikelOrderEntity.Id_Level;
			Id_Status = artikelOrderEntity.Id_Status;
			Id_Supplier_VersionOrder = artikelOrderEntity.Id_Supplier_VersionOrder;
			Id_VO = artikelOrderEntity.Id_VO;
			Nr_version_Order = artikelOrderEntity.Nr_version_Order;
			Step_Order = artikelOrderEntity.Step_Order;
			TotalCost_Order = artikelOrderEntity.TotalCost_Order;
			Version_Order_date = artikelOrderEntity.Version_Order_date;
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

		public Infrastructure.Data.Entities.Tables.FNC.Article_OrderAllEntity ToBudgetSuppLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Article_OrderAllEntity
			{
				Action_Article = Action_Article,
				Id_AO = Id_AO,
				Id_Order = Id_Order,
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
				//Article Version
				Action_Version_Article = Action_Version_Article,
				Currency_Version_Article = Currency_Version_Article,
				Dept_name_VersionArticle = Dept_name_VersionArticle,
				Id_AOV = Id_AOV,
				Id_Currency_Version_Article = Id_Currency_Version_Article,
				Id_Dept_VersionArticle = Id_Dept_VersionArticle,
				Id_Land_VersionArticle = Id_Land_VersionArticle,
				Id_Level_VersionArticle = Id_Level_VersionArticle,
				Id_Order_Version = Id_Order_Version,
				Id_Project_VersionArticle = Id_Project_VersionArticle,
				Id_Status_VersionArticle = Id_Status_VersionArticle,
				Id_Supplier_VersionArticle = Id_Supplier_VersionArticle,
				Id_User_VersionArticle = Id_User_VersionArticle,
				Land_name_VersionArticle = Land_name_VersionArticle,
				Quantity_VersionArticle = Quantity_VersionArticle,
				TotalCost__VersionArticle = TotalCost__VersionArticle,
				Unit_Price_VersionArticle = Unit_Price_VersionArticle,
				Version_Article_date = Version_Article_date,
				//order vesion	
				Id_Currency_Order = Id_Currency_Order,
				Id_Level = Id_Level,
				Id_Status = Id_Status,
				Id_Supplier_VersionOrder = Id_Supplier_VersionOrder,
				Id_VO = Id_VO,
				Nr_version_Order = Nr_version_Order,
				Step_Order = Step_Order,
				TotalCost_Order = TotalCost_Order,
				Version_Order_date = Version_Order_date,
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
