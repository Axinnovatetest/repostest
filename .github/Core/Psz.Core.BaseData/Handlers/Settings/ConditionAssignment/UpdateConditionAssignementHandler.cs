using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ConditionAssignment
{
	public class UpdateConditionAssignementHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.ConditionAssignment.ConditionAssignementModel _data { get; set; }
		public UpdateConditionAssignementHandler(Identity.Models.UserModel user, Models.ConditionAssignment.ConditionAssignementModel data)
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
				var response = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Update(_entity);
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
			var conditionAssingementEntity = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(this._data.Id);
			if(conditionAssingementEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Conition Assignement not found" }
						}
				};
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
			var theRest = conditionsAssignementEntities.Where(x => x.Nr != this._data.Id);
			var check = theRest.Where(x => x.Text == this._data.Text);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A condition Assignement with the same Name already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByConditionAssignement(this._data.Id);
			var exsist2 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByConditionAssignement(this._data.Id);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update condition assignement. The following customer and/or suppliers use this condition assignement. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
