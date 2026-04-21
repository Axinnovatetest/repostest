using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAUpdateLastCPVersionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int?>>
	{
		private string _data { get; set; }
		private int _data2 { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAUpdateLastCPVersionHandler(string data, int data2, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
			this._data2 = data2;
		}
		public ResponseModel<int?> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
				var lastCPEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetMaxCPVersionByBomVersion(articleEntity?.ArtikelNr ?? -1, this._data2);
				return ResponseModel<int?>.SuccessResponse(lastCPEntity?.CP_version ?? null);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data},_data2:{_data2}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int?> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int?>.AccessDeniedResponse();
			}
			return ResponseModel<int?>.SuccessResponse();
		}
	}
}