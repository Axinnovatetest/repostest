using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ConditionAssignment
{
	public class AddConditionAssignementHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.ConditionAssignment.ConditionAssignementModel _data { get; set; }
		public AddConditionAssignementHandler(Identity.Models.UserModel user, Models.ConditionAssignment.ConditionAssignementModel data)
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

				var _entity = this._data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Insert(_entity);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(string.IsNullOrEmpty(this._data.Text))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Text should not be empty" }
						}
				};
			}
			var conditionsAssignementEntities = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get();
			var check = conditionsAssignementEntities.Where(x => x.Text == this._data.Text);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A condition Assignement with the same Name already exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
