using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class ArtikelOrderHandler
	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public ArtikelOrderHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;

		}
		public ResponseModel<List<Models.Budget.ArtikelOrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.ArtikelOrderModel>();

				//var artikel_order = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.GetByOrderId(this._data);


				//if (artikel_order != null)
				//{
				//    foreach (var ord_artikel in artikel_order)
				//    {
				//        responseBody.Add(new Models.Budget.ArtikelOrderModel(ord_artikel));
				//    }
				//}

				return ResponseModel<List<Models.Budget.ArtikelOrderModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.ArtikelOrderModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.ArtikelOrderModel>>.SuccessResponse();


		}

	}
}
