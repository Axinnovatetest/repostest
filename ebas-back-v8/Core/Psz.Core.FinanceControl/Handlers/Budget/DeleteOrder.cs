//using Psz.Core.Common.Models;
//using Psz.Core.SharedKernel.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Psz.Core.FinanceControl.Handlers.Budget
//{
//    public class DeleteOrderHandler : IHandle<Models.Budget.InsertedDataOrderModel, ResponseModel<int>>
//    {
//        private Models.Budget.InsertedDataOrderModel _data { get; set; }
//        private Identity.Models.UserModel _user { get; set; }
//        public DeleteOrderHandler(Models.Budget.InsertedDataOrderModel data, Identity.Models.UserModel user)
//        {
//            this._data = data;
//            this._user = user;
//        }
//        public ResponseModel<int> Handle()
//        {
//            try
//            {
//                var validationResponse = this.Validate();
//                if (!validationResponse.Success)
//                {
//                    return validationResponse;
//                }
//                var Orderentity = new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderInsertedEntity()
//                {


//                        Type_Order = _data.Type_Order,
//                        Id_Project = _data.Id_Project,
//                        Id_Dept = _data.Id_Dept,
//                        Dept_name = _data.Dept_name,
//                        Id_Land = _data.Id_Land,
//                        Land_name = _data.Land_name,
//                        Id_Currency_Order = _data.Id_Currency_Order,
//                        Id_Supplier = _data.Id_Supplier,
//                        Id_User = _data.Id_User,
//                        Order_date = _data.Order_date,
//                        Id_VO = _data.Id_VO,
//                        Nr_version_Order = _data.Nr_version_Order,
//                        Id_Level = _data.Id_Level,
//                        Id_Status = _data.Id_Status,
//                        Step_Order = _data.Step_Order,
//                        Id_Supplier_VersionOrder = _data.Id_Supplier_VersionOrder,
//                        TotalCost_Order = _data.TotalCost_Order,
//                        Version_Order_date = _data.Version_Order_date,
//                        Id_Diverse_Supplier_Order = _data.Id_Diverse_Supplier_Order,
//                        Id_Order_Diverse = _data.Id_Order_Diverse,
//                        Id_Supplier_Order_Diverse = _data.Id_Supplier_Order_Diverse,
//                        Lieferantennummer_Order_Diverse = _data.Lieferantennummer_Order_Diverse,
//                        Ort_Order_Supplier_Diverse = _data.Ort_Order_Supplier_Diverse,
//                        Supplier_Contact_Description_Order_Diverse = _data.Supplier_Contact_Description_Order_Diverse,
//                        Supplier_Contact_Order_Diverse = _data.Supplier_Contact_Order_Diverse,
//                        Supplier_Name_Order_Diverse = _data.Supplier_Name_Order_Diverse,
//            };
//                var orderentity = Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.Get(_data.Id_Order);
//                var ordersArtikelsentity = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.GetArtikelOrder(_data.Id_Order);
//                if (orderentity == null)
//                {
//                    return ResponseModel<int>.SuccessResponse();
//                }

//                return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.Delete(orderentity.Id_Order));
//            }
//            catch (Exception e)
//            {
//                Infrastructure.Services.Logging.Logger.Log(e);
//                throw e;
//            }
//        }
//        public ResponseModel<int> Validate()
//        {

//            var OrderID = Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.Get(this._data.Id_Order);
//            var CountDepts = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetDeptsCount(this._data.Land_name,this._data.B_year);
//            var landname = this._data.Land_name;
//            var year = this._data.B_year;
//            var errors = new List<ResponseModel<int>.ResponseError>();
//            if (BudgetLandID == null)
//            {
//                return new ResponseModel<int>()
//                {
//                    Errors = new List<ResponseModel<int>.ResponseError>() {
//                        new ResponseModel<int>.ResponseError {Key = "", Value = "Land assignement not found"}
//                    }
//                };
//            }

//            if (CountDepts>0)
//            {
//                return new ResponseModel<int>()
//                {
//                    Errors = new List<ResponseModel<int>.ResponseError>() {
//                        new ResponseModel<int>.ResponseError {Key = "", Value = "You can't Delete A land with assigned departements" }
//                    }
//                };
//            }
//            if (errors.Count > 0)
//            {
//                return new ResponseModel<int>() { Errors = errors };
//            }
//            return ResponseModel<int>.SuccessResponse();
//        }
//    }
//}
