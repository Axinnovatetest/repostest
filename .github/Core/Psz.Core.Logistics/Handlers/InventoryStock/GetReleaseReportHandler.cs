
using Psz.Core.Identity.Models;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class GetReleaseReportHandler: IHandle<UserModel, ResponseModel<string>>
	{
		private readonly UserModel _user;
		private readonly int _data;
		public GetReleaseReportHandler(UserModel user, int data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<string> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				byte[] freigabeReport = UpdateTaskStatusHandler.GetReleaseReportPDF(_data, botransaction);
				if(botransaction.commit())
				{
					return ResponseModel<string>.SuccessResponse(Convert.ToBase64String( freigabeReport));
				}
				else
				{
					return ResponseModel<string>.FailureResponse("Transaction error");
				}
			} catch
			{
				botransaction.rollback();
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(_user == null || (!_user.SuperAdministrator && !_user.IsGlobalDirector && !_user.Access.Logistics.InventoryDownloadRelease))
				return ResponseModel<string>.AccessDeniedResponse();

			return ResponseModel<string>.SuccessResponse();
		}
	}
}
