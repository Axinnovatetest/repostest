using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAErlidigtHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAErlidigtHandler(int data, Identity.Models.UserModel user)
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
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data);
				if(faEntity == null)
					return ResponseModel<int>.FailureResponse("Anzahl zu groß");
				if(faEntity.Lagerort_id == 156)
					return ResponseModel<int>.FailureResponse("Closing for warehouse 156 is currently blocked, check with IT service .");
				if((!faEntity.FA_Gestartet.HasValue || (faEntity.FA_Gestartet.HasValue && !faEntity.FA_Gestartet.Value)) && faEntity.Lagerort_id != 15 && faEntity.Lagerort_id != 156)
					return ResponseModel<int>.FailureResponse("Fertigungsauftrag ist nicht Gestartet");
				if(faEntity.Kennzeichen.ToLower() == "storno")
					return ResponseModel<int>.FailureResponse("FA ist storniert!");
				if(faEntity.Kennzeichen.ToLower() == "erledigt")
					return ResponseModel<int>.FailureResponse("FA ist bereits erledigt!");
				if(faEntity.Kennzeichen.ToLower() == "gesperrt")
					return ResponseModel<int>.FailureResponse("FA ist noch nicht gebucht!");

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
			return ResponseModel<int>.SuccessResponse();
		}
	}
}