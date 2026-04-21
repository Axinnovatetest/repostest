using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetTechinicansHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Production.TechnicianModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetTechinicansHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Production.TechnicianModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var techinicianEntities = Infrastructure.Data.Access.Tables.PRS.TechnikerAccess.Get();

				var response = new List<Models.Production.TechnicianModel>();

				foreach(var technicianEntity in techinicianEntities)
				{
					response.Add(new Models.Production.TechnicianModel(technicianEntity));
				}

				return ResponseModel<List<Models.Production.TechnicianModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Production.TechnicianModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Production.TechnicianModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Production.TechnicianModel>>.SuccessResponse();
		}
	}
}
