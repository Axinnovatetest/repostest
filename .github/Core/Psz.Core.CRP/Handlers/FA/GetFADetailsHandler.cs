using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFADetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<FADetailsModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFADetailsHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FADetailsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data);
				var faErlidegtEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAErlidgit(faEntity?.ID ?? 1, this._user.Username);
				var technikEntity = Infrastructure.Data.Access.Tables.CTS.Fertigung_PlanungsdetailsAccess.GetByFAId(faEntity.ID);
				var HbgFaPosEntity = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.Get(faEntity?.HBGFAPositionId ?? -1);
				var HbgFaEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(HbgFaPosEntity?.ID_Fertigung ?? -1);
				var faPlannungEntity = Infrastructure.Data.Access.Tables.CTS.Tbl_Planung_gestartet_HauptAccess.GetByFertigungId(faEntity?.ID ?? 1);
				return ResponseModel<FADetailsModel>.SuccessResponse(new FADetailsModel(faEntity, technikEntity, faErlidegtEntity, HbgFaEntity, faPlannungEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<FADetailsModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FADetailsModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data) == null)
			{
				return ResponseModel<FADetailsModel>.NotFoundResponse();
			}

			return ResponseModel<FADetailsModel>.SuccessResponse();
		}
	}
}