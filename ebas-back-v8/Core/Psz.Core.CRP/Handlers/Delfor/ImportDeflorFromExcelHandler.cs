using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Delfor;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.Delfor
{
	public partial class DelforService
	{
		public ResponseModel<List<DeliveryForcastLineItemModel>> ImportDeflorFromExcel(Identity.Models.UserModel user, ImportFileModel data)
		{
			try
			{
				var validationResponse = this.ValidateImportDeflorFromExcel(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var commaSeperator = data.CommaSeperator == "true" ? true : false;
				var checkFrequency = data.CheckFrequency == "true" ? true : false;
				var excleResult = Helpers.DelforHelper.ReadDelforFromExcel(data.FilePath, out List<string> errors, commaSeperator, checkFrequency);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<DeliveryForcastLineItemModel>>.FailureResponse(errors);

				Helpers.DelforHelper.ValidateDelforData(excleResult, out List<string> warnings, user);
				var response = new ResponseModel<List<DeliveryForcastLineItemModel>>
				{
					Body = excleResult,
					Errors = null,
					Infos = null,
					Success = true,
					Warnings = warnings
				};

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DeliveryForcastLineItemModel>> ValidateImportDeflorFromExcel(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DeliveryForcastLineItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DeliveryForcastLineItemModel>>.SuccessResponse();
		}
	}
}