using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ConditionAssignment
{
	public class DeleteConditionAssignementHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteConditionAssignementHandler(Identity.Models.UserModel user, int data)
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
				var konditionsZuordnungstabelleEntity = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(this._data);
				var logType = Enums.ObjectLogEnums.LogType.Delete;
				var log = ObjectLogHelper.getLog(this._user, this._data, "Condition Assingement", konditionsZuordnungstabelleEntity.Text, null, Enums.ObjectLogEnums.Objects.Condition_assignement.GetDescription(), logType);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
				var response = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Delete(this._data.ToString());
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
			var konditionsZuordnungstabelleEntity = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(this._data);
			if(konditionsZuordnungstabelleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Condition Assignement Not found" }
						}
				};
			}
			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByConditionAssignement(this._data);
			var exsist2 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByConditionAssignement(this._data);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot delete condition assignement. The following customer and/or suppliers use this condition assignement. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
