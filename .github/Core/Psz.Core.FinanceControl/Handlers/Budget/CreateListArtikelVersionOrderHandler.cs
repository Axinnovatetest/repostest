using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateListArtikelVersionOrderHandler
	{
		private List<Models.Budget.InsertedArticleOrderModel> _data { get; set; }
		//private Models.Budget.ArtikelOrderParamsModel _data2 { get; set; }
		private int Max_Ver_Ord { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateListArtikelVersionOrderHandler(List<Models.Budget.InsertedArticleOrderModel> data, int Max_Ver_Ord, Identity.Models.UserModel user)
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
				//var LastVersionOrder = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.GetLastVersionOrder(this.Max_Ver_Ord);
				//foreach (var order_tableEntity in LastVersionOrder)
				//{
				//    _data2.Add(new Models.Budget.ArtikelOrderParamsModel(order_tableEntity));
				//}


				var ArtikelOrderEntity = new List<Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity>();
				foreach(var ArtikelOrder_tableEntity in ArtikelOrderEntity)
				{ //Last Version Order

					ArtikelOrder_tableEntity.Max_VO = _data2[0].Max_VO;
					ArtikelOrder_tableEntity.Nr_version_Order_param = _data2[0].Nr_version_Order_param;
					ArtikelOrder_tableEntity.Id_Level_param = _data2[0].Id_Level_param;
					ArtikelOrder_tableEntity.Id_Status_param = _data2[0].Id_Status_param;
					ArtikelOrder_tableEntity.Id_Dept_param = _data2[0].Id_Dept_param;
					ArtikelOrder_tableEntity.Id_Land_param = _data2[0].Id_Land_param;
					ArtikelOrder_tableEntity.Dept_name_param = _data2[0].Dept_name_param;
					ArtikelOrder_tableEntity.Land_name_param = _data2[0].Land_name_param;
					ArtikelOrder_tableEntity.Id_Currency_Order_param = _data2[0].Id_Currency_Order_param;
					ArtikelOrder_tableEntity.Id_Supplier_VersionOrder_param = _data2[0].Id_Supplier_VersionOrder_param;
					ArtikelOrder_tableEntity.TotalCost_Order_param = _data2[0].TotalCost_Order_param;
					ArtikelOrder_tableEntity.Step_Order_param = _data2[0].Step_Order_param;
					ArtikelOrder_tableEntity.Id_Project_param = _data2[0].Id_Project_param;
					//Artikel
					ArtikelOrder_tableEntity.Id_Order = _data[0].Id_Order;
					ArtikelOrder_tableEntity.Id_Article = _data[0].Id_Article;
					ArtikelOrder_tableEntity.Id_Currency_Article = _data[0].Id_Currency_Article;
					ArtikelOrder_tableEntity.Currency_Article = _data[0].Currency_Article;
					ArtikelOrder_tableEntity.Id_Dept = _data2[0].Id_Dept_param;
					ArtikelOrder_tableEntity.Dept_name = _data2[0].Dept_name_param;
					ArtikelOrder_tableEntity.Id_Land = _data2[0].Id_Land_param;
					ArtikelOrder_tableEntity.Land_name = _data2[0].Land_name_param;
					ArtikelOrder_tableEntity.Id_Project = _data2[0].Id_Project_param;
					ArtikelOrder_tableEntity.Quantity = _data[0].Quantity;
					ArtikelOrder_tableEntity.Unit_Price = _data[0].Unit_Price;
					ArtikelOrder_tableEntity.TotalCost_Article = _data[0].TotalCost_Article;
					ArtikelOrder_tableEntity.Action_Article = _data[0].Action_Article;
					ArtikelOrder_tableEntity.Article_date = _data[0].Article_date;
					ArtikelOrder_tableEntity.Id_User = _data[0].Id_User;
					ArtikelOrder_tableEntity.TotalCost_Order = _data[0].TotalCost_Order;



				};
				//var InsertedArtikelOrder = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.InsertListArtikelVersionOrder(ArtikelOrderEntity, this.Max_Ver_Ord);
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
