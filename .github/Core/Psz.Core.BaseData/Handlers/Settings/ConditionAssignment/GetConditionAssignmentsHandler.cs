using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.ConditionAssignment
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetConditionAssignmentsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetConditionAssignmentsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var konditionsZuordnungstabelleEntities = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get();
				var S = konditionsZuordnungstabelleEntities.OrderBy(t => t.Text != null)
   .ThenByDescending(t => t.Text).ToArray();
				var response = new List<KeyValuePair<int, string>>();

				foreach(var konditionsZuordnungstabelleEntity in S)
				{
					response.Add(new KeyValuePair<int, string>(konditionsZuordnungstabelleEntity.Nr, $"{konditionsZuordnungstabelleEntity.Nr}||{konditionsZuordnungstabelleEntity.Text}"));
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
