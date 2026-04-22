using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers
{
	public class CheckHorizonTestHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private DateTime _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CheckHorizonTestHandler(Identity.Models.UserModel user, DateTime data)
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

				//var check = Helpers.FAHorizonsHelper.CanCreateOrUpdate(_data, Enums.FAEnums.createOrUpdate.create, _user, out List<string> messages);
				//if(!check)
				//	return ResponseModel<int>.FailureResponse(messages);

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
