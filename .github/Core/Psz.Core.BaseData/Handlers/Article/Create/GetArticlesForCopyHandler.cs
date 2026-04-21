using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public partial class GetArticlesForCopyHandler: IHandle<string, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }

		public GetArticlesForCopyHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var results = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumber(this._data, false);

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					results?.Select(x => new KeyValuePair<int, string>(x.ArtikelNr, x.ArtikelNummer))?.Distinct()?.ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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

			// -
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
