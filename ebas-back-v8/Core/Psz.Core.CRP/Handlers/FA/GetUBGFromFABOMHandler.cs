using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetUBGFromFABOMHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Models.FA.UBGFromFaBOMRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetUBGFromFABOMHandler(Models.FA.UBGFromFaBOMRequestModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var bomEntities = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(this._data.FaId);
				if(this._data.ForCancel == false)
				{
					bomEntities = bomEntities?.Where(x => !x.UBGFertigungsId.HasValue || x.UBGFertigungsId.Value <= 0)?.ToList();
				}
				else
				{
					bomEntities = bomEntities?.Where(x => x.UBGFertigungsId.HasValue && x.UBGFertigungsId.Value > 0)?.ToList();
				}

				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomEntities.Select(x => x.Artikel_Nr ?? -1).ToList())
					.Where(x => x.Warengruppe?.ToLower() == "ef" && x.UBG == true)?.ToList();

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(articleEntities.Select(x =>
				new KeyValuePair<int, string>(x.ArtikelNr, $"{x.ArtikelNummer}")).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.FaId);
			if(faEntity == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse($"FA not found");
			}
			var bomEntities = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(this._data.FaId);
			if(bomEntities == null || bomEntities.Count <= 0)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse($"FA BOM empty");
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}