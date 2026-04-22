using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class ArtikelOrderConcatNameModel
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
		public string Article_Name { get; set; }
		public string Article_Name_Order_Diverse { get; set; }
		public double? Unit_Price_Diverse { get; set; }


		public ArtikelOrderConcatNameModel() { }
		public ArtikelOrderConcatNameModel(Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity artikelOrderEntity)
		{
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
			Action_Article = artikelOrderEntity.Action_Article;
			Article_date = artikelOrderEntity.Article_date;
			Id_User = artikelOrderEntity.Id_User;
			Article_Name = artikelOrderEntity.Article_Name;
			Article_Name_Order_Diverse = artikelOrderEntity.Article_Name_Order_Diverse;
			Unit_Price_Diverse = artikelOrderEntity.Unit_Price_Diverse;
		}

		public Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity ToBudgetSuppLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity
			{
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
				Action_Article = Action_Article,
				Article_date = Article_date,
				Id_User = Id_User,
				Article_Name = Article_Name,
				Article_Name_Order_Diverse = Article_Name_Order_Diverse,
				Unit_Price_Diverse = Unit_Price_Diverse,
			};
		}
	}
}
