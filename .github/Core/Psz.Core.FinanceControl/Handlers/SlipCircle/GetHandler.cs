using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.SlipCircle
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.SlipCircle.SlipCircleModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.SlipCircle.SlipCircleModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var belegkreiseVorgabenEntities = Infrastructure.Data.Access.Tables.FNC.BelegkreiseVorgabenAccess.Get();

				var response = new List<Models.SlipCircle.SlipCircleModel>();

				foreach(var belegkreiseVorgabenEntity in belegkreiseVorgabenEntities)
				{
					response.Add(new Models.SlipCircle.SlipCircleModel(belegkreiseVorgabenEntity));
				}

				return ResponseModel<List<Models.SlipCircle.SlipCircleModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.SlipCircle.SlipCircleModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.SlipCircle.SlipCircleModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.SlipCircle.SlipCircleModel>>.SuccessResponse();
		}
	}
}
