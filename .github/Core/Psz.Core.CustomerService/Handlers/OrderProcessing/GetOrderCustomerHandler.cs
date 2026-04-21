using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetOrderCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<CustomerModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetOrderCustomerHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<CustomerModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)orderDb.Kunden_Nr);
				var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)orderDb.Kunden_Nr);

				return ResponseModel<CustomerModel>.SuccessResponse(new CustomerModel(customerDb, adressDb));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<CustomerModel> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<CustomerModel>.AccessDeniedResponse();
			}

			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(orderDb == null)
				return ResponseModel<CustomerModel>.FailureResponse(key: "1", value: $"Order not found");
			if(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr ?? -1) == null)
				return ResponseModel<CustomerModel>.FailureResponse(key: "1", value: $"Customer not found");

			return ResponseModel<CustomerModel>.SuccessResponse();
		}
	}
}
