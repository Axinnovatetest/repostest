using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Settings
{
	public class FAErlidgtLogHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<FAErlidgtLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public FAErlidgtLogHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<FAErlidgtLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<FAErlidgtLogModel> response = new List<FAErlidgtLogModel>();
				var logEntiy = Infrastructure.Data.Access.Tables.CTS.PSZ_FA_erledigen_HilfstabelleAccess.Get();
				if(logEntiy != null && logEntiy.Count > 0)
					response = logEntiy.Select(x => new FAErlidgtLogModel(x)).ToList();

				return ResponseModel<List<FAErlidgtLogModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<FAErlidgtLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<FAErlidgtLogModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<FAErlidgtLogModel>>.SuccessResponse();
		}
	}
}