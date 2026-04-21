using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateOrderHandler
	{


		private Models.Budget.InsertedDataOrderModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateOrderHandler(Models.Budget.InsertedDataOrderModel data, Identity.Models.UserModel user)
		{
			this._data = data;
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
				var OrderEntity = new Infrastructure.Data.Entities.Tables.FNC.Budget_OrderInsertedEntity()
				{

					Id_Order = _data.Id_Order,
					Order_Number = _data.Order_Number,
					Type_Order = _data.Type_Order,
					Id_Project = _data.Id_Project,
					Id_Dept = _data.Id_Dept,
					Dept_name = _data.Dept_name,
					Id_Land = _data.Id_Land,
					Land_name = _data.Land_name,
					Id_Currency_Order = _data.Id_Currency_Order,
					Id_Supplier = _data.Id_Supplier,
					Id_User = _user.Id,
					Order_date = _data.Order_date,
					Id_VO = _data.Id_VO,
					Nr_version_Order = _data.Nr_version_Order,
					Id_Level = _data.Id_Level,
					Id_Status = _data.Id_Status,
					Step_Order = _data.Step_Order,
					Id_Supplier_VersionOrder = _data.Id_Supplier,
					TotalCost_Order = _data.TotalCost_Order,
					Version_Order_date = _data.Version_Order_date,
					Id_Diverse_Supplier_Order = _data.Id_Diverse_Supplier_Order,
					Id_Order_Diverse = _data.Id_Order,
					Id_Supplier_Order_Diverse = _data.Id_Supplier,
					Lieferantennummer_Order_Diverse = _data.Lieferantennummer_Order_Diverse,
					Ort_Order_Supplier_Diverse = _data.Ort_Order_Supplier_Diverse,
					Supplier_Contact_Description_Order_Diverse = _data.Supplier_Contact_Description_Order_Diverse,
					Supplier_Contact_Order_Diverse = _data.Supplier_Contact_Order_Diverse,
					Supplier_Name_Order_Diverse = _data.Supplier_Name_Order_Diverse,
				};
				//var InsertedOrder = Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.InsertOrder(OrderEntity);



				return ResponseModel<int>.SuccessResponse(/*InsertedOrder*/0);


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
