using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateListArtikelsVersionOrderHandler
	{
		private Models.Budget.InsertedListArticleOrderModel _data { get; set; }
		//private Models.Budget.ArtikelOrderParamsModel _data2 { get; set; }
		private int Max_Ver_Ord { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		private int InsertedArtikelOrder;
		//private int Inserted;

		public CreateListArtikelsVersionOrderHandler(Models.Budget.InsertedListArticleOrderModel data, int Max_Ver_Ord, Identity.Models.UserModel user)
		{
			this._data = data;
			this.Max_Ver_Ord = Max_Ver_Ord;
			//this._data2 = data2;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var _data2 = new List<Models.Budget.ArtikelOrderParamsModel>();
				//var LastVersionOrder = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetLastVersionOrder(this.Max_Ver_Ord);
				//foreach (var order_tableEntity in LastVersionOrder)
				//{
				//    _data2.Add(new Models.Budget.ArtikelOrderParamsModel(order_tableEntity));
				//}


				/* var ArtikelOrderEntity = new List<Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity>();
                 foreach (var order_article in ArtikelOrderEntity)
                 {
                     { //Last Version Order

                         order_article.Max_VO = _data2[0].Max_VO;
                         order_article.Nr_version_Order_param = _data2[0].Nr_version_Order_param;
                         order_article.Id_Level_param = _data2[0].Id_Level_param;
                         order_article.Id_Status_param = _data2[0].Id_Status_param;
                         order_article.Id_Dept_param = _data2[0].Id_Dept_param;
                         order_article.Id_Land_param = _data2[0].Id_Land_param;
                         order_article.Dept_name_param = _data2[0].Dept_name_param;
                         order_article.Land_name_param = _data2[0].Land_name_param;
                         order_article.Id_Currency_Order_param = _data2[0].Id_Currency_Order_param;
                         order_article.Id_Supplier_VersionOrder_param = _data2[0].Id_Supplier_VersionOrder_param;
                         order_article.TotalCost_Order_param = _data2[0].TotalCost_Order_param;
                         order_article.Step_Order_param = _data2[0].Step_Order_param;
                         order_article.Id_Project_param = _data2[0].Id_Project_param;
                         foreach (var Artikel in _data.ArtikelsOrder) { 
                             //Artikel
                             order_article.Id_Order = Artikel.Id_Order;
                         order_article.Id_Article = Artikel.Id_Article;
                         order_article.Id_Currency_Article = Artikel.Id_Currency_Article;
                         order_article.Currency_Article = Artikel.Currency_Article;
                         order_article.Id_Dept = _data2[0].Id_Dept_param;
                         order_article.Dept_name = _data2[0].Dept_name_param;
                         order_article.Id_Land = _data2[0].Id_Land_param;
                         order_article.Land_name = _data2[0].Land_name_param;
                         order_article.Id_Project = _data2[0].Id_Project_param;
                         order_article.Quantity = Artikel.Quantity;
                         order_article.Unit_Price = Artikel.Unit_Price;
                         order_article.TotalCost_Article = Artikel.TotalCost_Article;
                         order_article.Action_Article = Artikel.Action_Article;
                         order_article.Article_date = Artikel.Article_date;
                         order_article.Id_User = Artikel.Id_User;
                         order_article.TotalCost_Order = Artikel.TotalCost_Order;
                         }

                     }
                   Inserted = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.InsertArtikelVersionOrder(order_article, this.Max_Ver_Ord);
                 }*/

				var ArtikelOrderEntity = new Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity();
				/*{ //Last Version Order

                    Max_VO = _data2[0].Max_VO,
                    Nr_version_Order_param = _data2[0].Nr_version_Order_param,
                    Id_Level_param = _data2[0].Id_Level_param,
                    Id_Status_param = _data2[0].Id_Status_param,
                    Id_Dept_param = _data2[0].Id_Dept_param,
                    Id_Land_param = _data2[0].Id_Land_param,
                    Dept_name_param = _data2[0].Dept_name_param,
                    Land_name_param = _data2[0].Land_name_param,
                    Id_Currency_Order_param = _data2[0].Id_Currency_Order_param,
                    Id_Supplier_VersionOrder_param = _data2[0].Id_Supplier_VersionOrder_param,
                    TotalCost_Order_param = _data2[0].TotalCost_Order_param,
                    Step_Order_param = _data2[0].Step_Order_param,
                    Id_Project_param = _data2[0].Id_Project_param,
                };*/
				//Artikel
				foreach(var Artikel in _data.ArtikelsOrder)
				{
					ArtikelOrderEntity.Max_VO = _data2[0].Max_VO;
					ArtikelOrderEntity.Nr_version_Order_param = _data2[0].Nr_version_Order_param;
					ArtikelOrderEntity.Id_Level_param = _data2[0].Id_Level_param;
					ArtikelOrderEntity.Id_Status_param = _data2[0].Id_Status_param;
					ArtikelOrderEntity.Id_Dept_param = _data2[0].Id_Dept_param;
					ArtikelOrderEntity.Id_Land_param = _data2[0].Id_Land_param;
					ArtikelOrderEntity.Dept_name_param = _data2[0].Dept_name_param;
					ArtikelOrderEntity.Land_name_param = _data2[0].Land_name_param;
					ArtikelOrderEntity.Id_Currency_Order_param = _data2[0].Id_Currency_Order_param;
					ArtikelOrderEntity.Id_Supplier_VersionOrder_param = _data2[0].Id_Supplier_VersionOrder_param;
					ArtikelOrderEntity.TotalCost_Order_param = _data2[0].TotalCost_Order_param;
					ArtikelOrderEntity.Step_Order_param = _data2[0].Step_Order_param;
					ArtikelOrderEntity.Id_Project_param = _data2[0].Id_Project_param;
					ArtikelOrderEntity.Id_Order = Artikel.Id_Order;
					ArtikelOrderEntity.Id_Article = Artikel.Id_Article;
					ArtikelOrderEntity.Id_Currency_Article = Artikel.Id_Currency_Article;
					ArtikelOrderEntity.Currency_Article = Artikel.Currency_Article;
					ArtikelOrderEntity.Id_Dept = _data2[0].Id_Dept_param;
					ArtikelOrderEntity.Dept_name = _data2[0].Dept_name_param;
					ArtikelOrderEntity.Id_Land = _data2[0].Id_Land_param;
					ArtikelOrderEntity.Land_name = _data2[0].Land_name_param;
					ArtikelOrderEntity.Id_Project = _data2[0].Id_Project_param;
					ArtikelOrderEntity.Quantity = Artikel.Quantity;
					ArtikelOrderEntity.Unit_Price = Artikel.Unit_Price;
					ArtikelOrderEntity.TotalCost_Article = Artikel.TotalCost_Article;
					ArtikelOrderEntity.Action_Article = Artikel.Action_Article;
					ArtikelOrderEntity.Article_date = Artikel.Article_date;
					ArtikelOrderEntity.Id_User = Artikel.Id_User;
					ArtikelOrderEntity.TotalCost_Order = Artikel.TotalCost_Order;

				}



				if(_data.ArtikelsOrder != null && _data.ArtikelsOrder.Count > 0)
				{
					foreach(var ArtikelOrder in _data.ArtikelsOrder)
					{

						//InsertedArtikelOrder = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.InsertArtikelVersionOrder(ArtikelOrder.ToBudgetListArtikelsOrder(ArtikelOrderEntity.Id_Order), this.Max_Ver_Ord);

					}

				}
				return ResponseModel<int>.SuccessResponse(/*InsertedArtikelOrder*/0);

			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			/*if (this._user == null || !this._user.Access.Budget.Commande)
            {
                return ResponseModel<int>.AccessDeniedResponse();
            }*/
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
