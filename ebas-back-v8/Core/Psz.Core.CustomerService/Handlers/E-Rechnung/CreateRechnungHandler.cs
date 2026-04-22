using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class ForceRechnungAutoCreationHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<InvoicesCreationSummaryModel>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public ForceRechnungAutoCreationHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public async Task<ResponseModel<InvoicesCreationSummaryModel>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var block = Psz.Core.Common.Helpers.blockHelper.GetBlockState().LS;
				if(block)
					return ResponseModel<InvoicesCreationSummaryModel>.FailureResponse("Delivery note(s) are in creation, please try again in a moment .");
				List<string> errors = new List<string>();
				var result = await CronJobs.Configuration.CreateInvoices(errors, _user);
				result.Creationresult = 1;

				return new ResponseModel<InvoicesCreationSummaryModel>
				{
					Success = true,
					Body = result,
					Warnings = errors
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<ResponseModel<InvoicesCreationSummaryModel>> ValidateAsync()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return await ResponseModel<InvoicesCreationSummaryModel>.AccessDeniedResponseAsync();
			}
			return await ResponseModel<InvoicesCreationSummaryModel>.SuccessResponseAsync();
		}
	}
}
