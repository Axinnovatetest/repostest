using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetActiveStucklistedArticleNumbersHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.ActiveStucklistedANumberRequestModel _data { get; set; }
		public GetActiveStucklistedArticleNumbersHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.ActiveStucklistedANumberRequestModel data)
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

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetActiveStucklisted(searchTerms: this._data.SearchTerms, currentNumber: this._data.CurrentArticleNumber)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>())
					?.Select(x => new KeyValuePair<int, string>(x.ArtikelNr, x.ArtikelNummer))
					?.ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
