using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.ConditionAssignment
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.ConditionAssignment.ConditionAssignementModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.ConditionAssignment.ConditionAssignementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var konditionsZuordnungstabelleEntities = Infrastructure.Data.Access.Tables.FNC.KonditionsZuordnungstabelleEntity.Get();

				var response = new List<Models.ConditionAssignment.ConditionAssignementModel>();

				foreach(var konditionsZuordnungstabelleEntity in konditionsZuordnungstabelleEntities)
				{
					response.Add(new Models.ConditionAssignment.ConditionAssignementModel(konditionsZuordnungstabelleEntity));
				}

				return ResponseModel<List<Models.ConditionAssignment.ConditionAssignementModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.ConditionAssignment.ConditionAssignementModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.ConditionAssignment.ConditionAssignementModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.ConditionAssignment.ConditionAssignementModel>>.SuccessResponse();
		}
	}
}
