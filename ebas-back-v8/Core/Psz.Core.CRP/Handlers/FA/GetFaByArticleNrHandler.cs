using Infrastructure.Data.Access.Tables.PRS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFaByArticleNrHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<int>>>
	{
		private KeyValuePair<int, string> _data;
		private Identity.Models.UserModel _user { get; set; }

		public GetFaByArticleNrHandler(KeyValuePair<int, string> data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<List<int>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			return ResponseModel<List<int>>.SuccessResponse(FertigungAccess.GetByArtikelNr(this._data.Key, this._data.Value));

		}

		public ResponseModel<List<int>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<int>>.AccessDeniedResponse();
			}
			return ResponseModel<List<int>>.SuccessResponse();

		}
	}
}
