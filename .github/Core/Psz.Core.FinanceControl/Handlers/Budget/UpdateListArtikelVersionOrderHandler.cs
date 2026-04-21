using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateListArtikelVersionOrderHandler: IHandle<Models.Budget.InsertedListArticleOrderModel, ResponseModel<int>>
	{
		private Models.Budget.InsertedListArticleOrderModel _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }
		private int Max_Ver_Ord { get; set; }
		private int updatedArtikelOrder;

		public UpdateListArtikelVersionOrderHandler(Models.Budget.InsertedListArticleOrderModel data, int Max_Ver_Ord, Identity.Models.UserModel user)
		{
			this._data = data;
			this.Max_Ver_Ord = Max_Ver_Ord;
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

				var ArtikelOrderEntity = new Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity();
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
					ArtikelOrderEntity.Id_AO = Artikel.Id_AO;
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
					//foreach (var ArtikelOrder in _data.ArtikelsOrder)
					//{

					//    updatedArtikelOrder = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.UpdateArtikelVersionOrder(ArtikelOrder.ToBudgetListArtikelsOrder(ArtikelOrderEntity.Id_Order), this.Max_Ver_Ord);

					//}

				}
				return ResponseModel<int>.SuccessResponse(/*updatedArtikelOrder*/0);
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
