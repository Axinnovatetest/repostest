using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{

	// -- 12-02-2024
	public class BiggestTimeDifferenceBetweenTodayHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<OrderTypesResponseModel>>>
	{

		public Psz.Core.Identity.Models.UserModel _user { get; set; }

		public BiggestTimeDifferenceBetweenTodayHandler(UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<OrderTypesResponseModel>> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();

				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}

				// -- logic here request DAL Request -- order has delay



				return ResponseModel<List<OrderTypesResponseModel>>.SuccessResponse();

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}


		}

		public ResponseModel<List<OrderTypesResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<OrderTypesResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<OrderTypesResponseModel>>.SuccessResponse();
		}
	}
}
