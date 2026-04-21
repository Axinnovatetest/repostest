using Hangfire;
using Hangfire.Storage;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class CheckForceCreationAllowdHandler: IHandle<Identity.Models.UserModel, ResponseModel<CheckForceCreationAllowdModel>>
	{


		private Identity.Models.UserModel _user { get; set; }
		public CheckForceCreationAllowdHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<CheckForceCreationAllowdModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new CheckForceCreationAllowdModel();
				response.forceCreationAllowed = true;
				response.message = "";

				var currentDate = DateTime.Now;
				var connection = JobStorage.Current.GetConnection();
				var currentJob = connection.GetRecurringJobs().FirstOrDefault(p => p.Id.Contains("CreateInvoices"));
				var nextFire = currentJob?.NextExecution;

				if(nextFire.HasValue)
				{
					var cronTime = nextFire.Value.TimeOfDay;
					var cronTimePlus30 = nextFire.Value.AddMinutes(30).TimeOfDay;
					var cronTimeMinus30 = nextFire.Value.AddMinutes(-30).TimeOfDay;

					var currenTime = DateTime.Now.TimeOfDay;

					if((currenTime >= cronTimeMinus30 && currenTime < cronTime) ||
						(currenTime <= cronTimePlus30 && currenTime > cronTime))
					{
						var remainingTime = (cronTimePlus30 - currenTime).TotalMinutes;
						response.forceCreationAllowed = false;
						response.message = $"Forcing automatic bill creation is not allowed at the moment, please try again in {Math.Ceiling(remainingTime)} minute(s)";
					}
				}

				return ResponseModel<CheckForceCreationAllowdModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<CheckForceCreationAllowdModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<CheckForceCreationAllowdModel>.AccessDeniedResponse();
			}

			return ResponseModel<CheckForceCreationAllowdModel>.SuccessResponse();
		}
	}
}
