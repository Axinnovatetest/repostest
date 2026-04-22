using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Settings
{
	public class GetTerminFAUpdateLogHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<TerminFAUpdateLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _fa { get; set; }
		public GetTerminFAUpdateLogHandler(Identity.Models.UserModel user, int fa)
		{
			this._user = user;
			this._fa = fa;
		}
		public ResponseModel<List<TerminFAUpdateLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<TerminFAUpdateLogModel> response = new List<TerminFAUpdateLogModel>();
				var logEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetByFA(this._fa);
				if(logEntity != null && logEntity.Count > 0)
					response = logEntity.Select(x => new TerminFAUpdateLogModel(x)).ToList();

				return ResponseModel<List<TerminFAUpdateLogModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _fa:{_fa}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<TerminFAUpdateLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<TerminFAUpdateLogModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<TerminFAUpdateLogModel>>.SuccessResponse();
		}
	}
}
