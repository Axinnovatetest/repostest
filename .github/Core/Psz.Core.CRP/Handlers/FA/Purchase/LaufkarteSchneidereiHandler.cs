using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class LaufkarteSchneidereiHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public LaufkarteSchneidereiHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				byte[] responseBody = null;

				var LaufkarteSchneidereiEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetLaufkarteSchneiderei(this._data);
				var LaufkarteSchneiderei = LaufkarteSchneidereiEntity?.Select(x => new LaufkarteSchneidereiModel(x)).ToList();
				if(LaufkarteSchneiderei != null && LaufkarteSchneiderei.Count > 0)
				{
					string _temp = string.Empty;
					for(int i = 0; i < LaufkarteSchneiderei.Count; i++)
					{
						for(int j = i + 1; j < LaufkarteSchneiderei.Count; j++)
						{
							_temp = LaufkarteSchneiderei[i].Gewerk;
							if(LaufkarteSchneiderei[j].Gewerk == _temp)
								LaufkarteSchneiderei[j].Gewerk = string.Empty;
							else
								_temp = LaufkarteSchneiderei[j].Gewerk;
						}
					}
				}
				responseBody = Module.CRP_ReportingService.GenerateLaufkarteSchneidereiReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_LAUFKARTE_SCHNEIDEREI, LaufkarteSchneiderei);
				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
