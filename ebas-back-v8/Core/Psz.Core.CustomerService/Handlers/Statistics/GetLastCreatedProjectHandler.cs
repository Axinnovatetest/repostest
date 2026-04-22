using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetLastCreatedProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<LastCreatedProectModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetLastCreatedProjectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<LastCreatedProectModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var nb1 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats3();
				LastCreatedProectModel response = new LastCreatedProectModel();
				if(nb1 != 0)
					response.LastCreatedProject = nb1;
				return ResponseModel<LastCreatedProectModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<LastCreatedProectModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<LastCreatedProectModel>.AccessDeniedResponse();
			}
			return ResponseModel<LastCreatedProectModel>.SuccessResponse();
		}
	}
}
