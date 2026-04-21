using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Psz.Core.ManagementOverview.ProductionWorkload.Handlers
{
	public class RefreshWorkloadHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public RefreshWorkloadHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				var result = Infrastructure.Data.Access.Tables.MGO.ProductionWorkloadAccess.RefreshWorkload(_user.Id, _data);
				return ResponseModel<int>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<int> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// -
			if(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data) == null)
			{
				return ResponseModel<int>.FailureResponse($"Warehouse not found");
			}
			// - 
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
