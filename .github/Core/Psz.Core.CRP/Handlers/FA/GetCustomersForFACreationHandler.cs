using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetCustomersForFACreationHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		public UserModel _user { get; }
		public GetCustomersForFACreationHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var customers = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetCustomersForFACreation();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(customers);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(_user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}