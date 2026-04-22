using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateArtikelVersionOrderHandler
	{
		private Models.Budget.InsertedArticleOrderModel _data { get; set; }
		//private Models.Budget.ArtikelOrderParamsModel _data2 { get; set; }
		private int Max_Ver_Ord { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateArtikelVersionOrderHandler(Models.Budget.InsertedArticleOrderModel data, int Max_Ver_Ord, Identity.Models.UserModel user)
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

				/*  _data2.Max_VO = LastVersionOrder.Max_VO;
				  _data2.Nr_version_Order_param = _data.Nr_version_Order_param;
				  _data2.Id_Level_param = _data.Id_Level_param;
				  _data2.Id_Status_param = _data.Id_Status_param;
				  _data2.Id_Dept_param = _data.Id_Dept_param;
				  _data2.Id_Land_param = _data.Id_Land_param;
				  _data2.Dept_name_param = _data.Dept_name_param;
				  _data2.Land_name_param = _data.Land_name_param;
				  _data2.Id_Currency_Order_param = _data.Id_Currency_Order_param;
				  _data2.Id_Supplier_VersionOrder_param = _data.Id_Supplier_VersionOrder_param;
				  _data2.TotalCost_Order_param = _data.TotalCost_Order_param;
				  _data2.Step_Order_param = _data.Step_Order_param;
				  _data2.Id_Project_param = _data.Id_Project_param;*/
				var ArtikelOrderEntity = new Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity()
				{ //Last Version Order

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
					//Artikel
					Id_Order = _data.Id_Order,
					Id_Article = _data.Id_Article,
					Id_Currency_Article = _data.Id_Currency_Article,
					Currency_Article = _data.Currency_Article,
					Id_Dept = _data2[0].Id_Dept_param,
					Dept_name = _data2[0].Dept_name_param,
					Id_Land = _data2[0].Id_Land_param,
					Land_name = _data2[0].Land_name_param,
					Id_Project = _data2[0].Id_Project_param,
					Quantity = _data.Quantity,
					Unit_Price = _data.Unit_Price,
					TotalCost_Article = _data.TotalCost_Article,
					Action_Article = _data.Action_Article,
					Article_date = _data.Article_date,
					Id_User = _data.Id_User,


					//Version Order + _data.TotalCost_Article
					//TotalCost_Order = _data2[0].TotalCost_Order_param ,
					TotalCost_Order = _data.TotalCost_Order,

					/*
                    //Version Order
                    Id_Currency_Order = _data.Id_Currency_Order_param,
                    Id_Level = _data.Id_Level_param,
                    Id_Status = _data.Id_Status_param,
                    Id_Supplier_VersionOrder = _data.Id_Supplier_VersionOrder_param,
                    Id_VO = _data.Id_VO,
                    Nr_version_Order = _data.Nr_version_Order_param,
                    Step_Order = _data.Step_Order_param,
                    TotalCost_Order = _data.TotalCost_Order_param + _data.TotalCost_Article,
                    Version_Order_date = _data.Version_Order_date,
                    //Version Artikel
                    Action_Version_Article = _data.Action_Version_Article,
                    Currency_Version_Article = _data.Currency_Article,
                    Dept_name_VersionArticle = _data.Dept_name_param,
                    Id_AOV = _data.Id_AOV,
                    Id_Currency_Version_Article = _data.Id_Currency_Article,
                    Id_Dept_VersionArticle = _data.Id_Dept_param,
                    Id_Land_VersionArticle = _data.Id_Land_param,
                    Id_Level_VersionArticle = _data.Id_Level_param,
                    Id_Order_Version = _data.Id_VO,
                    Id_Project_VersionArticle = _data.Id_Project_param,
                    Id_Status_VersionArticle = _data.Id_Status_param,
                    Id_Supplier_VersionArticle = _data.Id_Supplier_VersionOrder_param,
                    Id_User_VersionArticle = _data.Id_User,
                    Land_name_VersionArticle = _data.Land_name_param,
                    Quantity_VersionArticle = _data.Quantity,
                    TotalCost__VersionArticle = _data.TotalCost_Article,
                    Unit_Price_VersionArticle = _data.Unit_Price,
                    Version_Article_date = _data.Version_Article_date,*/

				};
				//var InsertedArtikelOrder = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.InsertArtikelVersionOrder(ArtikelOrderEntity, this.Max_Ver_Ord);
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
