using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{

	public class GetInvoiceAmountHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<InvoiceAmountResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetInvoiceAmountHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<InvoiceAmountResponseModel>> Handle()
		{
			try
			{
				//var validationResponse = this.Validate();
				//if(!validationResponse.Success)
				//{
				//	return validationResponse;
				//}



				List<InvoiceAmountResponseModel> response = new List<InvoiceAmountResponseModel>();

				List<Infrastructure.Data.Entities.Tables.Statistics.MGO.InvoiceAmountEntity> items =
					Infrastructure.Data.Access.Tables.Statistics.PeriodicSalesAccess.GetInvoiceAmount();
				foreach(var item in items)
				{
					response.Add(new InvoiceAmountResponseModel
					{
						Amount = item.Amount,
						InvoiceDate = item.Belegdatum
					});
				}

				//response.Reverse();
				return ResponseModel<List<InvoiceAmountResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<InvoiceAmountResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<InvoiceAmountResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<InvoiceAmountResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<InvoiceAmountResponseModel>>.SuccessResponse();
		}

	}
}
