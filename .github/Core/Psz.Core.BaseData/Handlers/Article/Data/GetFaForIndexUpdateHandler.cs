using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class GetFaForIndexUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private KeyValuePair<int, string> _data { get; set; }
		public GetFaForIndexUpdateHandler(Identity.Models.UserModel user, KeyValuePair<int, string> data)
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

			// - 
			var originalArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Key);
			var sameOldCustomerIndex = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemIndex(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer.Substring(0,originalArticleEntity.ArtikelNummer.IndexOf('-')), originalArticleEntity.CustomerItemNumber, originalArticleEntity.CustomerIndex);
			var doneFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetDoneByArticleAndIndex(sameOldCustomerIndex.Select(x => x.ArtikelNr).ToList(), originalArticleEntity.Index_Kunde, null);
			if(doneFAs != null && doneFAs.Count > 0)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
							doneFAs.Select(x => new KeyValuePair<int, string>(x.ID, $"{x.Fertigungsnummer}")).Distinct().ToList()
							);
			}
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithIndex(this._data.Key, this._data.Value) == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse($"Article with index [{this._data.Value}] not found");
			}

			// -
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
