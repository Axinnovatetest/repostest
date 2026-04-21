using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.Delfor
{
	public partial class DelforService
	{
		public ResponseModel<int> ReloadDelforFile(Identity.Models.UserModel user, int data)
		{
			try
			{
				var validationResponse = this.ValidateReloadDelforFile(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var error = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Get(data);
				if(error != null)
				{
					var file = error.FileName;
					var customerDocumentErrors = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetByDocumentNumber(error.Documentnumber);
					customerDocumentErrors.ForEach(x =>
					{
						x.Validated = true;
						x.ValidationTime = DateTime.Now;
						x.ValidationUserId = user.Id;
					});
					Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Update(customerDocumentErrors);
					try
					{
						Helpers.DelforHelper.moveErrorToNewFile(file);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						return ResponseModel<int>.FailureResponse("Error occured when trasfering file.");
						throw;
					}
				}

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> ValidateReloadDelforFile(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}