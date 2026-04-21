using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class VerifyFALaufkarteHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public VerifyFALaufkarteHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				if(faEntity == null)
					return ResponseModel<string>.FailureResponse($"FA dosen't exsist");
				//if (faEntity.Anzahl_aktuell == 0)
				//    return ResponseModel<string>.FailureResponse($"FA quantity is 0");
				var LaufkarteSchneidereiEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetLaufkarteSchneiderei(this._data);
				if(LaufkarteSchneidereiEntity == null || LaufkarteSchneidereiEntity.Count == 0)
					return ResponseModel<string>.FailureResponse($"LaufkarteSchneiderei empty for this FA");

				return ResponseModel<string>.SuccessResponse("");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}
			return ResponseModel<string>.SuccessResponse("");
		}
	}
}