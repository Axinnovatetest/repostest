using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAUpdateLagersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAUpdateLagersHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				List<KeyValuePair<int, int>> response = new List<KeyValuePair<int, int>>();
				var faLagersEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAUpdateLagers(this._data);
				if(faLagersEntity != null && faLagersEntity.Count > 0)
					response = faLagersEntity.Select(x => new KeyValuePair<int, int>(x, x)).ToList();

				return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.AccessDeniedResponse();
			}
			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}
	}
}