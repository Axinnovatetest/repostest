using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteOrderHandler: IHandle<Models.Budget.GetOrdersModel, ResponseModel<int>>
	{
		private int _OrderID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteOrderHandler(int orderid, Identity.Models.UserModel user)
		{
			this._OrderID = orderid;
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

				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(_OrderID);
				if(orderEntity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				orderEntity.Deleted = true;
				orderEntity.DeleteTime = DateTime.Now;
				orderEntity.DeleteUserId = this._user.Id;
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			/* if (this._user == null
				 || !this._user.Access.Budget.ConfigDeleteArtikel)
			 {
				 return ResponseModel<int>.AccessDeniedResponse();
			 }*/
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
