using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Settings
{
	public class GetWunchWerkLogHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<FAWunchWerkLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetWunchWerkLogHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<FAWunchWerkLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<FAWunchWerkLogModel> response = new List<FAWunchWerkLogModel>();
				var logEntiy = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.Get();
				if(logEntiy != null && logEntiy.Count > 0)
					response = logEntiy.Select(x => new FAWunchWerkLogModel(x)).OrderByDescending(y => y.Dateupdate).ToList();

				return ResponseModel<List<FAWunchWerkLogModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<FAWunchWerkLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<FAWunchWerkLogModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<FAWunchWerkLogModel>>.SuccessResponse();
		}
	}
}