using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetListFAStatusHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private UserModel _user { get; set; }
		public GetListFAStatusHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var faStatusEntites = Enum.GetValues(typeof(Enums.FAEnums.FaStatus)).Cast<Enums.FAEnums.FaStatus>().ToList();
				if(faStatusEntites != null && faStatusEntites.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(faStatusEntites
							.Select(x => new KeyValuePair<string, string>(x.ToString(), $"{x.GetDescription()}".Trim())).Distinct().ToList());
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}