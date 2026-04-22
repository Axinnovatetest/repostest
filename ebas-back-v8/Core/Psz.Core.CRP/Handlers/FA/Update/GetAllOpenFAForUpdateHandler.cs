using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.CRP.Models.FA.Update;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public class GetAllOpenFAForUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<AllOpenFAForUpdateModel>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetAllOpenFAForUpdateHandler(string data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<AllOpenFAForUpdateModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				AllOpenFAForUpdateModel response = new AllOpenFAForUpdateModel();
				var articleentity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
				var LagerWithVersionning = Module.LagersWithVersionning;
				var updatesNotVersionningEntities = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetOpenFANotVersionning(articleentity.ArtikelNr, LagerWithVersionning);
				var updatesVersionningEntities = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetOpenFAVersionning(articleentity.ArtikelNr, LagerWithVersionning);

				var WithoutVersionning = updatesNotVersionningEntities?.Select(x => new OpenFAForUpdateModel(x))?.OrderBy(x => x.Fertigungsnummer)?.ToList();
				var WithVersionning = updatesVersionningEntities?.Select(x => new OpenFAForUpdateModel(x))?.OrderBy(x => x.Fertigungsnummer)?.ToList();

				response = new AllOpenFAForUpdateModel { FAWithoutVersionning = WithoutVersionning, FAWithVersionning = WithVersionning, KundenIndex = false, Stucklisten = false };

				return ResponseModel<AllOpenFAForUpdateModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<AllOpenFAForUpdateModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<AllOpenFAForUpdateModel>.AccessDeniedResponse();
			}
			var articleentity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
			if(articleentity == null)
				return ResponseModel<AllOpenFAForUpdateModel>.FailureResponse(key: "1", value: $"Article not found");
			return ResponseModel<AllOpenFAForUpdateModel>.SuccessResponse();
		}
	}
}