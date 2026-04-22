using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetOrderByIdOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.AllDataOrderModel>>

	{

		private int _data { get; set; }

		public GetOrderByIdOrderHandler(int id)
		{

			this._data = id;

		}
		public ResponseModel<Models.Budget.AllDataOrderModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.AllDataOrderModel();

				//var budgetOrder = Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.Get(this._data);
				//responseBody.Id_Order = budgetOrder.Id_Order;
				//responseBody.Order_Number = budgetOrder.Order_Number;
				//responseBody.Type_Order = budgetOrder.Type_Order;
				//responseBody.Id_Project = budgetOrder.Id_Project;
				//responseBody.Id_Supplier = budgetOrder.Id_Supplier;
				//responseBody.Id_Currency_Order = budgetOrder.Id_Currency_Order;
				//responseBody.Id_Dept = budgetOrder.Id_Dept;
				//responseBody.Dept_name = budgetOrder.Dept_name;
				//responseBody.Id_Land = budgetOrder.Id_Land;
				//responseBody.Land_name = budgetOrder.Land_name;
				//responseBody.Id_User = budgetOrder.Id_User;
				//responseBody.Order_date = budgetOrder.Order_date;


				return ResponseModel<Models.Budget.AllDataOrderModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.AllDataOrderModel> Validate()
		{

			return ResponseModel<Models.Budget.AllDataOrderModel>.SuccessResponse();


		}

	}
}
