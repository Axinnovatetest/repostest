using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;


namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class CheckForceRechnungTimeHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public CheckForceRechnungTimeHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var result = "Are you sure you want to proceed ?";
				//var currentDate = DateTime.Now;
				//var connection = JobStorage.Current.GetConnection();
				//var currentJob = connection.GetRecurringJobs().FirstOrDefault(p => p.Id.Contains("CreateInvoices"));
				//var lastFired = currentJob.LastExecution;
				//var nextFire = currentJob.NextExecution;

				//if(nextFire > currentDate)
				//	result = $"The Automatic Bill Creation Agent will be Executed At {nextFire}, Forcing this action could lead to creation of further Bills, Are you sure you want to proceed ?";
				//else
				//	result = $"The Automatic Bill Creation Agent Has Already been Executed At {lastFired},Forcing this action could lead to creation of further Bills, Are you sure you want to proceed ?";

				return ResponseModel<string>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<string> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			return ResponseModel<string>.SuccessResponse();
		}

	}
}
