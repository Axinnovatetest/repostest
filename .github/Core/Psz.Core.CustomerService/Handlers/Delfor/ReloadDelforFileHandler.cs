using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class ReloadDelforFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ReloadDelforFileHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var error = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Get(_data);
				if(error != null)
				{
					var file = error.FileName;
					var customerDocumentErrors = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetByDocumentNumber(error.Documentnumber);
					customerDocumentErrors.ForEach(x =>
					{
						x.Validated = true;
						x.ValidationTime = DateTime.Now;
						x.ValidationUserId = _user.Id;
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
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}



	}
}
