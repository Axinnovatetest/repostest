using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Plannung
{
	public class FAAnalyseShneidereiKabelGeschnittenDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FAAnalyseShneidereiKabelGeschnittenDataHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				var respose = new FAAnalyseShneidereiKabelGeschnittenDataModel(faEntity);
				return ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>.SuccessResponse(respose);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>.AccessDeniedResponse();
			}

			var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
			if(fa == null)
				return ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>.FailureResponse("Fa not found");

			// - if FA in Lager w/ PPS then it should be started
			if(Module.CTS.LagersWithoutPPS.Contains(fa.Lagerort_id ?? -1) == false && fa.FA_Gestartet != true)
			{
				return ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>.FailureResponse("Fa not found");
			}

			return ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>.SuccessResponse();
		}
	}
}
